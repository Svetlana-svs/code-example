package ru.softwerke.querybuilder.core.config;

import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.context.annotation.Configuration;
import org.springframework.validation.annotation.Validated;

/*
 *  @author Svetlana Suvorova
 */
@Configuration
@Validated
@ConfigurationProperties(prefix = "app.common")
public class ApplicationConfiguration {

    private Path path = new Path();

    public Path getPath() {
        return path;
    }

    public void setPath(Path path) {
        this.path = path;
    }

    public class Path {
        private  String queryForm;
        private String cssFile;

        public String getQueryForm() {
            return queryForm;
        }

        public void setQueryForm(String queryForm) {
            this.queryForm = queryForm;
        }

        public String getCssFile() {
            return cssFile;
        }

        public void setCssFile(String cssFile) {
            this.cssFile = cssFile;
        }
    }
}
