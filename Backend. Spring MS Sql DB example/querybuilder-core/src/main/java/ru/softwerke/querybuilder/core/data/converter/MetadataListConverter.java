package ru.softwerke.querybuilder.core.data.converter;

import org.apache.commons.lang3.StringUtils;
import ru.softwerke.querybuilder.core.data.queryForm.LookUp;
import ru.softwerke.querybuilder.core.data.queryForm.Metadata;
import ru.softwerke.querybuilder.integration.jpa.entity.Satzeintrag;
import ru.softwerke.querybuilder.integration.jpa.entity.Schluesseltabelle;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;

/*
 *  @author Svetlana Suvorova
 *
 *  Conversions data received from the entity to display on the client.
 *
 */
public class MetadataListConverter {

    public static List<Metadata> convert(List<Satzeintrag>satzeintrags) {
        if ((satzeintrags == null) || (satzeintrags.size() == 0)) {
            return new ArrayList<>();
        }
        return  satzeintrags.stream()
                .map(satzeintrag -> new Metadata(
                        satzeintrag.getSatzPos(),
                        satzeintrag.getMerkmal().getBeschreibung(),
                        satzeintrag.getVerschluesselung().getLength(),
                        satzeintrag.getVerschluesselung().getType(),
                        satzeintrag.getFieldType(),
                        satzeintrag.getFieldAlias(),
                        // Property "database"
                        satzeintrag.getSatzaufbau().getDateiList().stream()
                                .filter(d -> d.getSachdateiType().equals("TABLE"))
                                .map(d -> d.getSachdateiAlias())
                                .findAny()
                                .orElse(null),
                        // Property "table"
                        satzeintrag.getSatzaufbau().getDateiList().stream()
                                .filter(d -> d.getSachdateiType().equals("TABLE"))
                                .map(d -> d.getSachdateiTabelle())
                                .findAny()
                                .orElse(null),
                        // Property lookup "data"
                        satzeintrag.getSchluesseltabelle().stream()
                        .sorted(Comparator.comparing(Schluesseltabelle::getSchluesselPos))
                        .map(s -> new LookUp(s.getWert(), s.getAuspraegung().getBeschreibung()))
                        .collect(Collectors.toList())
                        ))
                .filter(m -> StringUtils.isNotBlank(m.getTable()))
                .collect(Collectors.toList());
    }
}