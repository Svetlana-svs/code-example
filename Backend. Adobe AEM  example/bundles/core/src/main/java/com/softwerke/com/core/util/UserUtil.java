package com.package.name.core.util;

import org.apache.commons.codec.binary.Base64;
import org.apache.commons.lang3.StringUtils;
import org.passay.CharacterData;
import org.passay.CharacterRule;
import org.passay.EnglishCharacterData;
import org.passay.PasswordGenerator;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import sun.security.provider.SecureRandom;

import java.security.NoSuchAlgorithmException;
import java.security.spec.InvalidKeySpecException;
import java.util.Random;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;


/**
 * Utility class for user data handling.
 */
public class UserUtil {
    private static final Logger logger = LoggerFactory.getLogger(UserUtil.class);

    public static String SECRET_FORMAT = "%s$%s$%s";
    public static String SECRET_DATE_FORMAT = "yyyy-MM-dd HH:mm";

    private static final String SECRET_KEY_ALGORITHM = "PBKDF2WithHmacSHA1";
    private static final int SECRET_KEY_ITERATIONS = 32000;
    private static final int SECRET_KEY_LENGTH = 20;

    /*
     * Generate password with length 8 - 20 characters
     * English or cyrillic letters using by random.
     */
    public static String getPassword() {
        PasswordGenerator passwordGenerator = new PasswordGenerator();

        Random rand = new Random();
        int language = rand.nextInt(2);

        CharacterData lowerCaseChars = (language == 0) ? EnglishCharacterData.LowerCase : CyrillicCharacterData.LowerCase;
        CharacterData upperCaseChars = (language == 0) ? EnglishCharacterData.UpperCase : CyrillicCharacterData.UpperCase;
        CharacterData digitChars = EnglishCharacterData.Digit;
        CharacterData specialChars = SpecialCharacterData.Special;

        CharacterRule lowerCaseRule = new CharacterRule(lowerCaseChars);

        CharacterRule upperCaseRule = new CharacterRule(upperCaseChars);
        upperCaseRule.setNumberOfCharacters(3);

        CharacterRule digitRule = new CharacterRule(digitChars);
        digitRule.setNumberOfCharacters(2);

        CharacterRule specialCharRule = new CharacterRule(specialChars);
        specialCharRule.setNumberOfCharacters(2);

        int length = 8 + rand.nextInt(13);
        lowerCaseRule.setNumberOfCharacters(length - 7);
        String password = passwordGenerator.generatePassword(length, specialCharRule, lowerCaseRule,
                upperCaseRule, digitRule);

        return password;
    }

    /*
     * Computes a salted PBKDF2 hash of given plaintext password suitable for storing in a database.
     * Empty passwords are not supported.
     * $ character is using as separator between password hash and random salt.
     */
    public static String getSecretHash(String secret) {
        String[] secretData = getSecret(secret);

        // Store the secret key with the secret
        return secretData[0] + "$" + secretData[1];
    }

    public static String[] getSecret(String secret) {
        Random rand = new Random();
        final int saltLength = 64 + rand.nextInt(192);
        SecureRandom random = new SecureRandom();

        byte[] salt = new byte[saltLength];
        random.engineNextBytes(salt);

        // Store the salt with the password
        return new String[]{Base64.encodeBase64String(salt), hash(secret, salt)};
    }

    /*
     * Checks whether given plaintext password corresponds to a stored salted hash of the password.
     */
    public static boolean checkSecret(String secret, String secretHash)
            throws IllegalStateException {
        String[] passwordHash = secretHash.split("\\$");
        if (passwordHash.length != 2) {
            throw new IllegalStateException("Error check user password: illegal the stored hash configuration.");
        }
        byte[] salt = Base64.decodeBase64(passwordHash[0]);
        String hashOfInput = hash(secret, salt);

        return (hashOfInput != null) && hashOfInput.equals(passwordHash[1]);
    }

    /*
     * Generate password hash by using salt.
     */
    private static String hash(String secret, byte[] salt)
            throws IllegalArgumentException {
        SecretKey secretKey = null;
        if (StringUtils.isEmpty(secret)) {
            throw new IllegalArgumentException("Error user secret key generation: empty secret.");
        }

        try {
            SecretKeyFactory secretKeyFactory = SecretKeyFactory.getInstance(SECRET_KEY_ALGORITHM);

            secretKey = secretKeyFactory.generateSecret(new PBEKeySpec(
                    secret.toCharArray(), salt, SECRET_KEY_ITERATIONS, SECRET_KEY_LENGTH));
        } catch (InvalidKeySpecException e) {
            logger.error("Error user secret key generation: error by key generation. ", e);
            return null;
        } catch (NoSuchAlgorithmException e) {
            logger.error("Error user secret key generation: algorithm is not available. ", e);
            return null;
        }

        return Base64.encodeBase64String(secretKey.getEncoded());
    }

    private enum SpecialCharacterData implements CharacterData {
        Special("INSUFFICIENT_SPECIAL", "!@#%^*");

        private final String errorCode;
        private final String characters;

        private SpecialCharacterData(String code, String charString) {
            this.errorCode = code;
            this.characters = charString;
        }

        @Override
        public String getErrorCode() {
            return this.errorCode;
        }

        @Override
        public String getCharacters() {
            return this.characters;
        }
    }

    private enum CyrillicCharacterData implements CharacterData {
        LowerCase("INSUFFICIENT_LOWERCASE", "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"),
        UpperCase("INSUFFICIENT_UPPERCASE", "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ");

        private final String errorCode;
        private final String characters;

        private CyrillicCharacterData(String code, String charString) {
            this.errorCode = code;
            this.characters = charString;
        }

        @Override
        public String getErrorCode() {
            return this.errorCode;
        }

        @Override
        public String getCharacters() {
            return this.characters;
        }
    }
}