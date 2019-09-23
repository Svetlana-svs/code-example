package ...widget;


import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/*
 * Indicates that a field is a Widget Definition configuration property and
 * declares base characteristics of this property (for example, whether this property is required) by Widget creation.
 */
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.FIELD)
public @interface WidgetProperty {
    boolean mandatory() default false;
}