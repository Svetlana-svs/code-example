package project.test.core.message;

import project.test.core.TestCase;
import project.test.core.common.Configuration;
import project.test.core.common.TestResult;
import project.test.core.common.Writer;
import project.test.core.user.TestLogin;
import project.test.core.impl.TestCaseImpl;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import java.util.ArrayList;
import java.util.List;

public class TestMessage extends TestCaseImpl
        implements TestCase {

    private final static String PROPERTY_NAME_HOST = "host";
    private final static String PROPERTY_NAME_GROUP_PATH = "group.path";

    private final static String PROPERTY_NAME_USER_LOGIN = "message.user.login";
    private final static String PROPERTY_NAME_USER_PASSWORD = "message.user.password";

    @Override
    public List<TestResult> test() {
        List<TestResult> results = new ArrayList<TestResult>();

        try {
           TestLogin testLogin = new TestLogin();
           testLogin.testLoginSuccess(PROPERTY_NAME_USER_LOGIN, PROPERTY_NAME_USER_PASSWORD);

            // Navigate to a group messages view page
            navigateToPageMessages();

             // Test new message creating
            Message message = new Message();
            TestMessageEdit testMessageCreate = new TestMessageEdit(message, MessageMode.CREATE);
            results.addAll(testMessageCreate.test());

            // Test view saved new message on the group messages view page
            TestMessageView testMessageCreateView = new TestMessageView(message);
            results.addAll(testMessageCreateView.test());

            TestMessageEdit testMessageEdit = new TestMessageEdit(message, MessageMode.EDIT);
            results.addAll(testMessageEdit.test());

            TestMessageView testMessageEditView = new TestMessageView(message);
            results.addAll(testMessageEditView.test());
        }
        catch (Exception e) {
            results.add(new TestResult(getTestName(), TestResult.FAIL,"Could not perform this test block.", e));
            Writer.writeScreenshot(getTestName(), driver);
        }

        return results;
    }

    private void navigateToPageMessages() {
        WebElement linkGroup = driver.findElement(By.cssSelector("a[href*='" +  Configuration.getProperty(PROPERTY_NAME_GROUP_PATH) + "']"));
        linkGroup.click();
        driver.manage().window().maximize();
    }
}
