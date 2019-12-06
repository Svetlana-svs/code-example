package com.package.name.integration.jpa.user.repository;

import com.package.name.integration.jpa.core.exception.ApplicationPersistenceException;
import com.package.name.integration.jpa.core.exception.NoSuchEntityException;
import com.package.name.integration.jpa.core.exception.TooManyEntitiesException;
import com.package.name.integration.jpa.core.repository.AbstractResult;
import com.package.name.integration.jpa.core.repository.Condition;

import java.util.List;
import java.util.Map;

/**
 * Implements all the  CRUD operations to manage user entity.
 */
public interface UserRepository {

    long create(UserEntity entity)  throws ApplicationPersistenceException, NoSuchEntityException;

    void update(Map<String, Object> fields, List<Condition> conditions) throws ApplicationPersistenceException ;

    <T1 extends AbstractResult> T1 fetch(Class<T1> result, List<Condition> conditions)
            throws ApplicationPersistenceException, NoSuchEntityException, TooManyEntitiesException;

    <T1 extends AbstractResult> List<T1> fetchAll(Class<T1> result, List<Condition> conditions)
            throws ApplicationPersistenceException, NoSuchEntityException;
}
