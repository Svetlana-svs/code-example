package com.softwerke.com.integration.jpa.core.repository;

import com.softwerke.com.integration.jpa.core.exception.ApplicationPersistenceException;
import com.softwerke.com.integration.jpa.core.service.JpaEntityManagerFactory;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import javax.persistence.*;
import javax.persistence.criteria.*;
import javax.transaction.Transactional;
import java.io.Serializable;
import java.lang.reflect.Array;
import java.util.*;

/**
 * This abstract class contains the basic CRUD operations to manage any entity.
 * @param <T> entity to be managed by this repository
 */
@Transactional(value=Transactional.TxType.REQUIRED)
public abstract class AbstractRepository<T extends Serializable> {

    private static final Logger logger = LoggerFactory.getLogger(AbstractRepository.class);

    @PersistenceContext
    protected EntityManager entityManager;

    /**
     * Inserts row into the table specified in {@link Table} annotation.
     * @param entity entity that represents the row
     */
    public void save(T entity) throws ApplicationPersistenceException {
        EntityTransaction transaction = null;
        try {
            entityManager =  JpaEntityManagerFactory.getEntityManager();
            transaction = entityManager.getTransaction();
            transaction.begin();

            entityManager.persist(entity);

            transaction.commit();
        } catch (RuntimeException e) {
            logger.error("Unable to save result set. ", e);
            if (entityManager != null  && transaction.isActive()) {
                transaction.rollback();
            }
            throw new ApplicationPersistenceException("Unable to save entity.", e);
        } finally {
            if (entityManager != null) {
                entityManager.close();
            }
        }
    }

    /**
     * Updates row in the table specified in {@link Table} annotation.
     * @param entity entity that represents the row
     */
    public void update(T entity) throws ApplicationPersistenceException {
        EntityTransaction transaction = null;
        try {
            entityManager =  JpaEntityManagerFactory.getEntityManager();
            transaction = entityManager.getTransaction();
            transaction.begin();

            entityManager.merge(entity);

            transaction.commit();
        } catch (RuntimeException e) {
            logger.error("Unable to update result set. ", e);
            if (entityManager != null  && transaction.isActive()) {
                transaction.rollback();
            }
            throw new ApplicationPersistenceException("Unable to update entity " + entity.getClass().getName(), e);
        } finally {
            if (entityManager != null) {
                entityManager.close();
            }
        }
    }

    /**
     * Update specific fields of the entity using update criteria.
     * @param entityClass updating entity class
     * @param fields list of the specific fields for update
     * @param conditions list of the where clause parameters
     */
    protected void update(final Class<T> entityClass, Map<String, Object> fields, List<Condition> conditions)
            throws ApplicationPersistenceException {
        EntityTransaction transaction = null;
        try {
            entityManager =  JpaEntityManagerFactory.getEntityManager();
            transaction = entityManager.getTransaction();
            transaction.begin();
            CriteriaBuilder builder = entityManager.getCriteriaBuilder();
            CriteriaUpdate<T> criteria = builder.createCriteriaUpdate(entityClass);
            Root<T> criteriaRoot = criteria.from(entityClass);


            for (Map.Entry<String, Object> field: fields.entrySet()) {
                // TODO: use JPA Metamodel
                criteria.set(field.getKey(), field.getValue());
            }
            criteria.where(getWhereClause(conditions, builder, criteriaRoot));

            entityManager.createQuery(criteria).executeUpdate();
            transaction.commit();
        } catch (RuntimeException e) {
            logger.error("Unable to update result set. ", e);
            if (entityManager != null  && transaction.isActive()) {
                transaction.rollback();
            }
            throw new ApplicationPersistenceException("Unable to update entity " + entityClass, e);
        } finally {
            if (entityManager != null) {
                entityManager.close();
            }
        }
    }

