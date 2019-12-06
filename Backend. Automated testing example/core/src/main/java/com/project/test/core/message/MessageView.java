package project.test.core.message;

import project.test.core.common.FieldInputType;
import project.test.core.annotation.InputIdentificator;
import project.test.core.annotation.InputType;

public class MessageView {

    private final static String PROPERTY_NAME_MESSAGE_VIEW_MESSAGE_BODY_CLASS = "message.view.message.body.class";
    private final static String PROPERTY_NAME_MESSAGE_VIEW_MESSAGE_HEADING_CLASS = "message.view.message.heading.class";
    private final static String PROPERTY_NAME_MESSAGE_VIEW_MESSAGE_DATETIME_CLASS = "message.view.message.datetime.class";
    private final static String PROPERTY_NAME_MESSAGE_VIEW_MESSAGE_SOURCE_CLASS = "message.view.message.source.class";
    private final static String PROPERTY_NAME_MESSAGE_VIEW_MESSAGE_URL_CLASS = "message.view.message.url.class";

    @InputType(type = FieldInputType.TEXT)
    @InputIdentificator(type="class", value=PROPERTY_NAME_MESSAGE_VIEW_MESSAGE_HEADING_CLASS)
    private String heading;

    @InputType(type = FieldInputType.IFRAME)
    @InputIdentificator(type="class", value=PROPERTY_NAME_MESSAGE_VIEW_MESSAGE_BODY_CLASS)
     private String body;

    @InputType(type = FieldInputType.DATETIME)
    @InputIdentificator(type="class", value=PROPERTY_NAME_MESSAGE_VIEW_MESSAGE_DATETIME_CLASS)
    private String dateTime;

    @InputType(type = FieldInputType.AUTOCOMPLETE)
    @InputIdentificator(type="class", value=PROPERTY_NAME_MESSAGE_VIEW_MESSAGE_SOURCE_CLASS)
    private String source;

    @InputType(type = FieldInputType.TEXT)
    @InputIdentificator(type="class", value=PROPERTY_NAME_MESSAGE_VIEW_MESSAGE_URL_CLASS)
    private String url;

    public MessageView() {
    }
 }
