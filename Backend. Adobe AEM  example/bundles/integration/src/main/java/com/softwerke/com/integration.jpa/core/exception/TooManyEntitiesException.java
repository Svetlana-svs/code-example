package com.package.name.integration.jpa.core.exception;


/**
 * This exception should be thrown if any persistence operation trying to manage an entity but get more than one entity
 */
public class TooManyEntitiesException extends Exception {

    public TooManyEntitiesException() {
        super();
    }

    public TooManyEntitiesException(String message) {
        super(message);
    }

    public TooManyEntitiesException(String message, Throwable cause) {
        super(message, cause);
    }

    public TooManyEntitiesException(Throwable cause) {
        super(cause);
    }
}
