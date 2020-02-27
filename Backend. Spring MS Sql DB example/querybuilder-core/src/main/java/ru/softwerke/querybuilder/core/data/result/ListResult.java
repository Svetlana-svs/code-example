package ru.softwerke.querybuilder.core.data.result;

public class ListResult {
    private String value;
    private String alias;

    public ListResult(String alias, String value) {
        this.alias = alias;
        this.value = value;
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }

    public String getAlias() {
        return alias;
    }

    public void setAlias(String alias) {
        this.alias = alias;
    }
}
