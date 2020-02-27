/**
 * @author Svetlana Suvorova
 * @lastEdit Svetlana Suvorova
 */

QB.LoginController = (function() {
    "use strict"

    const PARAMETER_ERROR = "?error";
    const CLASS_ERROR = "show-error-alert";

    function initEventListeners() {

        const errorAlert = document.getElementById("error-alert");
        if (document.location.search === PARAMETER_ERROR) {
            errorAlert.classList.toggle(CLASS_ERROR);
        }

         const inputFields = document.querySelectorAll("input");
         [].forEach.call(inputFields, function(field) {
             field.addEventListener("input", function(event) {
                if (errorAlert.classList.contains(CLASS_ERROR)) {
                    errorAlert.classList.remove(CLASS_ERROR);
                }
             });
         });
    }

    function init() {

        initEventListeners();
    }

    return {
        init: init
    };
})();
