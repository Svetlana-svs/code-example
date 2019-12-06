package com.softwerke.com.integration.jpa.user.repository.impl;

import com.softwerke.com.integration.jpa.core.exception.ApplicationPersistenceException;
import com.softwerke.com.integration.jpa.core.exception.NoSuchEntityException;
import com.softwerke.com.integration.jpa.core.exception.TooManyEntitiesException;
import com.softwerke.com.integration.jpa.core.repository.AbstractRepository;
import com.softwerke.com.integration.jpa.core.repository.AbstractResult;
import com.softwerke.com.integration.jpa.core.repository.Condition;
import com.softwerke.com.integration.jpa.user.repository.UserRepository;
import org.osgi.service.component.annotations.Component;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import javax.persistence.Tuple;
import javax.persistence.TupleElement;
import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Modifier;
import java.util.*;
import java.util.stream.Collectors;
import java.util.stream.Stream;


@Component(
        service = UserRepository.class,
        immediate = true)
public class UserRepositoryImpl extends AbstractRepository<UserEntity> implements UserRepository {

    private static final Logger logger = LoggerFactory.getLogger(UserRepositoryImpl.class);

    @Override
    public long create(UserEntity entity) throws ApplicationPersistenceException, NoSuchEntityException {
        super.save(entity);

        long id = Optional.ofNullable(entity.getId()).orElse(0L);
        if (id == 0) {
            throw new NoSuchEntityException("No entity of the class UserEntity was be created");
        }
        return id;
    }

    @Override
    public void update(Map<String, Object> fields, List<Condition> conditions) throws ApplicationPersistenceException {
        super.update(UserEntity.class, fields, conditions);
    }

    @Override
    public <T1 extends AbstractResult> T1 fetch(Class<T1> resultClass, List<Condition> conditions)
            throws ApplicationPersistenceException, NoSuchEntityException, TooManyEntitiesException {

        List<T1> results = fetchAll(resultClass, conditions);

        if (results.size() != 1) {
            throw new TooManyEntitiesException("Too many entities of the class " + UserEntity.class +
                    " with such conditions: " + conditions.toString());
        }
        return results.get(0);
    }

    @Override
    public <T1 extends AbstractResult> List<T1> fetchAll(Class<T1> resultClass, List<Condition> conditions)
            throws ApplicationPersistenceException, NoSuchEntityException {
        // Collect selection fields names from both result and parent result classes
        List<String> selectionFieldList = Stream.concat(Arrays.stream(resultClass.getDeclaredFields()),
                Arrays.stream(resultClass.getSuperclass().getDeclaredFields()))
                .filter(f -> Modifier.isProtected(f.getModifiers()))
                .map(Field::getName)
                .collect(Collectors.toList());
        List<Tuple> tuples = super.fetch(UserEntity.class, selectionFieldList, conditions);

        List<T1> results = getResultList(resultClass, tuples);
        // Condition for UserEntity only
        if ((results == null) || results.isEmpty()) {
            throw new NoSuchEntityException("No entities of the class " + UserEntity.class + " " +
                    "with such conditions: " + conditions.toString());
        }

        return results;
    }

    private <T1 extends AbstractResult> List<T1> getResultList(Class<T1> resultClass, List<Tuple> tuples) {
        List<T1> resultList = new ArrayList<>();
        for (Tuple tuple : tuples) {
            Map<String, Object> results = new HashMap<>();
            for (TupleElement tupleElement : tuple.getElements()) {
                String alias = tupleElement.getAlias();
                if (alias != null) {
                    Object value = tuple.get(alias);
                    results.put(alias, (value == null ? null : value.toString()));
                }
            }
            try {
                Object result = Class.forName(resultClass.getName()).getConstructor(Map.class).newInstance(results);
                resultList.add((T1) result);
            } catch (ClassNotFoundException | NoSuchMethodException | InstantiationException | IllegalAccessException | InvocationTargetException e) {
                logger.error("Error by mapping result list", e);
            }
        }

        return resultList;
    }
}