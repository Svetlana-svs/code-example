package project.test.core.message.service;

import project.test.core.common.Configuration;
import project.test.core.common.TestResult;
import project.test.core.message.Message;
import project.test.core.message.MessageMode;
import org.apache.commons.lang3.RandomStringUtils;
import org.openqa.selenium.*;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

import java.text.SimpleDateFormat;
import java.util.*;

public class MessageService {
    private final static String MESSAGE_EDIT_POSTFIX = Configuration.getProperty("message.edit.text");
    private final static String[] DatePickerFormats = {"dd.MM.yyyy", "HH:mm"};

    private final static String MESSAGE_INPUT_TEXT_CLASS = Configuration.getProperty("message.input.text.class");
    private final static String MESSAGE_INPUT_DATE_CLASS = Configuration.getProperty("message.input.date.class");
    private final static String MESSAGE_INPUT_DATEPICKER_CLASS = Configuration.getProperty("message.input.datepicker.class");
    private final static String MESSAGE_BUTTON_DATEPICKER_CLASS = Configuration.getProperty("message.button.datepicker.class");
    private final static String MESSAGE_INPUT_AUTOCOMPLETE_ATTRIBUTE = Configuration.getProperty("message.input.autocomplete.attribute");
    private final static String MESSAGE_INPUT_AUTOCOMPLETE_ATTRIBUTE_VALUE = Configuration.getProperty("message.input.autocomplete.attribute.value");
    private final static String MESSAGE_LIST_AUTOCOMPLETE_CLASS = Configuration.getProperty("message.list.autocomplete.class");
    private final static String MESSAGE_FRAME_TEXT_ID = Configuration.getProperty("message.frame.text.id");

    public static WebDriver driver = null;
    private static int webDriverResponseTimeout = Integer.parseInt(Configuration.getProperty("webdriver.response.timeout"));

    public static void inputDateField(TestResult result, Message message) {
        WebElement inputDatePicker = driver.findElement(By.className(MESSAGE_INPUT_DATE_CLASS))
                .findElement(By.tagName("input"));
        inputDatePicker.click();

        try {
            WebDriverWait wait = new WebDriverWait(driver, webDriverResponseTimeout);
            By locatorButtonDatePicker = By.cssSelector("button[class*='" + MESSAGE_BUTTON_DATEPICKER_CLASS +"']");
            wait.until(ExpectedConditions.presenceOfElementLocated(locatorButtonDatePicker));

            WebElement datePicker = driver.findElement(By.className("el-date-picker"));
            List<WebElement> datePickerElements =
                    datePicker.findElements(By.className(MESSAGE_INPUT_DATEPICKER_CLASS));
            if (datePickerElements != null && datePickerElements.size() >= 2) {
                List<String> datePickerTextList = new ArrayList<>();
                for (int i = 0; i < 2; i++) {
                    WebElement datePickerElement = datePickerElements.get(i);
                    SimpleDateFormat datePickerElementFormat = new SimpleDateFormat(DatePickerFormats[i]);
                    String datePickerText = datePickerElementFormat.format(new Date());
                    datePickerTextList.add(datePickerText);
                    try {

                        datePickerElement.click();
                        datePickerElement.sendKeys(datePickerText);
                    } catch (ElementNotInteractableException e) {
                    }
                }
                message.setDateTimeFieldList(String.join(" ", datePickerTextList));
            }

            WebElement datePickerButtonOk =  driver.findElement(locatorButtonDatePicker);
            datePickerButtonOk.click();
        }
        catch (TimeoutException e) {
            result.fail("Date picker is not found.");
        }
    }

    public static void inputTextFields(Message message, MessageMode mode) {
        List<WebElement> inputTextList =  driver.findElements(By.className(MESSAGE_INPUT_TEXT_CLASS));
        for (WebElement inputText: inputTextList) {
            try {
                String text = "";
                if (mode == MessageMode.CREATE) {
                    text = RandomStringUtils.randomAlphanumeric( 10);
                } else {
                    text = inputText.getAttribute("value") + MESSAGE_EDIT_POSTFIX;
                    inputText.sendKeys(Keys.CONTROL + "a");
                    inputText.sendKeys(Keys.DELETE);
                }

                message.setTextFieldList(text);
                if (inputText.isEnabled() && inputText.getAttribute("type").equals("text")) {
                    inputText.sendKeys(text);
                }
            }
            catch (ElementNotInteractableException e) {
            }
        }
    }

