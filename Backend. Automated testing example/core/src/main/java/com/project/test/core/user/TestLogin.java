package project.test.core.user;

import project.test.core.TestCase;
import project.test.core.common.Configuration;
import project.test.core.common.TestResult;
import project.test.core.common.UserCredentials;
import project.test.core.common.Writer;
import project.test.core.impl.TestCaseImpl;
import org.openqa.selenium.*;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

import java.net.URI;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class TestLogin extends TestCaseImpl
        implements TestCase {

    private final static String TEST_USER_LOGIN_NAME = "User Login";
    private final static String PROPERTY_NAME_HOST = "host";

    public TestLogin() {
        testName = TEST_USER_LOGIN_NAME;
    }

    @Override
    public List<TestResult> test() {
        List<TestResult> results = new ArrayList<TestResult>();

        try {
            results.add(testLoginFailure());
            results.add(testLoginSuccess());
            results.add(testLogout());
        }
        catch (Exception e) {
            results.add(new TestResult(getTestName(), TestResult.FAIL,"Could not perform this test block.", e));
            Writer.writeScreenshot(getTestName(), driver);
        }

        return results;
    }

    private TestResult testLoginFailure() {
        testCase = " Failure";
        TestResult result = new TestResult(getTestName(), TestResult.OK);

        String userLogin = UUID.randomUUID().toString().replaceAll("-", "").substring(0, 8);
        String userPassword = UUID.randomUUID().toString().replaceAll("-", "").substring(0, 10);

        try {
            result = testLogin(testCase, userLogin, userPassword);
        }
        catch (TimeoutException e) {
            result.fail("A Java exception occurred while trying to execute test.");
        }

        return result;
    }

    private TestResult testLoginSuccess() {
        TestResult result;
        testCase = " Success";

        try {
            result = testLogin(testCase, UserCredentials.getLogin(), UserCredentials.getPassword());
        }
        catch (TimeoutException e) {
            return new TestResult(getTestName(), TestResult.OK);
        }
        result.fail("A Java exception occurred while trying to execute test.");

        return result;
    }

    public TestResult testLoginSuccess(String login, String password) {
        TestResult result;
        testCase = " Success";

        try {
            result = testLogin(testCase, UserCredentials.getLogin(login), UserCredentials.getPassword(password));
        }
        catch (TimeoutException e) {
            return new TestResult(getTestName(), TestResult.OK);
        }
        result.fail("A Java exception occurred while trying to execute test.");

        return result;
    }

    private TestResult testLogin(String testCase, String userLogin, String userPassword)
            throws TimeoutException {
        TestResult result = new TestResult(getTestName(), TestResult.OK);

        if (driver == null) {
            result.fail("WebDriver is not set.");
        }

        URI pageURI = URI.create(Configuration.getProperty(PROPERTY_NAME_HOST));
        driver.get(pageURI.toString());

        WebElement linkLogin = driver.findElement(By.cssSelector("a[href*='login']"));
        linkLogin.click();

        By locatorInputPassword = By.cssSelector("input[type='password']");
        WebDriverWait wait = new WebDriverWait(driver, webDriverResponseTimeout);
        wait.until(ExpectedConditions.presenceOfElementLocated(locatorInputPassword));

        WebElement inputLogin = driver.findElement(By.cssSelector("input[name$='_login']"));
        inputLogin.sendKeys(userLogin);

        WebElement inputPassword =  driver.findElement(locatorInputPassword);
        inputPassword.sendKeys(userPassword);
        inputPassword.submit();

        wait.until(ExpectedConditions.or(
                ExpectedConditions.attributeContains(inputLogin,  "aria-invalid", "true"),
                ExpectedConditions.attributeContains(inputPassword,  "aria-invalid", "true"),
                ExpectedConditions.presenceOfElementLocated(By.className("alert-danger")))
        );

        return result;
    }

    private TestResult testLogout() {
        testCase = " Logout";
        TestResult result = new TestResult(getTestName(), TestResult.OK);

        try {
            WebElement linkLogout = driver.findElement(By.cssSelector("a[href*='logout']"));
            linkLogout.click();
            WebDriverWait wait = new WebDriverWait(driver, webDriverResponseTimeout);
            wait.until(ExpectedConditions.presenceOfElementLocated(new By.ByCssSelector("a[href*='login']")));
        }
        catch (TimeoutException e) {
            result.fail("Logout is failed.");
        }

        return result;
    }
}
