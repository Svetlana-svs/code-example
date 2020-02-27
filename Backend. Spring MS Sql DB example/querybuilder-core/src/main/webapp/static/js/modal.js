/**
 * @author Svetlana Suvorova
 * @lastEdit Sergei Kharitonov
 */

QB.ModalController = (function() {
    "use strict"

    const ID_CONTAINER = "modalWindow";

    let container = {};
    let modalTitle = {};
    let modalBody = {};

    let buttonYes = {};
    let buttonNo = {};

    let currentChannel = "";

    function initElements() {

        // Set DOM elements
        container = document.getElementById(ID_CONTAINER);
        if (container === null) {
            console.error("One of the necessary DOM elements not found");
            return;
        }
        modalTitle = container.querySelector("[class=modal-title]");
        modalBody = container.querySelector("[class=modal-body]");

        buttonYes = container.querySelector("[class*=btn-primary]");
        buttonNo = container.querySelector("[class*=btn-outline-primary]");
    }

    function initEventListeners() {
        buttonYes.addEventListener("click", confirmModal.bind(event, true), false);
        buttonNo.addEventListener("click", confirmModal.bind(event, false), false);
    }

    function confirmModal(confirm) {
        const publishData = {
            confirm: confirm,
            body: modalBody
        }

        // Publish about close modal for subscribers
        QB.EventHandler.publish(currentChannel, publishData);

        // Clear modal data
        currentChannel = "";
        modalTitle.innerHTML = "";
        modalBody.innerHTML = "";

        $(container).modal("hide");
    }

    function init() {

        initElements();
        initEventListeners();
    }

    function openModal(titleHtml, bodyHtml, channel) {
		modalTitle.innerHTML = titleHtml || "";
		modalBody.innerHTML = bodyHtml || "";
		currentChannel = channel;

        $(container).modal("show");
    }

    function closeModal() {
        currentChannel = "";
        modalTitle.innerHTML = "";
        modalBody.innerHTML = "";
        $(container).modal("hide");
    }

    return {
        init: init,
        openModal: openModal,
        closeModal: closeModal
    };
})();
