UUU.ClassicUINamespace.Registration = (function () {

    function onSelectionChanged (component, isChecked) {
        var parentPanel = component.findParentByType("panel");

        switch (component.itemId) {
        case "verificationRequired":

            var verificationPanel = parentPanel.getComponent("verificationPanel");
            var verificationSuccessCheckbox = verificationPanel.getComponent("isVerificationSuccessEmailRequired");
            var autoLoginCheckbox = verificationPanel.getComponent("autoLoginEnabled");
            if (isChecked) {
                verificationPanel.show();
                autoLoginCheckbox.setValue(true);
            } else {
                verificationPanel.hide();
                verificationSuccessCheckbox.setValue(false);
                autoLoginCheckbox.setValue(false);
            }
            break;
            case "isVerificationSuccessEmailRequired":

                var verificationSuccessEmailTemplatePath = parentPanel.
                        getComponent('verificationSuccessEmailTemplatePath');
                var verificationSuccessEmailBasicUrl = parentPanel.
                        getComponent('verificationSuccessEmailBasicUrl');
                if (isChecked) {
                    verificationSuccessEmailTemplatePath.enable();
                    verificationSuccessEmailBasicUrl.enable();
                } else {
                    verificationSuccessEmailTemplatePath.disable();
                    verificationSuccessEmailBasicUrl.disable();
                }

            break;
            case "pdaAndCommunicationAgreement":
            var communicationAgreementPageLink = parentPanel.getComponent('pdaAndCommunicationAgreementPageLink');
                if (isChecked) {
                    communicationAgreementPageLink.disable();
                } else {
                    communicationAgreementPageLink.enable();
                }
            break;
            case "rulesAgreement":
            var rulesPageLink = parentPanel.getComponent('rulesPageLink');
                if (isCchecked) {
                    rulesPageLink.disable();
                } else {
                    rulesPageLink.enable();
                }
            break;
        }
    }

    return {
        onSelectionChanged: onSelectionChanged
    }
})();
