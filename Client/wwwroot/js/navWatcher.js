//// navWatcher.js
//window.loadScript = function (scriptPath) {
//    return new Promise((resolve, reject) => {
//        var script = document.createElement('script');
//        script.src = scriptPath;
//        script.type = 'text/javascript';
//        script.onload = resolve;
//        script.onerror = reject;
//        document.head.appendChild(script);
//    });
//};

//window.navigationHandler = {
//    addBeforeUnloadListener: function (dotNetObjectRef) {
//        window.onbeforeunload = function (e) {
//            e.preventDefault();
//            e.returnValue = '';
//            dotNetObjectRef.invokeMethodAsync("HandleBeforeUnload");
//            return '';
//        };
//    },
//    removeBeforeUnloadListener: function () {
//        window.onbeforeunload = null;
//    },
//    preventNavigation: function () {
//        window.history.pushState(null, '', window.location.href);
//    }
//};

window.loadScript = function (scriptPath) {
    return new Promise((resolve, reject) => {
        var script = document.createElement('script');
        script.src = scriptPath;
        script.type = 'text/javascript';
        script.onload = resolve;
        script.onerror = reject;
        document.head.appendChild(script);
    });
};

window.navigationHandler = {
    addBeforeUnloadListener: function () {
        return;
        window.onbeforeunload = function (e) {
            e.preventDefault();
            e.returnValue = 'mmmm'; // Standard way to trigger a native prompt
            return ''; // For older browsers
        };
    },
    removeBeforeUnloadListener: function () {
        window.onbeforeunload = null;
    }
};
