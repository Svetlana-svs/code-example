package ...widget;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import ...api.service.security.impl.LoggedInUserHolder;
import ...dto.widget.WidgetInstanceDTO;
import ...dto.widget.WidgetTemplateDTO;

/*
 * Base widget class.
 */
public abstract class AbstractWidget {
    public static final ObjectMapper objectMapper = new ObjectMapper();
    private String definitionName;
    private String definitionDescription;
    private String templateName;
    private String templateDescription;

    protected LoggedInUserHolder loggedInUserHolder;

    /*
     * Create a new widget by defined Widget Definition and Widget Template properties.
     *
     * @param  definitionName         name of the Widget Definition for which widget is created
     * @param  definitionDescription  description of the Widget Definition for which widget is created
     * @param  templateName           name of the Widget Template that widget is created
     * @param  templateDescription    description of the Widget Template for which widget is created
     * @param  loggedInUserHolder     service instance to provide security information
     */
    public AbstractWidget(String definitionName, String definitionDescription, String templateName, String templateDescription,
                          LoggedInUserHolder loggedInUserHolder) {
        this.definitionName = definitionName;
        this.definitionDescription = definitionDescription;
        this.templateName = templateName;
        this.templateDescription = templateDescription;
        this.loggedInUserHolder = loggedInUserHolder;
    }

    /*
     * Method handles Http request by get widget template information.
     *
     * @param  requestParameters WidgetTemplate object for which information is required
     * @return response entity, consisting object with required information as body
     */
    public ResponseEntity<Object> request(WidgetTemplateDTO requestParameters) {
        return new ResponseEntity<>(requestParameters, HttpStatus.OK);
    }

    /*
     * Method handles Http request by get widget instance information.
     *
     * @param  requestParameters WidgetInstance object for which information is required
     * @param  action            for which action on the WidgetInstance object information is required
     * @return response entity, consisting object with required information as body
     */
    public ResponseEntity<Object> request(WidgetInstanceDTO requestParameters, String action) {
        return new ResponseEntity<>(requestParameters, HttpStatus.OK);
    }

    /*
     * Method is processed Http request by get an object information.
     *
     * @param  requestParameters as an object
     * @return response entity, consisting object with required information as body
     */
    public ResponseEntity<Object> request(Object requestParameters) {
        return new ResponseEntity<>(requestParameters, HttpStatus.OK);
    }

    public String getDefinitionName() {
        return this.definitionName;
    }

    public String getDefinitionDescription() {
        return this.definitionDescription;
    }

    public String getTemplateName() {
        return templateName;
    }

    public String getTemplateDescription() {
        return templateDescription;
    }
}
