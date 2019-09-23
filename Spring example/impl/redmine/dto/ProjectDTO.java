package ...widget.impl.redmine.dto;


import java.util.ArrayList;
import java.util.List;

/*
 * DTO class to display project data received from the external tracking tool on the client.
 */
public class ProjectDTO {

    private Integer id;
    private  String identifier;
    private String name;

    private List<IssueDTO> issues;

    public ProjectDTO() {}

    public ProjectDTO(final int id, final String identifier, final String name) {
        this.id = id;
        this.identifier = identifier;
        this.name = name;
        this.issues = new ArrayList<>();
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getIdentifier() {
        return identifier;
    }

    public void setIdentifier(String identifier) {
        this.identifier = identifier;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public List<IssueDTO> getIssues() {
        return issues;
    }

    public void setIssues(List<IssueDTO> issues) {
        this.issues = issues;
    }
}
