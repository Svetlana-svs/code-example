package project.test.core.common;

public enum FieldInputType {
    TEXT("text"),
    AUTOCOMPLETE("autocomplete"),
    DATETIME("datetime"),
    IFRAME("iframe");

    private final String value;

    FieldInputType(String value) {
        this.value = value;
    }

    public final String getValue() {
        return value;
    }

    @Override
    public String toString() {
        return value;
    }
}
