/**
 * @author Svetlana Suvorova
 * @lastEdit Sergei Kharitonov
 */

QB.EventHandler = (function() {
    "use strict"

    let channels = {};

    function subscribe(channel, callback) {
        if (!channels[channel]) {
            channels[channel] = [];
        }
        channels[channel].push({
            context: this,
            callback: callback
        });
        return this;
    }

    function publish(channel) {
        if (!channels[channel]) {
            return false;
        }
        const publishData = Array.prototype.slice.call(arguments, 1);
        for (let i = 0, len = channels[channel].length; i < len; i++) {
            const subscription = channels[channel][i];
            subscription.callback.apply(subscription.context, publishData);
        }
        return this;
    }

    return {
        channels: {
            MODAL_QUERY_FORM_NEW: "modal_query_form_new",
            MODAL_QUERY_FORM_DELETE: "modal_query_form_delete",
            MODAL_QUERY_FORM_SELECT_TABLE: "modal_query_form_select_table"
        },
        publish: publish,
        subscribe: subscribe,
        installTo: function(obj) {
            obj.subscribe = subscribe;
            obj.publish = publish;
        }
    };

}());