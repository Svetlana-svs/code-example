package ru.softwerke.querybuilder.integration.jpa.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import ru.softwerke.querybuilder.integration.jpa.entity.Datei;

import java.util.List;

/*
 *  @author Svetlana Suvorova
 */
@Repository
public interface DateiRepository extends JpaRepository<Datei, String> {

    @Query("SELECT d FROM Datei d WHERE d.sachdateiAlias IS NOT NULL")
    List<Datei> findDatabaseAllTables();

}
