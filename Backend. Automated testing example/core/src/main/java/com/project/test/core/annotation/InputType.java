package project.test.core.annotation;

import project.test.core.common.FieldInputType;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;


/*
 * Indicates input type of a field (for example text, dateTime, autocomplete).
 */
@Retention(RetentionPolicy.RUNTIME)
@Target({ElementType.FIELD, ElementType.METHOD})
public @interface InputType {
    FieldInputType type() default FieldInputType.TEXT;
}
