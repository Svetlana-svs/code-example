package ...widget.impl.redmine.dto;


/*
 * DTO class to display issue data received from the external tracking tool on the client.
 */
public class IssueDTO {

    private int id;
    private String description;
    private String priorityText;
    private String subject;

    public IssueDTO(final int id, final String subject, final String description, final String priorityText) {
        this.id = id;
        this.subject = subject;
        this.description = description;
        this.priorityText = priorityText;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getSubject() {
        return subject;
    }

    public void setSubject(String subject) {
        this.subject = subject;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getPriorityText() {
        return priorityText;
    }

    public void setPriorityText(String priorityText) {
        this.priorityText = priorityText;
    }
}
