package ...widget;


import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/*
 * Declares Widget Templates that will have to be created for current Widget Definition (@see Widget)).
 */
@Retention(RetentionPolicy.RUNTIME)
@Target({ElementType.TYPE})
public @interface WidgetTemplates {
    WidgetTemplate[] value();
}
