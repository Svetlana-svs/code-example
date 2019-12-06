package com.package.name.integration.jpa.core.exception;

/**
 * This exception should be thrown if any persistence operation trying to manage the entity that does not exist
 */
public class NoSuchEntityException extends Exception {

    public NoSuchEntityException() {
        super();
    }

    public NoSuchEntityException(String message) {
        super(message);
    }

    public NoSuchEntityException(String message, Throwable cause) {
        super(message, cause);
    }

    public NoSuchEntityException(Throwable cause) {
        super(cause);
    }
}
