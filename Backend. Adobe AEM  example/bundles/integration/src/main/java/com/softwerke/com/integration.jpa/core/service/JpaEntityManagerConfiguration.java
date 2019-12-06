package com.package.name.integration.jpa.core.service;

import org.osgi.service.metatype.annotations.AttributeDefinition;
import org.osgi.service.metatype.annotations.AttributeType;
import org.osgi.service.metatype.annotations.ObjectClassDefinition;


@ObjectClassDefinition(
        name = "UserProfile Jpa Entity Manager Configuration",
        description = "Persistence properties configuration to get an entity manager from the entity manager factory."
)
public @interface JpaEntityManagerConfiguration {

    @AttributeDefinition(
            name = "Hibernate dialect",
            description = "Set the appropriate SQL statements for the  chosen database.",
            type = AttributeType.STRING)
    String hibernate_dialect() default "";

    @AttributeDefinition(
            name = "Hibernate show sql",
            description = "Enable the logging of all the generated SQL statements.",
            type = AttributeType.BOOLEAN)
    boolean hibernate_show__sql() default false;
    @AttributeDefinition(
            name = "Hibernate format",
            description = "Set format the generated SQL statement to make it more readable.",
            type = AttributeType.BOOLEAN)
    boolean hibernate_format__sql() default false;

    @AttributeDefinition(
            name = "Hibernate connection driver",
            description = "Set the JDBC driver class.",
            type = AttributeType.STRING)
    String hibernate_connection_driver__class() default "";

    @AttributeDefinition(
            name = "Hibernate connection URL",
            description = "Set the JDBC URL to the database instance.",
            type = AttributeType.STRING)
    String hibernate_connection_url() default "";

    @AttributeDefinition(
            name = "Hibernate connection username",
            description = "Set the username to the database access.",
            type = AttributeType.STRING)
    String hibernate_connection_username() default "";

    @AttributeDefinition(
            name = "User verification page",
            description = "Set the password to the database access.",
            type = AttributeType.STRING)
    String hibernate_connection_password() default "";
}

