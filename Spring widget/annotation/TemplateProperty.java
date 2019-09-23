package ...widget;


import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/*
 * Indicates that a field is a Widget Template configuration property,
 * declares base characteristics of this property (for example, template name for that the property will be set)
 * and assigns default field values by Widget Template creation.
 */
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.FIELD)
public @interface TemplateProperty {
    String forTemplate();
    String defaultValue() default "";
}
