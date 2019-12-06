package com.package.name.integration.jpa.core.repository;


public class Condition {
    private String attribute;
    private Option option;
    private Object value;

    public Condition(String attribute, Object value, Option option) {
        this.attribute = attribute;
        this.option = option;
        this.value = value;
    }

    public Condition(String attribute, Object value) {
        this.attribute = attribute;
        this.value = value;
        this.option = Option.EQUAL;
    }

    public String getAttribute() {
        return attribute;
    }

    public void setAttribute(String attribute) {
        this.attribute = attribute;
    }

    public Option getOption() {
        return option;
    }

    public void setOption(Option option) {
        this.option = option;
    }

    public Object getValue() {
        return value;
    }

    public void setValue(Object value) {
        this.value = value;
    }
}
