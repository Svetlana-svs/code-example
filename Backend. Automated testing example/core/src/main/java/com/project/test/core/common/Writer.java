package project.test.core.common;

import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVPrinter;
import org.apache.commons.io.FileUtils;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.TakesScreenshot;
import org.openqa.selenium.WebDriver;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.File;
import java.io.IOException;
import java.io.PrintWriter;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

public class Writer {
    private static final String FILE_NAME_DATE_FORMAT = "yyyy-MM-dd_HH-mm-ss";
    private static final String TEST_RESULT_FOLDER = "reports";
    private static final String TEST_RESULT_FILE_PREFIX = "report";
    private static final String LOG_FOLDER = "log";

    private static final Logger logger = LoggerFactory.getLogger(Writer.class);

    public Writer() {
    }

    public static void createFolder(String folderName) {
        Path folderPath = Paths.get(folderName);
        try {
            if (!Files.exists(folderPath)) {
                Files.createDirectory(folderPath);
            }
        } catch (IOException e) {
            logger.error("IOException while create reports folder.", e);
        }
    }

    public static void writeCSV(List<TestResult> results) {
        if (results == null || results.isEmpty()) {
            return;
        }
        try {
            String fileName = getFileName(FileExtensionType.CSV, null);
            PrintWriter writer = new PrintWriter(new File(fileName), "UTF-8");
            CSVPrinter csvPrinter = CSVFormat.EXCEL
                    .withDelimiter(';')
                    .withHeader(TestResult.getHeader())
                    .print(writer);

            for (TestResult result : results) {
                try {
                    csvPrinter.printRecord(result.getRecord());
                } catch (IOException e) {
                    logger.error("IOException while writing test report.", e);
                 }
            }
            csvPrinter.flush();
            csvPrinter.close();
        }
        catch (Exception e)
        {
            logger.error("IOException while writing test report.", e);
         }
     }

    public static void writeScreenshot(String fileNamePrefix, WebDriver driver) {
        File screenshot = ((TakesScreenshot)driver).getScreenshotAs(OutputType.FILE);
        try {
            String fileName = getFileName(FileExtensionType.PNG, fileNamePrefix);
            FileUtils.copyFile(screenshot, new File(fileName));
        }
        catch (IOException e) {
            logger.error("IOException while writing screenshot.", e);
        }
    }

    private static String getFileName(FileExtensionType fileExtension, String filePrefix) {
        String fileNamePrefix = "";
        SimpleDateFormat fileNameDateFormat = new SimpleDateFormat(FILE_NAME_DATE_FORMAT);

        switch (fileExtension) {
            case CSV:
                fileNamePrefix = TEST_RESULT_FOLDER + "//" + TEST_RESULT_FILE_PREFIX;
                break;
            case PNG:
                fileNamePrefix = LOG_FOLDER + "//" + filePrefix;
                break;
        }

        return String.format("%s_%s.%s", fileNamePrefix, fileNameDateFormat.format(new Date()), fileExtension.getValue());
    }

    private enum FileExtensionType {
        CSV("csv"),
        PNG("png");

        private final String value;

        FileExtensionType(String value){
            this.value = value;
        }

        public final String getValue(){
            return value;
        }

        @Override
        public String toString() {
            return value;
        }
    }
}

