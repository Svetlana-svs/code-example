package ...widget;


import java.lang.annotation.*;

/*
 * Declares Widget Definition.
 * A common use case is to assign base required properties values by Widget Definition creating.
*/
@Documented
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.TYPE)
public @interface Widget {
    String name();
    String description() default "";
}
