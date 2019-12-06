package com.package.name.integration.jpa.core.service;

import org.osgi.framework.Bundle;
import org.osgi.framework.BundleContext;
import org.osgi.framework.FrameworkUtil;
import org.osgi.framework.ServiceReference;
import org.osgi.service.component.annotations.Activate;
import org.osgi.service.component.annotations.Component;
import org.osgi.service.component.annotations.ConfigurationPolicy;
import org.osgi.service.metatype.annotations.Designate;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.spi.PersistenceProvider;
import java.util.Map;


@Designate(ocd = JpaEntityManagerConfiguration.class)
@Component(
        service = JpaEntityManagerFactory.class,
        configurationPolicy = ConfigurationPolicy.REQUIRE,
        immediate = true
)
public class JpaEntityManagerFactory {

    private static final Logger logger = LoggerFactory.getLogger(JpaEntityManagerFactory.class);

    private static EntityManagerFactory entityManagerFactory;

    private static Map<String, Object> persistenceProperties;

    @Activate
    protected void activate(final JpaEntityManagerConfiguration configuration) {
        persistenceProperties = JpaPersistenceProperties.getProperties(configuration);
    }

    public static EntityManager getEntityManager() {
        return getEntityManagerFactory().createEntityManager();
    }

    private static EntityManagerFactory getEntityManagerFactory() {
        if (entityManagerFactory == null) {
            Bundle thisBundle = FrameworkUtil.getBundle(JpaEntityManagerFactory.class);

            BundleContext context = thisBundle.getBundleContext();
            ServiceReference serviceReference = context.getServiceReference(PersistenceProvider.class.getName());

            PersistenceProvider persistenceProvider = (PersistenceProvider) context
                    .getService(serviceReference);

            entityManagerFactory = persistenceProvider.createEntityManagerFactory(
                    "userprofile-hibernate", persistenceProperties
            );
        }

        return entityManagerFactory;
    }
}
