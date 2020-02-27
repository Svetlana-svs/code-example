package ru.softwerke.querybuilder.integration.jpa.entity;

import javax.persistence.*;
import java.io.Serializable;
import java.time.LocalDateTime;

/*
 *  @author Svetlana Suvorova
 */
@Entity
@Table(name = "duva_vw_Datei")
public class Datei implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "ID", insertable = false, updatable = false, nullable = false)
    private int id;

    @Column(name = "Beschreibung", nullable = false, length = 250)
    private String beschreibung;

    @Column(name = "Typ", nullable = false, length = 1)
    private String type;

    @Column(name = "Kurzbeschreibung", nullable = true, length = 80)
    private String kurzbeschreibung;

    @Column(name = "Zusatzinformation", nullable = true, length = 300)
    private String zusatzinformation;

    @Column(name = "SachgebietID", nullable = true)
    private Integer sachgebietID;

    @Column(name = "Sachgebiet", nullable = true, length = 250)
    private String sachgebiet;

    @Column(name = "ErhebungID", nullable = false)
    private int erhebungID;

    @Column(name = "Erhebung", nullable = true, length = 250)
    private String erhebung;

    @Column(name = "TraegerID", nullable = false)
    private int traegerID;

    @Column(name = "Merkmalstraeger", nullable = true, length = 250)
    private String merkmalstraeger;

    @Column(name = "RaumbezugID", nullable = false)
    private int RaumbezugID;

    @Column(name = "Raumbezug", nullable = true, length = 1)
    private String raumbezug;

    @Column(name = "ZeitbezugID", nullable = false, length = 1)
    private String zeitbezugID;

    @Column(name = "Zeitbezug", nullable = true, length = 23)
    private String zeitbezug;

    @Column(name = "Vom", nullable = true, length = 10)
    private String vom;

    @Column(name = "Bis", nullable = true, length = 10)
    private String bis;

    @Column(name = "SatzaufbauID", insertable = false, updatable = false, nullable = false)
    private int satzaufbauId;

    @Column(name = "Satzaufbau", insertable = false, updatable = false, nullable = true, length = 250)
    private String satzaufbau;

    @Column(name = "StatusID", nullable = true)
    private Integer statusID;

    @Column(name = "Status", nullable = true, length = 250)
    private String status;

    @Column(name = "SachdateiTyp", nullable = false, insertable = false, updatable = false, length = 5)
    private String sachdateiType;

    @Column(name = "Sachdatei", nullable = true, insertable = false, updatable = false, length = 255)
    private String sachdatei;

    @Column(name = "SachdateiFormat", nullable = true, insertable = false, updatable = false, length = 7)
    private String sachdateiFormat;

    @Column(name = "Feldtrennzeichen", nullable = true, length = 1)
    private String feldtrennzeichen;

    @Column(name = "Stringbegrenzer", nullable = true, length = 1)
    private String stringbegrenzer;

    @Column(name = "SachdateiAlias", nullable = true, length = 30)
    private String sachdateiAlias;

    @Column(name = "SachdateiTabelle", nullable = true, length = 30)
    private String sachdateiTabelle;

    @Column(name = "SachdateiBenutzer", nullable = true, length = 30)
    private String sachdateiBenutzer;

    @Column(name = "SachdateiKennwort", nullable = true, length = 30)
    private String sachdateiKennwort;

    @Column(name = "Kopfzeilen", nullable = true)
    private Integer kopfzeilen;

    @Column(name = "\"Letzte Aenderung\"", nullable = true)
    private LocalDateTime letzteAenderung;

    @Column(name = "AenderungBenutzerID", nullable = true)
    private Integer aenderungBenutzerID;

    @Column(name = "\"Aenderung Benutzer\"", nullable = true, length = 30)
    private String aenderungBenutzer;

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

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public String getKurzbeschreibung() {
        return kurzbeschreibung;
    }

    public void setKurzbeschreibung(String kurzbeschreibung) {
        this.kurzbeschreibung = kurzbeschreibung;
    }

    public String getZusatzinformation() {
        return zusatzinformation;
    }

    public void setZusatzinformation(String zusatzinformation) {
        this.zusatzinformation = zusatzinformation;
    }

    public Integer getSachgebietID() {
        return sachgebietID;
    }

    public void setSachgebietID(Integer sachgebietID) {
        this.sachgebietID = sachgebietID;
    }

    public String getSachgebiet() {
        return sachgebiet;
    }

    public void setSachgebiet(String sachgebiet) {
        this.sachgebiet = sachgebiet;
    }

    public int getErhebungID() {
        return erhebungID;
    }

    public void setErhebungID(int erhebungID) {
        this.erhebungID = erhebungID;
    }

    public String getErhebung() {
        return erhebung;
    }

    public void setErhebung(String erhebung) {
        this.erhebung = erhebung;
    }

    public int getTraegerID() {
        return traegerID;
    }

    public void setTraegerID(int traegerID) {
        this.traegerID = traegerID;
    }

    public String getMerkmalstraeger() {
        return merkmalstraeger;
    }

    public void setMerkmalstraeger(String merkmalstraeger) {
        this.merkmalstraeger = merkmalstraeger;
    }

    public int getRaumbezugID() {
        return RaumbezugID;
    }

    public void setRaumbezugID(int raumbezugID) {
        RaumbezugID = raumbezugID;
    }

    public String getRaumbezug() {
        return raumbezug;
    }

    public void setRaumbezug(String raumbezug) {
        this.raumbezug = raumbezug;
    }

    public String getZeitbezugID() {
        return zeitbezugID;
    }

    public void setZeitbezugID(String zeitbezugID) {
        this.zeitbezugID = zeitbezugID;
    }

    public String getZeitbezug() {
        return zeitbezug;
    }

    public void setZeitbezug(String zeitbezug) {
        this.zeitbezug = zeitbezug;
    }

    public String getVom() {
        return vom;
    }

    public void setVom(String vom) {
        this.vom = vom;
    }

    public String getBis() {
        return bis;
    }

    public void setBis(String bis) {
        this.bis = bis;
    }

    public int getSatzaufbauId() {
        return satzaufbauId;
    }

    public void setSatzaufbauId(int satzaufbauId) {
        this.satzaufbauId = satzaufbauId;
    }

    public String getSatzaufbau() {
        return satzaufbau;
    }

    public void setSatzaufbau(String satzaufbau) {
        this.satzaufbau = satzaufbau;
    }

    public Integer getStatusID() {
        return statusID;
    }

    public void setStatusID(Integer statusID) {
        this.statusID = statusID;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getSachdateiType() {
        return sachdateiType;
    }

    public void setSachdateiType(String sachdateiType) {
        this.sachdateiType = sachdateiType;
    }

    public String getSachdatei() {
        return sachdatei;
    }

    public void setSachdatei(String sachdatei) {
        this.sachdatei = sachdatei;
    }

    public String getSachdateiFormat() {
        return sachdateiFormat;
    }

    public void setSachdateiFormat(String sachdateiFormat) {
        this.sachdateiFormat = sachdateiFormat;
    }

    public String getFeldtrennzeichen() {
        return feldtrennzeichen;
    }

    public void setFeldtrennzeichen(String feldtrennzeichen) {
        this.feldtrennzeichen = feldtrennzeichen;
    }

    public String getStringbegrenzer() {
        return stringbegrenzer;
    }

    public void setStringbegrenzer(String stringbegrenzer) {
        this.stringbegrenzer = stringbegrenzer;
    }

    public String getSachdateiAlias() {
        return sachdateiAlias;
    }

    public void setSachdateiAlias(String sachdateiAlias) {
        this.sachdateiAlias = sachdateiAlias;
    }

    public String getSachdateiTabelle() {
        return sachdateiTabelle;
    }

    public void setSachdateiTabelle(String sachdateiTabelle) {
        this.sachdateiTabelle = sachdateiTabelle;
    }

    public String getSachdateiBenutzer() {
        return sachdateiBenutzer;
    }

    public void setSachdateiBenutzer(String sachdateiBenutzer) {
        this.sachdateiBenutzer = sachdateiBenutzer;
    }

    public String getSachdateiKennwort() {
        return sachdateiKennwort;
    }

    public void setSachdateiKennwort(String sachdateiKennwort) {
        this.sachdateiKennwort = sachdateiKennwort;
    }

    public Integer getKopfzeilen() {
        return kopfzeilen;
    }

    public void setKopfzeilen(Integer kopfzeilen) {
        this.kopfzeilen = kopfzeilen;
    }

    public LocalDateTime getLetzteAenderung() {
        return letzteAenderung;
    }

    public void setLetzteAenderung(LocalDateTime letzteAenderung) {
        this.letzteAenderung = letzteAenderung;
    }

    public Integer getAenderungBenutzerID() {
        return aenderungBenutzerID;
    }

    public void setAenderungBenutzerID(Integer aenderungBenutzerID) {
        this.aenderungBenutzerID = aenderungBenutzerID;
    }

    public String getAenderungBenutzer() {
        return aenderungBenutzer;
    }

    public void setAenderungBenutzer(String aenderungBenutzer) {
        this.aenderungBenutzer = aenderungBenutzer;
    }
}
