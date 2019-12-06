package ...widget.exception;


/*
 * Thrown by trying to get data from external service when data response is empty.
 */
public class NotFoundException extends RuntimeException {
    public NotFoundException(String msg) {
        super(msg);
    }
}