package ru.softwerke.querybuilder.core.service.impl;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.MapperFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;
import com.fasterxml.jackson.datatype.jsr310.deser.LocalDateTimeDeserializer;
import com.fasterxml.jackson.datatype.jsr310.ser.LocalDateTimeSerializer;
import org.apache.commons.lang3.RandomStringUtils;
import org.apache.commons.lang3.StringUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.AnonymousAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;
import ru.softwerke.querybuilder.core.config.ApplicationConfiguration;
import ru.softwerke.querybuilder.core.data.queryForm.Info;
import ru.softwerke.querybuilder.core.data.queryForm.QueryForm;
import ru.softwerke.querybuilder.core.data.queryForm.QueryFormViews;
import ru.softwerke.querybuilder.core.exception.QueryFormIOException;
import ru.softwerke.querybuilder.core.service.MetadataService;
import ru.softwerke.querybuilder.core.service.QueryFormService;
import ru.softwerke.querybuilder.integration.jpa.dto.ConnectionType;

import javax.naming.AuthenticationException;
import java.io.File;
import java.io.IOException;
import java.io.RandomAccessFile;
import java.nio.channels.FileChannel;
import java.nio.channels.FileLock;
import java.nio.channels.OverlappingFileLockException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.OpenOption;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;

import static java.nio.file.StandardOpenOption.CREATE_NEW;
import static java.nio.file.StandardOpenOption.WRITE;

/*
 *  @author Svetlana Suvorova
 */
@Service
public class QueryFormServiceImpl implements QueryFormService {

    @Autowired
    private MetadataService metadataService;

    private static final String QUERY_FORM_FILE_PREFIX = "form";
    private static final String QUERY_FORM_FILE_EXTENSION = "json";
    private static final String CSS_FILE_EXTENSION = "css";
    private static final OpenOption[] options = new OpenOption[] { WRITE, CREATE_NEW };
    private static final String QUERY_FORM_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
    private static final DateTimeFormatter formatter = DateTimeFormatter.ofPattern(QUERY_FORM_DATE_FORMAT);

    public ObjectMapper mapper = new ObjectMapper();

    private final Logger logger = LoggerFactory.getLogger(this.getClass());

    public QueryFormServiceImpl(MetadataService metadataService) {

        this.metadataService = metadataService;

        LocalDateTimeDeserializer dateTimeDeserializer = new LocalDateTimeDeserializer(formatter);
        LocalDateTimeSerializer dateTimeSerializer = new LocalDateTimeSerializer(formatter);

        JavaTimeModule javaTimeModule = new JavaTimeModule();
        javaTimeModule.addDeserializer(LocalDateTime.class, dateTimeDeserializer);
        javaTimeModule.addSerializer(LocalDateTime.class, dateTimeSerializer);

        mapper.registerModule(javaTimeModule);
        mapper.disable(MapperFeature.DEFAULT_VIEW_INCLUSION);
        mapper.enable(MapperFeature.ACCEPT_CASE_INSENSITIVE_ENUMS);
  //      mapper.configure(SerializationFeature.FAIL_ON_EMPTY_BEANS, false);
    }

    @Autowired
    private ApplicationConfiguration applicationConfiguration;

    @Override
    public String createQueryForm(String data) throws QueryFormIOException, AuthenticationException {

         // Create identificator of the new form
        String id = RandomStringUtils.randomAlphanumeric(20);
        Path filePath = getFilePath(id);

        File queryFormFile = new File(filePath.toString());

        if (queryFormFile.exists()) {
            // If file with new id exists try to create other id
            id = RandomStringUtils.randomAlphanumeric(20);
            filePath = getFilePath(id);
        }

        String dataInfoFilled = setFormInfo(data);
        writeFile(queryFormFile, dataInfoFilled);

        return id;
    }

    @Override
    public QueryForm getQueryForm(String id) throws QueryFormIOException, IllegalArgumentException {

        QueryForm queryForm = null;
                String queryFormData = getQueryFormContent(id);
        try {

            queryForm = mapper.readValue(queryFormData, QueryForm.class);
        } catch (final JsonProcessingException e) {
            logger.error("UserFormService json processing exception by filter form get: " + e.getMessage(), (Object)e.getStackTrace());
            throw new IllegalArgumentException("UserFormService json processing exception by trying form get.");
        }

        return queryForm;
    }

    /*
     * Get query form with metadata as json.
     */
    @Override
    public String getQueryFormFull(String id) throws QueryFormIOException, IllegalArgumentException {

        String queryFormJson = "";

        QueryForm queryForm = getQueryForm(id);
        metadataService.setMetadata(queryForm);

        try {

            queryFormJson = mapper.writerWithView(QueryFormViews.Admin.class).writeValueAsString(queryForm);
        } catch (final JsonProcessingException e) {
            logger.error("UserFormService json processing exception by filter form get: " + e.getMessage(), (Object)e.getStackTrace());
            throw new IllegalArgumentException("UserFormService json processing exception by trying form get.");
        }

        return queryFormJson;
    }

