package com.softwerke.com.integration.jpa.core.exception;

/**
 * This exception should be thrown on internal problems with database connection or query execution failure
 */
public class ApplicationPersistenceException extends Exception {

    public ApplicationPersistenceException() {
        super();
    }

    public ApplicationPersistenceException(String message) {
        super(message);
    }

    public ApplicationPersistenceException(String message, Throwable cause) {
        super(message, cause);
    }

    public ApplicationPersistenceException(Throwable cause) {
        super(cause);
    }
}