    /**
     * Fetches the entity using named query with conditions from the table specified in {@link Table} annotation.
     * @param queryName identifier of the named query
     * @param conditions list of the query parameters
     * @return if rows with such conditions exists, then list of the entity that represents these rows. Otherwise, null
     */
    protected List<T> fetch(String queryName, Map<String, Object> conditions, String sqlResultSetMappingName)
            throws ApplicationPersistenceException {
        EntityTransaction transaction = null;
        List<T> results = null;
        try {
            entityManager =  JpaEntityManagerFactory.getEntityManager();
            transaction = entityManager.getTransaction();
            transaction.begin();
            Query query = entityManager.createNativeQuery(queryName, sqlResultSetMappingName);

            if ((conditions != null) && !conditions.isEmpty()) {
                for (Map.Entry<String, Object> parameter: conditions.entrySet()) {
                    query.setParameter(parameter.getKey(), parameter.getValue());
                }
            }
            results = (List<T>)query.getResultList();
            transaction.commit();
        } catch (RuntimeException e) {
            logger.error("Unable to fetch result set. ", e);
            if (entityManager != null  && transaction.isActive()) {
                transaction.rollback();
            }
            throw new ApplicationPersistenceException("Unable to fetch the result set for " + sqlResultSetMappingName, e);
        } finally {
            if (entityManager != null) {
                entityManager.close();
            }
        }

        return results;
    }

    /**
     * Fetches the entity using criteria with conditions from the table specified in {@link Table} annotation.
     * @param entityClass entity class
     * @param fields list of the query selection fields
     * @param conditions list of the where clause parameters
     * @return if rows with such conditions exists, then list of the entity that represents these rows. Otherwise, null
     */
    protected List<Tuple> fetch(final Class<T> entityClass, List<String> fields, List<Condition> conditions)
            throws  ApplicationPersistenceException {
        EntityTransaction transaction = null;
        List<Tuple> results = null;
        try {
            entityManager =  JpaEntityManagerFactory.getEntityManager();
            transaction = entityManager.getTransaction();
            transaction.begin();

            CriteriaBuilder builder = entityManager.getCriteriaBuilder();
            CriteriaQuery<Tuple> criteria = builder.createTupleQuery();
            Root<T> criteriaRoot = criteria.from(entityClass);

            criteria.multiselect(getSelections(fields, criteriaRoot));
            criteria.where(getWhereClause(conditions, builder, criteriaRoot));

            results = entityManager.createQuery(criteria).getResultList();
            transaction.commit();
        } catch (RuntimeException e) {
            logger.error("Unable to fetch result set. ", e);
            if (entityManager != null  && transaction.isActive()) {
                transaction.rollback();
            }
            throw new ApplicationPersistenceException("Unable to fetch the result set of entity " + entityClass, e);
        } finally {
            if (entityManager != null) {
                entityManager.close();
            }
        }

        return results;
    }

    private List<Selection<?>> getSelections(List<String> selectionFieldList, Root<T> criteriaRoot) {
        List<Selection<?>> selections = new ArrayList<>(selectionFieldList.size());
        for (String selectionField: selectionFieldList) {
            // TODO: use JPA Metamodel
            Path<?> path = criteriaRoot.get(selectionField);
            selections.add(path.alias(selectionField));
        }
        return selections;
    }

    private Predicate[] getWhereClause(List<Condition> conditions, CriteriaBuilder builder, Root<T> criteriaRoot) {
        // TODO: use JPA Metamodel
        List<Predicate> predicates = new ArrayList<Predicate>(conditions.size());
        for (Condition condition: conditions) {
            Path path = criteriaRoot.get(condition.getAttribute());
            switch (condition.getOption()) {
                case EQUAL:
                    predicates.add(builder.equal(path, condition.getValue()));
                    break;
                case ISNULL:
                    predicates.add(builder.isNull(path));
                    break;
                case LIKE:
                    predicates.add(builder.like(path, "%" + condition.getValue() + "%"));
                    break;
                case LESS:
                    predicates.add(builder.lessThan(path, (Comparable)condition.getValue()));
                    break;
                case GREATER:
                    predicates.add(builder.greaterThan(path, (Comparable)condition.getValue()));
                    break;
                case GREATER_OR_EQUAL:
                    predicates.add(builder.greaterThanOrEqualTo(path, (Comparable)condition.getValue()));
                    break;
            }

        }
        Predicate[] predicatesArray = (Predicate[]) Array.newInstance(Predicate.class, predicates.size());

        return predicates.toArray(predicatesArray);
    }
}
