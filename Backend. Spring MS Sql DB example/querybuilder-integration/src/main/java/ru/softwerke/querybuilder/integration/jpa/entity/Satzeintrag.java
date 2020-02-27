package ru.softwerke.querybuilder.integration.jpa.entity;

import org.hibernate.annotations.NotFound;
import org.hibernate.annotations.NotFoundAction;

import javax.persistence.*;
import java.io.Serializable;
import java.util.Set;

/*
 *  @author Svetlana Suvorova
 */
@Entity
@Table(name = "Satzeintrag")
public class Satzeintrag implements Serializable {

    @EmbeddedId
    private SatzeintragPK id;

    @Column(name = "SatzaufbauID", insertable = false, updatable = false, nullable = false)
    private int satzaufbauId;

    @Column(name = "SatzPos", insertable = false, updatable = false, nullable = false)
    private int satzPos;

    @ManyToOne
    @JoinColumn(name = "SatzaufbauID", insertable = false, updatable = false, nullable = false)
    private Satzaufbau satzaufbau;

    @OneToOne(cascade = CascadeType.ALL, fetch = FetchType.EAGER)
    @JoinColumn(name = "MerkmalID", referencedColumnName = "ID", insertable = false, updatable = false)
    private Merkmal merkmal;

    @OneToOne(cascade = CascadeType.ALL, fetch = FetchType.EAGER)
    @JoinColumn(name = "VerschluesselungID", referencedColumnName = "ID", insertable = false, updatable = false, nullable = false)
    private Verschluesselung verschluesselung;

    @Column(name = "FeldTyp", nullable = false, length = 1, insertable = false, updatable = false)
    private String fieldType;

    @Column(name = "FeldAlias", nullable = true, length = 30, insertable = false, updatable = false)
    private String fieldAlias;

    @OneToMany( cascade = CascadeType.ALL, fetch = FetchType.EAGER)
    @NotFound(action = NotFoundAction.IGNORE)
    @JoinColumn(name = "VerschluesselungID", referencedColumnName = "VerschluesselungID", insertable = false, updatable = false, nullable = true)
    private Set<Schluesseltabelle> schluesseltabelle;

    public SatzeintragPK getId() {
        return id;
    }

    public void setId(SatzeintragPK id) {
        this.id = id;
    }

    public int getSatzaufbauId() {
        return satzaufbauId;
    }

    public void setSatzaufbauId(int satzaufbauId) {
        this.satzaufbauId = satzaufbauId;
    }

    public int getSatzPos() {
        return satzPos;
    }

    public void setSatzPos(int satzPos) {
        this.satzPos = satzPos;
    }

    public Satzaufbau getSatzaufbau() {
        return satzaufbau;
    }

    public void setSatzaufbau(Satzaufbau satzaufbau) {
        this.satzaufbau = satzaufbau;
    }

    public Merkmal getMerkmal() {
        return merkmal;
    }

    public void setMerkmal(Merkmal merkmal) {
        this.merkmal = merkmal;
    }

    public Verschluesselung getVerschluesselung() {
        return verschluesselung;
    }

    public void setVerschluesselung(Verschluesselung verschluesselung) {
        this.verschluesselung = verschluesselung;
    }

    public String getFieldType() {
        return fieldType;
    }

    public void setFeldType(String fieldType) {
        this.fieldType = fieldType;
    }

    public String getFieldAlias() {
        return fieldAlias;
    }

    public void setFieldAlias(String fieldAlias) {
        this.fieldAlias = fieldAlias;
    }

    public void setFieldType(String fieldType) {
        this.fieldType = fieldType;
    }

    public Set<Schluesseltabelle> getSchluesseltabelle() {
        return schluesseltabelle;
    }

    public void setSchluesseltabelle(Set<Schluesseltabelle> schluesseltabelle) {
        this.schluesseltabelle = schluesseltabelle;
    }
}
