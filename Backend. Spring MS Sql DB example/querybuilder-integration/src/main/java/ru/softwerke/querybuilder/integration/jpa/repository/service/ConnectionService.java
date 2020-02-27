package ru.softwerke.querybuilder.integration.jpa.repository.service;

import org.hibernate.Session;
import org.hibernate.exception.DataException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.transaction.interceptor.TransactionAspectSupport;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import java.io.Serializable;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

/*
 *  @author Svetlana Suvorova
 */
@Service
public class ConnectionService<T extends Serializable> {

    @Autowired
    @PersistenceContext
    private EntityManager entityManager;

    private final Logger logger = LoggerFactory.getLogger(this.getClass());

    @Transactional
    public List<Object[]> getResultList(String query) {

        List<Object[]> results = new ArrayList<>();
        try {
            Session session = entityManager.unwrap(Session.class);
            try {

                results = session.createSQLQuery(query).getResultList();
            } catch (RuntimeException e) {
                logger.debug(e.getMessage());
                throw new DataException("UserService generates exception: ", new SQLException(query));
            }
        } catch (Throwable e) {
            TransactionAspectSupport.currentTransactionStatus()
                    .setRollbackOnly();
        }

        return results;
    }

    public static String decryptString(String hash) {

        return decryptString(hash, StandardCharsets.UTF_8);
    }

    public static String decryptString(String hash, Charset charset) {

        byte[] byteArray = hash.getBytes(StandardCharsets.UTF_8);
        StringBuilder decrypt = new StringBuilder();

        for(int i = 0; i < byteArray.length; i++) {
            decrypt.append((char) (byteArray[i] ^ (i + 1)));
        }

        return (byteArray.length > 4) ? decrypt.substring(4) : decrypt.toString();
    }
}
