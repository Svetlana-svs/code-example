package project.test.core.message;

import project.test.core.common.FieldInputType;
import project.test.core.annotation.InputType;

import java.util.ArrayList;
import java.util.List;

public class Message {

    public final static String MESSAGE_TEXT_PREFIX = "Test Auto";

    @InputType(type = FieldInputType.TEXT)
    private List<String> textFieldList;

    @InputType(type = FieldInputType.IFRAME)
    private String body;

    @InputType(type = FieldInputType.AUTOCOMPLETE)
    private List<String> autocompleteFieldList;

    @InputType(type = FieldInputType.DATETIME)
    private List<String> dateTimeFieldList;

    public Message() {
        this.textFieldList = new ArrayList<String>();
        this.autocompleteFieldList = new ArrayList<String>();
        this.dateTimeFieldList = new ArrayList<String>();
    }

    public void clear() {
        this.textFieldList.clear();
        this.autocompleteFieldList.clear();
        this.dateTimeFieldList.clear();
        this.body = "";
    }

    public List<String> getTextFieldList() {
        return textFieldList;
    }

    public void setTextFieldList(List<String> textFieldList) {
        this.textFieldList = textFieldList;
    }

    public void setTextFieldList(String textField) {
        this.textFieldList.add(textField);
    }

    public String getBody() {
        return body;
    }

    public void setBody(String body) {
        this.body = body;
    }

    public List<String> getAutocompleteFieldList() {
        return autocompleteFieldList;
    }

    public void setAutocompleteFieldList(List<String> autocompleteFieldList) {
        this.autocompleteFieldList = autocompleteFieldList;
    }

    public void setAutocompleteFieldList(String autocompleteField) {
        this.autocompleteFieldList.add(autocompleteField);
    }

    public List<String> getDateTimeFieldList() {
        return dateTimeFieldList;
    }

    public void setDateTimeFieldList(List<String> dateTimeFieldList) {
        this.dateTimeFieldList = dateTimeFieldList;
    }

    public void setDateTimeFieldList(String dateTimeField) {
        this.dateTimeFieldList.add(dateTimeField);
    }
}
