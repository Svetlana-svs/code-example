package ...widget;


import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/*
 * Indicates that a field is a Widget Template configuration property.
 * Declares Widget Template that will have to be created for current Widget Definition (@see Widget)).
 */
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.FIELD)
public @interface TemplateProperties {
    TemplateProperty[] value();
}
