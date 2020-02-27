/**
 * @author Svetlana Suvorova
 * @lastEdit Svetlana Suvorova
 */

// Application namespace initialize

//const QB = {};
//const CONTEXT_PATH = "";

const QB = (function() {
    "use strict"

    let contextPath = "";

	function getContextPath() {
		return contextPath;
	}

	function init(settings) {
	    if (typeof settings !== "undefined") {
		    contextPath = settings.contextPath;
	    }
	}

	return {
		init: init,

        // Namespace settings
		getContextPath: getContextPath
	};
})();



