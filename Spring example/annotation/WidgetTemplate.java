package ...widget;


import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/*
 * Declares Widget Template.
 * A common use case is to assign base required properties values by Widget Template creation.
 */
@Retention(RetentionPolicy.RUNTIME)
@Target({ElementType.TYPE})
public @interface WidgetTemplate {
    String name();
    String description() default "";
}
