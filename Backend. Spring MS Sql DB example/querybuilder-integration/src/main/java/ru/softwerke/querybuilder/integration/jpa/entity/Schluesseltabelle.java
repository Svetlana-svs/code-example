package ru.softwerke.querybuilder.integration.jpa.entity;

import javax.persistence.*;
import java.io.Serializable;
import java.util.List;

/*
 *  @author Svetlana Suvorova
 */
@Entity
@Table(name = "Schluesseltabelle")
public class Schluesseltabelle implements Serializable {

    @EmbeddedId
    private SchluesseltabellePK id;

    @Column(name = "VerschluesselungID", insertable = false, updatable = false, nullable = false)
    private int verschluesselungId;

    @Column(name = "SchluesselPos", insertable = false, updatable = false, nullable = false)
    private int SchluesselPos;

    @Column(name = "AuspraegungID", insertable = false, updatable = false, nullable = false, length = 250)
    private int auspraegungId;

    @Column(name = "Wert", insertable = false, updatable = false, nullable = false, length = 50)
    private String wert;

    @OneToOne(cascade = CascadeType.ALL, fetch = FetchType.EAGER)
    @JoinColumn(name = "AuspraegungID", referencedColumnName = "ID", insertable = false, updatable = false, nullable = false)
    private Auspraegung auspraegung;

    public SchluesseltabellePK getId() {
        return id;
    }

    public void setId(SchluesseltabellePK id) {
        this.id = id;
    }

    public int getVerschluesselungId() {
        return verschluesselungId;
    }

    public void setVerschluesselungId(int verschluesselungId) {
        this.verschluesselungId = verschluesselungId;
    }

    public int getSchluesselPos() {
        return SchluesselPos;
    }

    public void setSchluesselPos(int schluesselPos) {
        SchluesselPos = schluesselPos;
    }

    public int getAuspraegungId() {
        return auspraegungId;
    }

    public void setAuspraegungId(int auspraegungId) {
        this.auspraegungId = auspraegungId;
    }

    public String getWert() {
        return wert;
    }

    public void setWert(String wert) {
        this.wert = wert;
    }

    public Auspraegung getAuspraegung() {
        return auspraegung;
    }

    public void setAuspraegung(Auspraegung auspraegung) {
        this.auspraegung = auspraegung;
    }
}
