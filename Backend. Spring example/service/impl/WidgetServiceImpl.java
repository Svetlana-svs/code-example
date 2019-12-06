package ...widget.service.impl;


import io.github.classgraph.*;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import ...api.service.security.impl.LoggedInUserHolder;
import ...widget.AbstractWidget;
import ...widget.service.WidgetService;

import java.lang.reflect.Constructor;

/**
 * {@inheritDoc}
 *
 * Implements WidgetService interface.
 */
@Service
public class WidgetServiceImpl implements WidgetService {

    private static final Logger LOGGER = LoggerFactory.getLogger(WidgetService.class);
    private LoggedInUserHolder loggedInUserHolder;

    public static String apiRedmineAccessKey;
    public static String apiJenkinsUser;
    public static String apiJenkinsAccessKey;

    @Autowired
    public WidgetServiceImpl(LoggedInUserHolder loggedInUserHolder) {
        this.loggedInUserHolder = loggedInUserHolder;
    }

    @Value("${..redmine.admin.api-key}")
    public void setApiRedmineAccessKey(String apiKey) {
        apiRedmineAccessKey = apiKey;
    }

    @Value("${..jenkins.user}")
    public void setApiJenkinsUser(String user) {
        apiJenkinsUser = user;
    }

    @Value("${..jenkins.user.api-key}")
    public void setApiJenkinsAccessKey(String apiKey) {
        apiJenkinsAccessKey = apiKey;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public <T extends AbstractWidget> T createWidget(String definitionName,
                                                     String definitionDescription,
                                                     String templateName,
                                                     String templateDescription) {

        try (ScanResult scanResult = new ClassGraph().enableAllInfo().whitelistPackages("...widget.impl")
                .scan()) {
            ClassInfoList routeClassInfoList = scanResult.getClassesWithAnnotation("...widget.Widget");
            for (ClassInfo routeClassInfo : routeClassInfoList) {
                AnnotationInfo widgetDefinitionAnnotationInfo = routeClassInfo.getAnnotationInfo("...widget.Widget");
                AnnotationParameterValueList widgetDefinitionAnnotationParamValues =
                        widgetDefinitionAnnotationInfo.getParameterValues();

                String widgetDefinitionName = (String) widgetDefinitionAnnotationParamValues.get("name");
                if (widgetDefinitionName.equals(definitionName)) {
                    try
                    {
                        Class<T> wizardClass = (Class<T>)Class.forName(routeClassInfo.getName());
                        Constructor<T> cons = wizardClass.getConstructor(
                                String.class, String.class, String.class, String.class, LoggedInUserHolder.class);
                        T widgetObject = (T)cons.newInstance(new Object[] {definitionName, definitionDescription,
                                templateName, templateDescription, loggedInUserHolder});

                        return widgetObject;
                    }
                    catch (Exception ex)
                    {
                        LOGGER.error("Create widget instance error: {}", ex.getMessage());
                    }
                }
            }
        } catch (EnumConstantNotPresentException ex) {
            LOGGER.error("Create widget instance error: {}", ex.getMessage());
        }
        return null;
    }
}
