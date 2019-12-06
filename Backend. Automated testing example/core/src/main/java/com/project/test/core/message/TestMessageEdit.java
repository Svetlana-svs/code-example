package project.test.core.message;

import project.test.core.TestCase;
import project.test.core.common.Configuration;
import project.test.core.common.TestResult;
import project.test.core.common.Writer;
import project.test.core.impl.TestCaseImpl;
import project.test.core.message.service.MessageService;
import org.openqa.selenium.*;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

import java.util.*;

public class TestMessageEdit extends TestCaseImpl
        implements TestCase {

    private final static String TEST_MESSAGE_EDIT_NAME = "Message";

    private final static String MESSAGE_ERROR = Configuration.getProperty("message.error.class");

    private final static String MESSAGE_NEW_LINK_ATTRIBUTE = Configuration.getProperty("message.new.link.attribute");
    private final static String MESSAGE_NEW_LINK_ATTRIBUTE_VALUE = Configuration.getProperty("message.new.link.attribute.value");

    private final static String MESSAGE_EDIT_LINK_CLASS = Configuration.getProperty("message.edit.link.class");
    private final static String MESSAGE_EDIT_PAGE_ID = Configuration.getProperty("message.edit.page.id");
    private final static String MESSAGE_EDIT_MENU_ITEM_SAVE_CLASS = Configuration.getProperty("message.edit.menu.item.save.class");

    private String parentWindowId;
    private Message message;
    private MessageMode mode;

    TestMessageEdit (Message message, MessageMode mode) {
        testName = TEST_MESSAGE_EDIT_NAME;
        this.message = (message == null) ? new Message() : message;
        this.mode = mode;
        testCase = (mode == MessageMode.CREATE) ? "Create" : "Edit";
    }

    @Override
    public List<TestResult> test() {
        List<TestResult> results = super.test();
        TestResult result = new TestResult(getTestName(), TestResult.OK);

        try {
            switch (mode) {
                case CREATE:
                    navigateToPageMessageCreate(result);
                    break;
                case EDIT:
                    navigateToPageMessageEdit(result);
                    message.clear();
                    break;
            }

            // Fill message input fields
            MessageService.inputDateField(result, message);
            MessageService.inputTextFields(message, mode);
            MessageService.inputTextMessage(message, mode);
            MessageService.autocompleteField(result, message, mode);

            saveMessage();
            // Check error message after save
            checkSaveMessage(result);
        }
        catch (Exception e) {
            results.add(new TestResult(getTestName(), TestResult.FAIL,"Could not perform this test block.", e));
            Writer.writeScreenshot(getTestName(), driver);
        }
        results.add(result);

        return results;
    }

    private void saveMessage() {
        List<WebElement> messagesSettingsList =  driver.findElements(By.className(MESSAGE_EDIT_MENU_ITEM_SAVE_CLASS));
        for (WebElement messagesSettings: messagesSettingsList) {
            if (messagesSettings.isEnabled()) {
                try {
                    WebElement sa = messagesSettings.findElement(By.tagName("a"));
                    sa.click();
                    break;
                }
                catch (NoSuchElementException e) {
                }
            }
        }
    }

    private void navigateToPageMessageCreate(TestResult result) {
        // Navigate to message create page
        By locatorLinkNew = By.cssSelector("a[" + MESSAGE_NEW_LINK_ATTRIBUTE + "*='" + MESSAGE_NEW_LINK_ATTRIBUTE_VALUE + "']");

        navigateToPage(result, locatorLinkNew, null);
    }

    private void navigateToPageMessageEdit(TestResult result) {
        // Navigate to Message Edit page
        WebElement messageView = TestMessageView.getMessageViewElement(result, message);
        By locatorLinkEdit = By.xpath("//li[contains(@class,'" + MESSAGE_EDIT_LINK_CLASS + "')]//a[contains(.,'Edit')]");

        navigateToPage(result, locatorLinkEdit, messageView);
    }

    private void navigateToPage(TestResult result, By locatorLink, WebElement parentElement) {
        try {
            WebDriverWait wait = new WebDriverWait(driver, webDriverResponseTimeout);
            wait.until(ExpectedConditions.presenceOfElementLocated(locatorLink));
        }
        catch (TimeoutException e) {
            result.fail("'Edit' message link is not found.");
        }
        WebElement linkPage = (parentElement == null ? driver : parentElement).findElement(locatorLink);
        linkPage.click();

        Set<String> set = driver.getWindowHandles();
        Iterator<String> it = set.iterator();
        parentWindowId = it.next();
        String childWindowId = it.next();
        driver.switchTo().window(childWindowId);

        try {
            WebDriverWait wait = new WebDriverWait(driver, webDriverResponseTimeout);
            wait.until(ExpectedConditions.presenceOfElementLocated(new By.ById(MESSAGE_EDIT_PAGE_ID)));
            driver.manage().window().maximize();
        }
        catch (TimeoutException e) {
            result.fail("New message edit page is not found.");
        }
    }

    private void checkSaveMessage(TestResult result) {
        By locatorError = By.cssSelector("div[class*='" + MESSAGE_ERROR + "']");
        try {
            WebDriverWait wait = new WebDriverWait(driver, webDriverResponseTimeout);
            wait.until(ExpectedConditions.presenceOfElementLocated(locatorError));
        } catch (TimeoutException e) {
            driver.switchTo().window(parentWindowId);
            return ;
        }

        result.fail("Error by new message save.");
    }
}
