package project.test.core.common;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class Configuration {
    private static Properties properties = new Properties();

    private static final Logger logger = LoggerFactory.getLogger(Configuration.class);

    public void setConfiguration() {
        try {
            // load a properties file
            InputStream inputStream = new FileInputStream("application.properties");
            properties.load(inputStream);
        } catch (IOException e) {
            logger.error("IOException while read application properties.", e);
        }
    }

    public static String getProperty(String key) {
        return properties.getProperty(key);
    }
}
