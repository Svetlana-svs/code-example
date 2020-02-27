package ru.softwerke.querybuilder.integration.jpa.entity;

import com.fasterxml.jackson.annotation.JsonIgnore;
import org.hibernate.annotations.NotFound;
import org.hibernate.annotations.NotFoundAction;

import javax.persistence.*;
import java.io.Serializable;

/*
 *  @author Svetlana Suvorova
 */
@Entity
@Table(name = "Benutzer")
public class Benutzer implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "ID", insertable = false, updatable = false, nullable = false)
    private int id;

    @Column(name = "Name", insertable = false, updatable = false, nullable = false, length = 30)
    private String name;

    @JsonIgnore
    @Column(name = "Passwort", insertable = false, updatable = false, nullable = false, length = 30)
    private String password;

    @OneToOne(cascade = CascadeType.ALL, fetch = FetchType.EAGER)
//    @NotFound(action = NotFoundAction.IGNORE)
    @JoinColumn(name = "ID", referencedColumnName = "BenutzerID", insertable = false, updatable = false, nullable = true)
    private BenutzerDetail benutzerDetail;


    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public BenutzerDetail getBenutzerDetail() {
        return benutzerDetail;
    }

    public void setBenutzerDetail(BenutzerDetail benutzerDetail) {
        this.benutzerDetail = benutzerDetail;
    }
}
