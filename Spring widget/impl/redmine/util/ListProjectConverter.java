package ...widget.impl.redmine.util;

import com.taskadapter.redmineapi.bean.Project;
import ...widget.impl.redmine.dto.ProjectDTO;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

/*
 * Conversions projects data received from the external issue tracking tool to display on the client.
 */
public class ListProjectConverter {

    public static List<ProjectDTO> convert(List<Project> projects) {
        if ((projects == null) || (projects.size() == 0)) {
            return new ArrayList();
        }
        return  projects.stream()
                .map(p -> new ProjectDTO(p.getId(), p.getIdentifier(), p.getName()))
                .collect(Collectors.toList());
    }
}