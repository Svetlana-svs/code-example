package project.test.core.message;

public enum MessageMode {
    CREATE("create"),
    EDIT("edit");

    private final String value;

    MessageMode(String value) {
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
