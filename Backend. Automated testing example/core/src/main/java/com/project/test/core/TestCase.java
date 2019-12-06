package project.test.core;

import project.test.core.common.TestResult;

import java.net.MalformedURLException;
import java.util.List;

public interface TestCase {

    List<TestResult> test() throws MalformedURLException;
}
