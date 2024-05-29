// wwwroot/js/LocalStorageHelper.js
window.LocalStorageHelper = {
    saveState: function (key, data) {
        localStorage.setItem(key, JSON.stringify(data));
    },
    loadState: function (key) {
        return JSON.parse(localStorage.getItem(key));
    },
    removeState: function (key) {
        localStorage.removeItem(key);
    }
};
