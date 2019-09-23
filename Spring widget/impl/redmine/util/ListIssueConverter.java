package ...widget.impl.redmine.util;


import com.taskadapter.redmineapi.bean.Issue;
import ...widget.impl.redmine.dto.IssueDTO;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

/*
 * Conversions issues data received from the external issue tracking tool to display on the client.
 */
public class ListIssueConverter {

    public static List<IssueDTO> convert(List<Issue> issues) {
        if ((issues == null) || (issues.size() == 0)) {
            return new ArrayList<>();
        }
        return  issues.stream()
                .map(x -> new IssueDTO(x.getId(), x.getSubject(), x.getDescription(), x.getPriorityText()))
                .collect(Collectors.toList());
    }
}