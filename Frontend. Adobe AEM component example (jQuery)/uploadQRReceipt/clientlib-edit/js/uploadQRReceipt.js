S.ClassicUINamespace.UploadQRReceipt = (function ($) {

    function onRadioButtonChanged (component, val, panelModificationName) {
        if (panelModificationName) {
        var parentPanel = component.findParentByType('panel');
           var panelModification = parentPanel.getComponent(panelModificationName);
            (val == 'Inline') ? panelModification.show() : panelModification.hide();
        }
    }

    function onSelectionChanged (component, isChecked) {
        var tabPanel = component.findParentByType('tabpanel');
        // QR Scanning tab hide/show
        isChecked ? tabPanel.unhideTabStripItem(2) : tabPanel.hideTabStripItem(2);
    }

    return {
        onRadioButtonChanged: onRadioButtonChanged,
        onSelectionChanged: onSelectionChanged
    }
})(jQuery);
