package ru.softwerke.querybuilder.integration.jpa.entity;

import javax.persistence.Column;
import javax.persistence.Embeddable;
import java.io.Serializable;

/*
 *  @author Svetlana Suvorova
 */
@Embeddable
public class SchluesseltabellePK implements Serializable {

    @Column(name = "VerschluesselungID", insertable = false, updatable = false, nullable = false)
    private int verschluesselungId;

    @Column(name = "SchluesselPos", insertable = false, updatable = false, nullable = false)
    private int SchluesselPos;
}
