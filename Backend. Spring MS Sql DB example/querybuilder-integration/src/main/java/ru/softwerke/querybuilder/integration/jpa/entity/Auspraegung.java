package ru.softwerke.querybuilder.integration.jpa.entity;

import javax.persistence.*;
import java.io.Serializable;

/*
 *  @author Svetlana Suvorova
 */
@Entity
@Table(name = "Auspraegung")
public class Auspraegung  implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "ID", insertable = false, updatable = false, nullable = false)
    private int id;

    @Column(name = "Beschreibung", insertable = false, updatable = false, nullable = false, length = 250)
    private String beschreibung;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getBeschreibung() {
        return beschreibung;
    }

    public void setBeschreibung(String beschreibung) {
        this.beschreibung = beschreibung;
    }
}
