/**
 * @author Svetlana Suvorova
 * @lastEdit Svetlana Suvorova
 */

QB.FooterController = (function() {
    "use strict"

    const APP_VERSION = ' 0.0.11'

    function init() {

        // Set current date in footer
        const currentTimeElem = document.getElementById('current-time');
        const today = new Date();
        currentTimeElem.innerText = today.toDateString();

        // Set current version in footer
        const appVersion = document.getElementById('app-version');
        appVersion.innerText = APP_VERSION;
    }

    return {
        init: init
    };
})();
