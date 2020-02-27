package ru.softwerke.querybuilder.integration.jpa.config;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.boot.autoconfigure.domain.EntityScan;
import org.springframework.boot.autoconfigure.orm.jpa.JpaProperties;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.boot.orm.jpa.EntityManagerFactoryBuilder;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Primary;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;
import org.springframework.orm.jpa.JpaTransactionManager;
import org.springframework.orm.jpa.JpaVendorAdapter;
import org.springframework.orm.jpa.LocalContainerEntityManagerFactoryBean;
import org.springframework.orm.jpa.persistenceunit.PersistenceUnitManager;
import org.springframework.orm.jpa.vendor.AbstractJpaVendorAdapter;
import org.springframework.orm.jpa.vendor.HibernateJpaVendorAdapter;
import org.springframework.transaction.annotation.EnableTransactionManagement;
import ru.softwerke.querybuilder.integration.jpa.context.DataSourceRouter;

import javax.persistence.EntityManagerFactory;
import javax.sql.DataSource;
import java.util.HashMap;
import java.util.Map;

/*
 *  @author Svetlana Suvorova
 */
@Configuration
@EnableJpaRepositories(basePackages = "ru.softwerke.querybuilder.integration.jpa.repository",
        entityManagerFactoryRef = "entityManagerFactory",
        transactionManagerRef = "transactionManager")
@EntityScan(basePackages = "ru.softwerke.querybuilder.integration.jpa.entity")
@EnableTransactionManagement
public class DataSourceConfiguration {

    @Autowired(required = false)
    private PersistenceUnitManager persistenceUnitManager;

    @Autowired
    ConnectionConfiguration connectionConfiguration;

    @Bean
    @Primary
    @ConfigurationProperties("app.jpa")
    public JpaProperties applicationJpaProperties() {
        return new JpaProperties();
    }

    @Primary
    @Bean(name = "routingDataSource")
    public DataSource routingDataSource() {
        Map<Object, Object> targetDataSources = new HashMap<>();
        ConnectionConfiguration.getConnections()
                .forEach( connection -> targetDataSources.put(connection.getAlias(), connection.getDataSource()));

        DataSourceRouter dataSourceRouter = new DataSourceRouter();
        dataSourceRouter.setTargetDataSources(targetDataSources);

        return dataSourceRouter;
    }

    @Bean
    @Primary
    public LocalContainerEntityManagerFactoryBean entityManagerFactory (
            final JpaProperties applicationJpaProperties) {

        EntityManagerFactoryBuilder builder =
                createEntityManagerFactoryBuilder(applicationJpaProperties);

        return builder.dataSource(routingDataSource())
                .packages("ru.softwerke.querybuilder.integration.jpa.entity")
                .persistenceUnit("entityManagerFactory").build();
    }

    @Bean
    @Primary
    public JpaTransactionManager transactionManager(
            @Qualifier("entityManagerFactory") final EntityManagerFactory factory) {
        return new JpaTransactionManager(factory);
    }

    private EntityManagerFactoryBuilder createEntityManagerFactoryBuilder (JpaProperties applicationJpaProperties) {
        JpaVendorAdapter jpaVendorAdapter =
                createJpaVendorAdapter(applicationJpaProperties);

        return new EntityManagerFactoryBuilder(jpaVendorAdapter,
                applicationJpaProperties.getProperties(), this.persistenceUnitManager);
    }

    private JpaVendorAdapter createJpaVendorAdapter (JpaProperties jpaProperties) {
        AbstractJpaVendorAdapter adapter = new HibernateJpaVendorAdapter();
        adapter.setShowSql(jpaProperties.isShowSql());
        adapter.setDatabase(jpaProperties.getDatabase());
        adapter.setDatabasePlatform(jpaProperties.getDatabasePlatform());
        adapter.setGenerateDdl(jpaProperties.isGenerateDdl());

        return adapter;
    }
}
