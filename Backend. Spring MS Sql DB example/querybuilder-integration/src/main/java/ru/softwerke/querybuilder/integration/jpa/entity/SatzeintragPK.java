package ru.softwerke.querybuilder.integration.jpa.entity;

import javax.persistence.Column;
import javax.persistence.Embeddable;
import java.io.Serializable;

/*
 *  @author Svetlana Suvorova
 */
@Embeddable
public class SatzeintragPK implements Serializable {

    @Column(name = "SatzPos", insertable = false, updatable = false, nullable = false)
    private int satzPos;

    @Column(name = "SatzaufbauID", insertable = false, updatable = false, nullable = false)
    private int satzaufbauId;
}
