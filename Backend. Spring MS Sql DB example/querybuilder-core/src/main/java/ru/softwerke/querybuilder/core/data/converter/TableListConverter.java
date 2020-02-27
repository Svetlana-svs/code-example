package ru.softwerke.querybuilder.core.data.converter;

import ru.softwerke.querybuilder.core.data.dto.Table;
import ru.softwerke.querybuilder.integration.jpa.entity.Datei;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

/*
 *  @author Svetlana Suvorova
 *
 *  Conversions data received from the entity to display on the client.
 *
 */
public class TableListConverter {

    public static List<Table> convert(List<Datei> dateis) {
        if ((dateis == null) || (dateis.size() == 0)) {
            return new ArrayList<>();
        }
        return  dateis.stream()
                .filter(datei -> datei.getSachdateiType().equals("TABLE"))
                .map(datei -> new Table(datei.getSatzaufbauId(), datei.getBeschreibung()))
                .collect(Collectors.toList());
    }
}