package ru.softwerke.querybuilder.integration.jpa.entity;

import javax.persistence.*;
import java.io.Serializable;
import java.util.Set;

/*
 *  @author Svetlana Suvorova
 */
@Entity
@Table(name = "Satzaufbau")
public class Satzaufbau implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "ID", insertable = false, updatable = false, nullable = false)
    private int id;

    @OneToMany(cascade = CascadeType.ALL, fetch = FetchType.EAGER)
    @JoinColumn(name = "SatzaufbauID", insertable = false, updatable = false, nullable = false)
    private Set<Datei> dateiList;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public Set<Datei> getDateiList() {
        return dateiList;
    }

    public void setDateiList(Set<Datei> dateiList) {
        this.dateiList = dateiList;
    }
}