    @Override
    public void saveQueryForm(String id, String data) throws QueryFormIOException, AuthenticationException {

        Path filePath = getFilePath(id);
        File queryFormFile = new File(filePath.toString());

        if (queryFormFile.exists()) {
            String dataInfoFilled = setFormInfo(data);
            writeFile(queryFormFile, dataInfoFilled);
        }
    }

    @Override
    public void deleteQueryForm(String id) throws QueryFormIOException {

        try {
            Files.delete(getFilePath(id));
        } catch(final IOException e) {
            logger.error("QueryBuilder exception by file removing. ", e);
            throw new QueryFormIOException("QueryBuilder exception by file removing. ");
        }
    }

    @Override
    public Map<String, String> getQueryFormList() {
        Map<String, String> queryFormList = new HashMap<>();
        File queryFormsPath = new File(applicationConfiguration.getPath().getQueryForm());

        if (queryFormsPath.exists()) {
            // Read files names from queryForm list folder
            File[] fileNames = queryFormsPath.listFiles(
                    (path, name) -> name.toLowerCase().startsWith(QUERY_FORM_FILE_PREFIX) &&
                            name.toLowerCase().endsWith(QUERY_FORM_FILE_EXTENSION));
            //JSON parser object to parse read file
            for (File file : fileNames) {
                try {
                    // Get id from file name
                    String id = file.getName()
                            .substring(QUERY_FORM_FILE_PREFIX.length() + 1, file.getName().indexOf(QUERY_FORM_FILE_EXTENSION) - 1);
                    if (StringUtils.isNotBlank(id)) {
                        queryFormList.put(id, new String(Files.readAllBytes(file.toPath()), StandardCharsets.UTF_8));
                    }
                } catch(final IOException e) {
                    logger.error("QueryBuilder exception by file reading. ", e);
                }
            }
        }

        return queryFormList;
    }

    @Override
    public List<String> getCssFileList() {
        List<String> cssFileList = new ArrayList<>();
        String cssFileFolder = applicationConfiguration.getPath().getCssFile();
        File cssFilePath = new File(cssFileFolder);

        if (cssFilePath.exists()) {
            // Read files names from queryForm list folder
            File[] fileNames = cssFilePath.listFiles(
                    (path, name) -> name.endsWith(CSS_FILE_EXTENSION));
            Arrays.stream(fileNames).forEach(f -> cssFileList.add(f.getName()));
         }

        return cssFileList;
    }

    private void writeFile(File file, String data) throws QueryFormIOException {
        try {
            RandomAccessFile stream = new RandomAccessFile(file, "rw");
            FileChannel channel = stream.getChannel();

            FileLock lock = null;
            try {
                lock = channel.tryLock();
            } catch (final OverlappingFileLockException e) {
                stream.close();
                channel.close();
                logger.error("QueryBuilder exception by file locking. ", e);
                throw new QueryFormIOException(e.getMessage());
            }
            stream.setLength(0);
            stream.write(data.getBytes(StandardCharsets.UTF_8));
            lock.release();

            stream.close();
            channel.close();
        } catch (final IOException e) {
            logger.error("QueryBuilder exception by file writing. ", e);
            throw new QueryFormIOException(e.getMessage());
        }
    }

    private Path getFilePath(String id) {
        String fileName = String.format("%s_%s.%s", QUERY_FORM_FILE_PREFIX, id, QUERY_FORM_FILE_EXTENSION);
        return Paths.get(applicationConfiguration.getPath().getQueryForm(), fileName);
    }

    private String setFormInfo(String data) throws QueryFormIOException, AuthenticationException {
        try {
            QueryForm queryForm = mapper.readValue(data, QueryForm.class);
            Info info = queryForm.getInfo();

            if (StringUtils.isBlank(queryForm.getInfo().getConnection().get(ConnectionType.SDB))) {
                String connectionSdb = (queryForm.getFilter().isEmpty()) ? "" :
                        metadataService.getConnectionSdb(queryForm.getFilter().get(0).getMetadata().getDatabase(),
                                queryForm.getFilter().get(0).getMetadata().getTable());
                info.getConnection().put(ConnectionType.SDB, connectionSdb);
            }

            Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
            // Additional check user authentication
            if ((authentication instanceof AnonymousAuthenticationToken)) {
                throw new AuthenticationException("User with anonymous authentication is trying a query form to change.");
            }
            info.setAuthor(authentication.getName());
            queryForm.setInfo(info);

            return mapper.writerWithView(QueryFormViews.Internal.class).writeValueAsString(queryForm);
        } catch (final JsonProcessingException e) {
            logger.error("QueryBuilder exception by json with form data parsing. ", e);
            throw new QueryFormIOException(e.getMessage());
        }
    }

    private String getQueryFormContent(String id) throws QueryFormIOException {
        String queryFormContent = "";
        Path filePath = getFilePath(id);
        File queryFormsPath = new File(filePath.toString());

        if (queryFormsPath.exists()) {
            try {
                queryFormContent = new String (Files.readAllBytes(filePath), StandardCharsets.UTF_8);
            }
            catch (final IOException e) {
                logger.error("QueryBuilder exception by file reading. ", e);
                throw new QueryFormIOException("QueryBuilder exception by file reading. ");
            }
        }

        return queryFormContent;
    }
}
