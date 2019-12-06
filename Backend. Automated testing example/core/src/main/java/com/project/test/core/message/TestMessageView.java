package project.test.core.message;

import project.test.core.TestCase;
import project.test.core.common.Configuration;
import project.test.core.common.TestResult;
import project.test.core.common.Writer;
import project.test.core.annotation.InputIdentificator;
import project.test.core.annotation.InputType;
import project.test.core.impl.TestCaseImpl;
import org.openqa.selenium.*;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

import java.beans.IntrospectionException;
import java.beans.PropertyDescriptor;
import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.net.URL;
import java.util.*;

public class TestMessageView extends TestCaseImpl
        implements TestCase {

    private final static String TEST_MESSAGE_VIEW_NAME = "Message View";

    private final static String HOST = Configuration.getProperty("host");
    private final static String MESSAGE_VIEW_PATH = Configuration.getProperty("message.view.path");

    private final static String MESSAGE_VIEW_CONTAINER_ID = Configuration.getProperty("message.view.container.id");
    private final static String MESSAGE_VIEW_MESSAGE_CLASS = Configuration.getProperty("message.view.message.class");
    private final static String MESSAGE_VIEW_MESSAGE_BODY_CLASS = Configuration.getProperty("message.view.message.body.class");

    private Message message;

    TestMessageView(Message message) {
        testName = TEST_MESSAGE_VIEW_NAME;
        this.message = (message == null) ? new Message() : message;
    }

    public static WebElement getMessageViewElement(TestResult result, Message message) {
        WebElement messageView = null;
        By locatorMessages = By.id(MESSAGE_VIEW_CONTAINER_ID);
        try {
            WebDriverWait wait = new WebDriverWait(driver, webDriverResponseTimeout);
            wait.until(ExpectedConditions.presenceOfElementLocated(locatorMessages));
        }
        catch (TimeoutException e) {
            result.fail("Message view page is not found.");
            return  messageView;
        }

        try {
            By locatorMessage = By.xpath("//div[@id='" + MESSAGE_VIEW_CONTAINER_ID + "']//p[contains(@class,'" +
                    MESSAGE_VIEW_MESSAGE_BODY_CLASS + "') and contains(.,'" + message.getBody() + "')]//ancestor::div[contains(@class,'" +
                    MESSAGE_VIEW_MESSAGE_CLASS + "')]");
            try {
                WebDriverWait wait = new WebDriverWait(driver, webDriverResponseTimeout);
                wait.until(ExpectedConditions.presenceOfElementLocated(locatorMessage));
            }
            catch (TimeoutException e) {
                result.fail("No message is found.");
                return  messageView;
            }

            messageView = driver.findElement(locatorMessage);
            if (messageView == null) {
                result.fail(String.format("No messages with body '%s' is found.", message.getBody()));
                return null;
            }
        } catch (NoSuchElementException e) {
            result.fail("No message is found. " + e);
            return  messageView;
        }
        return  messageView;
    }

    @Override
    public List<TestResult> test() {
        List<TestResult> results = super.test();

        try {
            URL messageViewPageUri = new URL(HOST + "/" +  MESSAGE_VIEW_PATH);
            driver.navigate().to(messageViewPageUri);

            results.add(testMessage());
            results.add(testMessageElement());
        }
            catch (Exception e) {
            results.add(new TestResult(getTestName(), TestResult.FAIL,"Could not perform this test block.", e));
            Writer.writeScreenshot(getTestName(), driver);
        }

        return results;
    }

    private TestResult testMessage() {
        testCase = "Created message fields validate";
        TestResult result = new TestResult(getTestName(), TestResult.OK);
        if (!messageValidated()) {
            result.fail("Some required field of the new message is not set.");
        }

        return result;
    }

    private TestResult testMessageElement() {
        testCase = "Displaying message fields validate";
        TestResult result = new TestResult(getTestName(), TestResult.OK);

        WebElement messageView = getMessageViewElement(result, message);
        if (messageView == null) {
            return result;
        }
        Field[] messageFields = message.getClass().getDeclaredFields();
        Field[] messageViewFields = MessageView.class.getDeclaredFields();

        for (Field messageViewField: messageViewFields) {
            InputType messageViewFieldInputType = messageViewField.getAnnotation(InputType.class);
            InputIdentificator fieldViewId = messageViewField.getAnnotation(InputIdentificator.class);
            if ((messageViewFieldInputType == null) || (fieldViewId == null)) {
                continue;
            }
            WebElement messageViewElement = messageView.findElement(getLocator(fieldViewId));

            for (Field messageField : messageFields) {
                InputType messageFieldInputType = messageField.getAnnotation(InputType.class);
                // If fields of the message and view message is compatibility by input type
                if ((messageFieldInputType != null) && messageViewFieldInputType.type().equals(messageFieldInputType.type())) {
                    String messageFieldValue = getMessageFieldValue(result, messageField);
                    String messageViewFieldValue = getMessageViewFieldValue(messageViewElement);
                    if (!messageFieldValue.contains(messageViewFieldValue)) {
                        result.fail(String.format("'%s' field of the new message is failed.", messageViewField.getName()));
                    }
                    break;
                }
            }
        }

        return result;
    }

    private String getMessageFieldValue(TestResult result, Field messageField) {
        Object messageFieldValue;
        try {
            Method methodGetField = (new PropertyDescriptor(messageField.getName(), Message.class)).getReadMethod();
            messageFieldValue = methodGetField.invoke(message);
            if (messageFieldValue == null) {
                result.fail(String.format("'%s' fields list of the new message is empty.", messageField.getName()));
                return "";
            }
        } catch (IntrospectionException | IllegalAccessException | IllegalArgumentException | InvocationTargetException  e) {
            result.fail(e.getMessage());
            return "";
        }
        return messageFieldValue.toString();
    }

    private String getMessageViewFieldValue(WebElement messageViewElement) {
        String messageFieldValue = "";
        String tagName = messageViewElement.getTagName();
        if (tagName.equals("a")) {
            String attributeValue = messageViewElement.getAttribute("href");
            int messageValuePos = attributeValue.lastIndexOf("/") + 1;
            if ((messageValuePos != 0) && (messageValuePos < attributeValue.length())) {
                return attributeValue.substring(messageValuePos);
            }
            return "";
        } else {
            return messageViewElement.getText();
        }
    }

    private By getLocator(InputIdentificator identificator) {
        By locator = null;
        if (identificator == null) {
            return locator;
        }

        String name = Configuration.getProperty(identificator.value());
        switch (identificator.type()) {
            case "class" :
                locator = new By.ByClassName(name);
                break;
            case "id" :
                locator = new By.ById(name);
                break;
        }

        return  locator;
    }

    private boolean messageValidated() {
        // Validate message required fields
        return (message.getTextFieldList() != null && message.getTextFieldList().size() >= 1 &&
                message.getAutocompleteFieldList() != null && message.getAutocompleteFieldList().size() >= 1 &&
                message.getDateTimeFieldList() != null && message.getDateTimeFieldList().size() >= 1 &&
                message.getBody() != null && !message.getBody().isEmpty());
    }
}
