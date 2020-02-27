package ru.softwerke.querybuilder.core.config;

/*
 *  @author Svetlana Suvorova
 */
@FunctionalInterface
public interface ThrowingConsumer<T, E extends Exception> {
    void accept(T t) throws E;
}
