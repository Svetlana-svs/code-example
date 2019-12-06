package project.test.core.common;

public class TestResult {
    public static final int OK = 1;
    public static final int FAIL = 0;

    private String name;
    private Integer result;
    private String description = "";
    private Exception exception;

    public TestResult(String name)
    {
        this.name = name;
        this.result = null;
        this.description = "";
    }

    public TestResult(String name, Integer result)
    {
        this.name = name;
        this.result = (result == TestResult.OK || result == TestResult.FAIL) ? result : TestResult.FAIL;
    }

    public TestResult(String name, Integer result, String description, Exception exception)
    {
        this.name = name;
        this.result = (result == TestResult.OK || result == TestResult.FAIL) ? result : null;
        this.description = description;
        this.exception = exception;
    }

    static String[] getHeader() {
        return new String[] {
                "NAME",
                "RESULT",
                "FAILURE DESCRIPTION",
                "JAVA EXCEPTION"
        };
    }

    String[] getRecord() {
        return new String[]{
                name,
                getResult(),
                getDescription(),
                getException()
        };
    }

    public String getResult()
    {
        return (this.result == null) ? "" : ((this.result == TestResult.OK) ? "OK" : ((result == TestResult.FAIL) ? "FAIL" : ""));
    }

    public void setResult(Integer result)
    {
        if (result == TestResult.OK || result == TestResult.FAIL) {
            this.result = result;
        }
    }

    public String getName()
    {
        return this.name;
    }

    public void setName(String name)
    {
        this.name = name;
    }

    public String getDescription()
    {
        return  (description == null || description.isEmpty()) ? "" : "\"" + description.replaceAll("\"","\\\"") + "\"";
    }

    public void setDescription(String description)
    {
        this.description += (((description == null || description.isEmpty()) ? "" : " " ) + description);
    }

    public String getException()
    {
        return (exception == null) ? "" : "\"" + exception.toString().replaceAll("\"","\\\"") + "\"";
    }

    public void setException(Exception exception)
    {
        this.exception = exception;
    }

    public void fail(String description)
    {
        this.setResult(TestResult.FAIL);
        this.setDescription(description);
    }

    public void fail(String description, Exception exception)
    {
        this.setResult(TestResult.FAIL);
        this.setDescription(description);
        this.setException(exception);
    }
}
