package com.softwerke.com.core.util;

import java.util.regex.Pattern;


/**
 * Utility class for validate user data handling.
 */
public class ValidationUtil {

    // pattern for the validation latin and cyrillic letters
    public static final String REGEX_NAME_LETTERS = "^[a-zA-Z\\u0430-\\u044F\\u0410-\\u044F\\u0401\\u0451]+$";

    // pattern for the validation characters: // valid characters: letter, space, hyphen, apostrophe
    public static final String REGEX_NAME_CHARACTERS = "^[a-zA-Z\\u0430-\\u044F\\u0410-\\u044F\\u0401\\u0451\\u0020\\u002C-\\u002F\\u0027\\u2019\\u2018\\u05F3\\u02B9\\u02BC\\u0301\\u2032\\uA78C\\u0060]+$";

    // pattern for the validation characters: letter, digital, space, hyphen
    public static final String REGEX_CITY = "^[0-9a-zA-Z\\u0430-\\u044F\\u0410-\\u044F\\u0401\\u0451\\u0020\\u002D]+$";

    // pattern for the validation characters: letter, digital, space, hyphen, dot
    static final String REGEX_CITY_WITH_TYPE_JOIN = "^[0-9a-zA-Z\\u0430-\\u044F\\u0410-\\u044F\\u0401\\u0451\\u0020\\u002D\\u002E]+$";

    // pattern for the validation characters: letter, digital, space, hyphen, dot, comma, slash, parentheses, asterisk
    public static final String REGEX_ADDRESS_LINE = "^[0-9a-zA-Z\\u0430-\\u044F\\u0410-\\u044F\\u0401\\u0451\\u0020\\u0028-\\u002F]*$";

    // pattern for the validation if string contains a digit
    public static final String REGEX_DIGIT= ".*\\d+.*";

    // TODO: create regex for phone check

    // pattern for the validation characters: letter, digital, space, hyphen, dot, comma, slash, parentheses, asterisk
    public static final String REGEX_PASSWORD = "(?=.*\\d)(?=.*[a-z\\u0430-\\u044F\\u0451])(?=.*[A-Z\\u0410-\\u044F\\u0401])(?=.*[!@#\\$%\\^\\*]).{8,20}";

    /**
     * User data input validation.
     * @param input the input field
     * @param regex RegEx for validation
     * @return boolean validation result
     */
    public static boolean validateParameter(final String input, final String regex) {
        if (input == null) {
            // Validate on null separately
            return true;
        }
        Pattern pattern = Pattern.compile(regex);
        return (pattern.matcher(input).matches());
    }
}