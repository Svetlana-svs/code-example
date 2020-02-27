package ru.softwerke.querybuilder.integration.jpa.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import ru.softwerke.querybuilder.integration.jpa.entity.Satzeintrag;

import java.util.List;

/*
 *  @author Svetlana Suvorova
 */
@Repository
public interface AdminRepository extends JpaRepository<Satzeintrag, String> {

    @Query("SELECT se FROM Satzeintrag se WHERE se.satzaufbauId = :satzaufbauId")
    List<Satzeintrag> findAllTableFields(@Param("satzaufbauId") int satzaufbauId);

    @Query("SELECT se FROM Satzeintrag se WHERE se.satzaufbauId = :satzaufbauId AND se.verschluesselung IS NOT NULL AND se.verschluesselung.type = 'S'")
    List<Satzeintrag> findAllTableFieldsByType(@Param("satzaufbauId") int satzaufbauId);
}
