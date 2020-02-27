package ru.softwerke.querybuilder.integration.jpa.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import ru.softwerke.querybuilder.integration.jpa.dto.User;
import ru.softwerke.querybuilder.integration.jpa.entity.Benutzer;

import java.util.List;
import java.util.Set;

/*
 *  @author Svetlana Suvorova
 */
@Repository
public interface UserRepository extends JpaRepository<Benutzer, Long> {

    @Query(value="SELECT b FROM Benutzer b WHERE b.benutzerDetail.active = 1")
    List<Benutzer> findAllActiveUser();

    @Query(value="SELECT b FROM Benutzer b WHERE b.name IN :userNames")
    List<Benutzer> findAllUserByName(@Param("userNames") Set<String> userNames);

    @Query(value="SELECT new ru.softwerke.querybuilder.integration.jpa.dto.User(b.id, b.name) FROM Benutzer b WHERE b.id IN :userIds")
    List<User> findAllUserById(@Param("userIds") Set<Integer> userIds);

    @Query(value="SELECT new ru.softwerke.querybuilder.integration.jpa.dto.User(b.id, b.name) FROM Benutzer b LEFT JOIN BenutzerDetail bd ON b.id = bd.benutzerId WHERE bd.benutzerId IS NULL OR (bd.benutzerId IS NOT NULL AND bd.active = 1)")
    List<User> findAllUser();
}
