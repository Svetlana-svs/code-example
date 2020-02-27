package ru.softwerke.querybuilder.core.config;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.ResourceHandlerRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

/*
 *  @author Svetlana Suvorova
 */
@Configuration
public class ApplicationWebConfigureAdapter implements WebMvcConfigurer {

    @Autowired
    private ApplicationConfiguration applicationConfiguration;

    @Override
    public void addResourceHandlers(ResourceHandlerRegistry registry) {
        registry.addResourceHandler("/ext/**").addResourceLocations("file:///" + applicationConfiguration.getPath().getCssFile() + "/");
    }
}
