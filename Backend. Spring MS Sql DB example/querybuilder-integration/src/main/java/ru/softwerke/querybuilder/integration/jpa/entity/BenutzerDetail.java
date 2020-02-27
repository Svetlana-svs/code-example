package ru.softwerke.querybuilder.integration.jpa.entity;

import com.fasterxml.jackson.annotation.JsonIgnore;

import javax.persistence.*;
import java.io.Serializable;

/*
 *  @author Svetlana Suvorova
 */
@Entity
@Table(name = "BenutzerDetail")
public class BenutzerDetail implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "BenutzerID", insertable = false, updatable = false, nullable = false)
    private int benutzerId;

    @Column(name = "Aktivitaet", insertable = false, updatable = false, nullable = false)
    private int active;


    public int getBenutzerId() {
        return benutzerId;
    }

    public void setBenutzerId(int benutzerId) {
        this.benutzerId = benutzerId;
    }

    public int getActive() {
        return active;
    }

    public void setActive(int active) {
        this.active = active;
    }
}
