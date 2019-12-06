package project.test.core.impl;

import project.test.core.TestCase;
import project.test.core.common.Configuration;
import project.test.core.common.TestResult;
import org.openqa.selenium.WebDriver;

import java.util.ArrayList;
import java.util.List;

public class TestCaseImpl implements TestCase {

    public static WebDriver driver = null;

    private final static String PROPERTY_NAME_WEBDRIVER_RESPONSE_TIMEOUT = "webdriver.response.timeout";

    public static int webDriverResponseTimeout = Integer.parseInt(Configuration.getProperty(PROPERTY_NAME_WEBDRIVER_RESPONSE_TIMEOUT));
    protected String testName = "";
    protected String testCase = "";

    public List<TestResult> test() {
         // TODO: throw exception by empty driver
        return new ArrayList<TestResult>();
    }

    protected String getTestName() {
        return String.format("%s %s", testName, testCase);
    }
}
