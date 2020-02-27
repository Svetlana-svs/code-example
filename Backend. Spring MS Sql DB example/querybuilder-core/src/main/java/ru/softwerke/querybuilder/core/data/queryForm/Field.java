package ru.softwerke.querybuilder.core.data.queryForm;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonView;

import java.io.Serializable;
import java.util.List;

public class Field implements Serializable {

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private Settings settings;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private Metadata metadata;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private List<String> data;

    public Settings getSettings() {
        return settings;
    }

    public void setSettings(Settings settings) {
        this.settings = settings;
    }

    public Metadata getMetadata() {
        return metadata;
    }

    public void setMetadata(Metadata metadata) {
        this.metadata = metadata;
    }

    public List<String> getData() {
        return data;
    }

    public void setData(List<String> data) {
        this.data = data;
    }

    public class Settings implements Serializable {

        @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
        private String id;

        @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
        private FieldType type;

        @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
        private String title;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public FieldType getType() {
            return type;
        }

        public void setType(FieldType type) {
            this.type = type;
        }

        public String getTitle() {
            return title;
        }

        public void setTitle(String title) {
            this.title = title;
        }
    }

    public static enum FieldType {
        @JsonProperty("select")
        SELECT,

        @JsonProperty("radio")
        RADIO,

        @JsonProperty("checkbox")
        CHECKBOX,

        @JsonProperty("multiplechoice")
        MULTIPLECHOICE,

        @JsonProperty("yesno")
        YESNO,

        @JsonProperty("input")
        TEXTINPUT,

        @JsonProperty("soundex")
        SOUNDEX,

        @JsonProperty("button")
        BUTTON,

        @JsonProperty("info")
        INFO,

        @JsonProperty("rtf")
        RTF;
    }
}