    public static void inputTextMessage(Message message, MessageMode mode) {
        driver.switchTo().frame(driver.findElement(By.id(MESSAGE_FRAME_TEXT_ID)));
        WebElement messageBodyElement =  driver.findElement(By.tagName("body"));

        String text = "";
        if (mode == MessageMode.CREATE) {
            text = Message.MESSAGE_TEXT_PREFIX + " " + RandomStringUtils.randomAlphanumeric( 20);
        } else {
            text = messageBodyElement.getText() + MESSAGE_EDIT_POSTFIX;
            messageBodyElement.clear();
        }

        message.setBody(text);
        messageBodyElement.sendKeys(text);
        driver.switchTo().defaultContent();
    }

    public static void autocompleteField(TestResult result, Message message, MessageMode mode) {
        String text = "";
        try {
            WebDriverWait wait = new WebDriverWait(driver, webDriverResponseTimeout);
            By locatorAutocompleteLists = By.cssSelector("input[" + MESSAGE_INPUT_AUTOCOMPLETE_ATTRIBUTE +
                    "^='" + MESSAGE_INPUT_AUTOCOMPLETE_ATTRIBUTE_VALUE);
            List<WebElement> inputAutocompleteList =  driver.findElements(locatorAutocompleteLists);
            for (WebElement inputAutocomplete: inputAutocompleteList) {

                String inputAutocompleteId =  inputAutocomplete.getAttribute(MESSAGE_INPUT_AUTOCOMPLETE_ATTRIBUTE);
                List<WebElement> autocompleteLists = driver.findElements(By.cssSelector("ul[class*='" + 
                       MESSAGE_LIST_AUTOCOMPLETE_CLASS + "']"));

                for (WebElement autocompleteList: autocompleteLists) {
                    String autocompleteListId = autocompleteList.getAttribute("id");

                    if (inputAutocompleteId.contains(autocompleteListId)) {
                        if (mode == MessageMode.CREATE) {
                            int lettersCnt = (((new Random()).nextInt(10) % 2) == 0) ? 1 : 10;
                            text = RandomStringUtils.randomAlphabetic(lettersCnt);
                            inputAutocomplete.click();
                        } else {
                            text = inputAutocomplete.getAttribute("value") + MESSAGE_EDIT_POSTFIX;
                            inputAutocomplete.clear();
                        }
                    }
                    try {
                        // Handling Alert project.test.core.message after lost focus of a preview input
                        wait.until(ExpectedConditions.alertIsPresent());
                        Alert alert = driver.switchTo().alert();
                        alert.accept();
                    }
                    catch (TimeoutException | NoAlertPresentException exp) {
                    }
                    try {
                        message.setAutocompleteFieldList(text);
                        inputAutocomplete.sendKeys(text);
                    } catch (ElementNotInteractableException e) {
                    }

                    By locatorAutocompleteListElements = By.cssSelector("li[role='option']");
                    boolean autocompleteListExist = true;
                    try {
                        wait.until(ExpectedConditions.presenceOfNestedElementsLocatedBy(
                                By.id(autocompleteListId), locatorAutocompleteListElements));
                    } catch (TimeoutException e) {
                        autocompleteListExist = false;
                    }

                    if (autocompleteListExist) {
                        List<WebElement> autocompleteListElements = autocompleteList.findElements(locatorAutocompleteListElements);
                        if (autocompleteListElements.size() > 0) {
                            int index = (new Random()).nextInt(autocompleteListElements.size() - 1);
                            WebElement autocompleteListElement = autocompleteListElements.get(index);
                            message.setAutocompleteFieldList(autocompleteListElement.getText());
                            autocompleteListElement.click();
                            // Bug with double click on an autocomplete list
                            try {
                                Thread.sleep(500);
                                if (!inputAutocomplete.getText().equals(autocompleteListElement.getText())) {
                                    autocompleteListElement.click();
                                }
                            }
                            catch (StaleElementReferenceException | ElementNotInteractableException exp) {
                            }
                        }
                    }
                    break;
                }
            }
        }
        catch ( ElementNotInteractableException | InterruptedException e) {
            result.fail("Error by autocomplete field.");
        }
    }
}
