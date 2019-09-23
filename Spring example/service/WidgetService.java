package ...widget.service;


import ...widget.AbstractWidget;

/*
 * Service for activities on the widgets and provide specific and security widget configuration properties.
 * Service is used as a widget fabric to load widget instances in real time using annotations.
 */
public interface WidgetService {
    /*
     * Create WidgetInstance object by defined Widget Definition and Widget Template properties.
     *
     * @param  definitionName         name of the Widget Definition for which instance is created
     * @param  definitionDescription  description of the Widget Definition for which instance is created
     * @param  templateName           name of the Widget Template that instance is created
     * @param  templateDescription    description of the Widget Template for which instance is created
     * @return WidgetInstance object
     */
    public <T extends AbstractWidget> T createWidget(String definitionName,
                                                     String definitionDescription,
                                                     String templateName,
                                                     String templateDescription);
}
