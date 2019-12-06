ABBOTT.MainNamespace.UserProfileController = (function () {
    "use strict"

    const userDataCache = (function () {
        var cachedData = null;
        return {
            resetCache: function () {
                cachedData = null;
            },
            getData: function () {
                return cachedData;
            },
            setData: function (data) {
                if (data) {
                    cachedData = data;
                }
            },
            containsCachedData: function () {
                return !!(cachedData)
            }
        }
    })();

    return {
        resetCache: function () {
            userDataCache.resetCache();
        },

        updateUserDataCache: function (name, newValue) {
            const oldValue = ABBOTT.MainNamespace.CookieController.getUserInfoCookieValue(name);
            if (oldValue !== newValue) {
                ABBOTT.MainNamespace.CookieController.setUserInfoCookieValue(name, newValue);
                if (userDataCache.containsCachedData()) {
                    userDataCache.resetCache();
                }
            }
        },

        getUserProfile: function (successCallback, failCallback) {
            if (userDataCache.containsCachedData()) {
                const data = {
                    success: true,
                    userData: userDataCache.getData()
                }
                if (typeof successCallback === "function") {
                    successCallback(data);
                }
                return data;
            }

            const brandId = ABBOTT.MainNamespace.Utils.getBrandId();
            axios({
                method: "post",
                url: "/bin/abbott/getUserProfile",
                params: {brandId: brandId}
            })
            .then(function (response) {
                if (response && response.data) {
                    const data = response.data;
                    if (data.success) {
                        userDataCache.setData(data.userData);
                    }
                    if (typeof successCallback === "function") {
                        successCallback(data);
                    }
                }
                else {
                    console.log(response);
                    if (typeof failCallback === "function") {
                        failCallback(response);
                    }
                }
            })
            .catch(function(error) {
                console.log(error);
                if (typeof failCallback === "function") {
                    failCallback(error);
                }
            })
            .finally(function () {/**/});
        }
    }
})();
