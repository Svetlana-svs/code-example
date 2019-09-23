package ...widget.exception;

/*
 * Thrown by widget configuration fault.
 */
public class BadRequestException extends RuntimeException {
    public BadRequestException(String msg) {
        super(msg);
    }
}