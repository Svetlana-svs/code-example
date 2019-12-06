package ...widget.impl.redmine;


import com.taskadapter.redmineapi.Params;
import com.taskadapter.redmineapi.RedmineException;
import com.taskadapter.redmineapi.RedmineManager;
import com.taskadapter.redmineapi.RedmineManagerFactory;
import com.taskadapter.redmineapi.bean.Issue;
import com.taskadapter.redmineapi.bean.Project;
import org.apache.http.client.HttpClient;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.*;
import org.springframework.http.client.HttpComponentsClientHttpRequestFactory;
import ...api.service.security.impl.LoggedInUserHolder;
import ...dto.widget.WidgetInstanceDTO;
import ...dto.widget.WidgetTemplateDTO;
import ...widget.*;
import ...widget.exception.BadRequestException;
import ...widget.exception.NotFoundException;
import ...widget.impl.redmine.dto.ProjectDTO;
import ...widget.impl.redmine.util.ListIssueConverter;
import ...widget.impl.redmine.util.ListProjectConverter;
import ...widget.service.impl.WidgetServiceImpl;

import javax.security.auth.login.Configuration;
import java.io.IOException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/*
 * Widget class to provide information from the project management and issue tracking tool Redmine.
 */
@Widget(
        name = "redmine",
        description = "Widget to provide information from project management and issue tracking tool Redmine."
)
@WidgetTemplates({
        @WidgetTemplate(name = "default redmine", description = "Default redmine widget.")
})
public class RedmineWidget extends AbstractWidget {
    private static final Logger LOGGER = LoggerFactory.getLogger(RedmineWidget.class);

    @WidgetProperty(mandatory = true)
    @TemplateProperties({
            @TemplateProperty(forTemplate = "default redmine", defaultValue = "https://redmine.local")
    })
     String uri;

    @WidgetProperty(mandatory = true)
    @TemplateProperties({
            @TemplateProperty(forTemplate = "default redmine")
    })
    List<ProjectDTO> projects;

    public RedmineWidget(String definitionName, String definitionDescription, String templateName, String templateDescription,
                         LoggedInUserHolder loggedInUserHolder) {
        super(definitionName, definitionDescription, templateName, templateDescription, loggedInUserHolder);
    }

    /*
     * Method handles Http request by get an widget template information.
     *
     * @param  requestParameters WidgetTemplate object for which information is required
     * @return response entity, consisting object with required information as body
     */
    @Override
    public ResponseEntity<Object> request(WidgetTemplateDTO requestParameters) {

        if (loggedInUserHolder == null) {
            return new ResponseEntity<>(String.format("Error request: Service %s is not available.", LoggedInUserHolder.class.getName()),
                    HttpStatus.BAD_REQUEST);
        }

        Map<String, Object> configuration = requestParameters.getConfiguration();
        RedmineManager manager = getRedmineManager(configuration);
        if (manager == null) {
            LOGGER.error("Error request: Redmine manager is not available.");
            return new ResponseEntity<>("Error request: Redmine manager is not available.", HttpStatus.BAD_REQUEST);
        }

        try {
            String projectsDTO = getProjects(manager);
            configuration.replace("projects", projectsDTO);
        } catch (RedmineException ex) {
            LOGGER.error("Error request: Redmine manager error is raised. {}", ex.getMessage());
            return new ResponseEntity<>(ex.getMessage(), HttpStatus.BAD_REQUEST);
        } catch (IOException ex) {
            LOGGER.error("Error request: json processing error. {}", ex.getMessage());
            return new ResponseEntity<>(ex.getMessage(), HttpStatus.BAD_REQUEST);
        }

        requestParameters.setConfiguration(configuration);

        return new ResponseEntity<>(requestParameters, HttpStatus.OK);
    }

