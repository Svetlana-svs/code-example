package ru.softwerke.querybuilder.integration.jpa.entity;

import javax.persistence.*;
import java.io.Serializable;

/*
 *  @author Svetlana Suvorova
 */
@Entity
@Table(name = "Verschluesselung")
public class Verschluesselung implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "ID", insertable = false, updatable = false, nullable = false)
    private int id;

    @Column(name = "Typ", nullable = false)
    private String type;

    @Column(name = "Laenge", nullable = false)
    private int length;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public int getLength() {
        return length;
    }

    public void setLength(int length) {
        this.length = length;
    }
}
