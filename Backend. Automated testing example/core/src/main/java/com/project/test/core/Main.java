package project.test.core;

import project.test.core.common.Configuration;
import project.test.core.common.TestResult;
import project.test.core.common.Writer;
import project.test.core.message.service.MessageService;
import project.test.core.user.TestLogin;
import project.test.core.impl.TestCaseImpl;
import project.test.core.message.TestMessage;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.chrome.ChromeOptions;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.List;

public class Main {

    private static final Logger logger = LoggerFactory.getLogger(Main.class);

    private static final String WEBDRIVER_NAME_CHROME = "chromedriver.exe";
    private static final String WEBDRIVER_NAME_FIREFOX = "geckodriver.exe";
    private final static String WEBDRIVER_FOLDER = "resource";

    private enum WebBrowser {
        chrome,
        firefox
    }

    private static WebDriver getWebDriver(String nameWebBrowser) {
        WebBrowser webBrowser = WebBrowser.valueOf(nameWebBrowser);
        String projectPath = System.getProperty("user.dir");
        switch (webBrowser) {
            case chrome:
//            System.setProperty("webdriver.chrome.verboseLogging", "true");
                System.setProperty("webdriver.chrome.driver", projectPath + "/resource/" + WEBDRIVER_NAME_CHROME);
                ChromeOptions chromeOptions = new ChromeOptions();
                return new ChromeDriver(chromeOptions);
           case firefox:
                System.setProperty("webdriver.gecko.driver", projectPath + "/resource/" + WEBDRIVER_NAME_FIREFOX);
                return new FirefoxDriver();
        }

        return null;
    }

    public static void main(String[] args) {
        int sysresult = 0;
        WebDriver driver = null;

        try {
            Configuration config = new Configuration();
            config.setConfiguration();
        } catch (Exception e) {
            logger.error("Configuration file is not found.", e);
        }
        Writer.createFolder("reports");
        List<TestResult> results = new ArrayList<TestResult>();
        try {
            for (WebBrowser browser: WebBrowser.values()) {
                driver = getWebDriver(browser.name());
                try {
                    results.add(new TestResult(browser.name()));

                    // Test user authentication
                    TestCaseImpl.driver = driver;
                    TestLogin testLogin = new TestLogin();
                    results.addAll(testLogin.test());

                    // Test creation, view, edit Message
                    MessageService.driver = driver;
                    TestMessage testMessage = new TestMessage();
                    results.addAll(testMessage.test());

                } catch (Exception e) {
                    sysresult = 1;
                    logger.error("There was a FATAL ERROR while executing the program! Stack trace follows.", e);
                } finally {
                    if (driver != null) {
                        driver.quit();
                    }
                }
            }
        } catch (Exception e) {
            logger.error("Error by driver close.", e);
        }

        Writer.writeCSV(results);
        System.exit(sysresult);
    }
}