package project.test.core.annotation;

import project.test.core.common.FieldInputType;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;


/*
 * Using for input identification in DOM structure.
 */
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.FIELD)
public @interface InputIdentificator {
    String type() default "";
    String value() default "";
}