    /*
     * Method handles Http request.
     * Method gets project data from external Redmine service by configuration property of the request parameter.
     * If request is required data for an action on the widget instance (for edit, for example),
     * projects list as additional data is set to response widget instance.
     *
     * @param  requestParameters WidgetInstance object for which information is required
     * @param  action            for which action on the WidgetInstance object information is required
     * @return response entity, consisting Redmine widget instance as body
     */
    @Override
    public ResponseEntity<Object> request(WidgetInstanceDTO requestParameters, String action) {

        if (loggedInUserHolder == null) {
            return new ResponseEntity<>(String.format("Error request: Service %s is not available.", LoggedInUserHolder.class.getName()),
                    HttpStatus.BAD_REQUEST);
        }

        Map<String, Object> configuration = requestParameters.getConfiguration();
        RedmineManager manager = getRedmineManager(configuration);
        if (manager == null) {
            LOGGER.error("Error request: Redmine manager is not available.");
            return new ResponseEntity<>("Error request: Redmine manager is not available.", HttpStatus.BAD_REQUEST);
        }

        try {
            String projectDTO = getProjectData(manager, configuration);
            configuration.replace("projects", projectDTO);
            requestParameters.setConfiguration(configuration);

            if (action != null) {
                Map<String, Object> widgetTemplateConfiguration = requestParameters.getWidgetTemplate().getConfiguration();
                String projectsDTO = getProjects(manager);
                widgetTemplateConfiguration.replace("projects", projectsDTO);
                requestParameters.getWidgetTemplate().setConfiguration(widgetTemplateConfiguration);
            }

            return new ResponseEntity<>(requestParameters, HttpStatus.OK);
        } catch (RedmineException ex) {
            LOGGER.error("Error request: Redmine manager error is raised. {}", ex.getMessage());
            return new ResponseEntity<>(ex.getMessage(), HttpStatus.BAD_REQUEST);
        } catch (BadRequestException ex) {
            LOGGER.error(ex.getMessage());
            return new ResponseEntity<>(ex.getMessage(), HttpStatus.BAD_REQUEST);
        } catch (IOException ex) {
            LOGGER.error("Error request: json processing error. {}", ex.getMessage());
            return new ResponseEntity<>(ex.getMessage(), HttpStatus.BAD_REQUEST);
        }
    }

    private RedmineManager getRedmineManager(Map<String, Object> configuration) {

        if (configuration == null || !configuration.containsKey("uri")) {
        throw new BadRequestException("Error request: configuration parameter uri is missing.");
        }
        // Get redmine manager with admin permissions
        RedmineManager manager = RedmineManagerFactory.createWithApiKey((String)configuration.get("uri"),
                WidgetServiceImpl.apiRedmineAccessKey, getHttpClient());

        if (manager == null) {
            return null;
        }
        ...entity.usergroup.User currentUser = loggedInUserHolder.getCurrentUser();
        if (currentUser == null) {
            LOGGER.error("Error request: user data is not available.");
            return null;
        }
        // Set redmine manager for current user
        manager.setOnBehalfOfUser(currentUser.getLogin());

        return manager;
    }

    private String getProjects(RedmineManager manager)
            throws RedmineException, IOException {
        String projectsDTO = "";

        List<Project> redmineProjects = manager.getProjectManager().getProjects();
        projectsDTO = objectMapper.writeValueAsString(
                ListProjectConverter.convert(redmineProjects));

        return projectsDTO;
    }

    private String getProjectData(RedmineManager manager, Map<String, Object> configuration)
            throws RedmineException, BadRequestException, NotFoundException, IOException {

        if (configuration == null || !configuration.containsKey("uri")) {
            throw new BadRequestException("Error request: configuration parameter uri is missing.");
        }
        if (!configuration.containsKey("projects") || configuration.get("projects").toString().isEmpty()) {
            throw new BadRequestException("Error request: projects configuration is empty.");
        }

        String projectsDTO = "";
        ProjectDTO projectDTO = null;

        List<HashMap<String, String>> projects = (List<HashMap<String, String>>)configuration.get("projects");
        HashMap<String, String> project = projects.get(0);
        if (!project.containsKey("id") || (project.get("id") == null) || project.get("id").isEmpty()) {
            throw new BadRequestException("Error request: parameter project id is missing.");
        }

        Project redmineProject = manager.getProjectManager().getProjectById(Integer.parseInt(project.get("id")));
        if (redmineProject == null) {
            throw new NotFoundException(String.format("Error request: Redmine project with id %s is missing.", project.get("id")));
        }
        projectDTO = new ProjectDTO(redmineProject.getId(), redmineProject.getName(), redmineProject.getIdentifier());

        Params params = new Params()
                .add("project_id", Integer.toString(projectDTO.getId()))
                .add("status_id", Configuration.getIssueStatusId())
                .add("assigned_to_id", Configuration.getUserId());
        List<Issue> redmineIssues = manager.getIssueManager().getIssues(params).getResults();
        projectDTO.setIssues(ListIssueConverter.convert(redmineIssues));

        projectsDTO = AbstractWidget.objectMapper.writeValueAsString(projectDTO);

        return projectsDTO;
    }

    private HttpClient getHttpClient() {
        int timeout = 5000;
        HttpComponentsClientHttpRequestFactory clientHttpRequestFactory
                = new HttpComponentsClientHttpRequestFactory();
        clientHttpRequestFactory.setConnectTimeout(timeout);
        return clientHttpRequestFactory.getHttpClient();
    }
}
