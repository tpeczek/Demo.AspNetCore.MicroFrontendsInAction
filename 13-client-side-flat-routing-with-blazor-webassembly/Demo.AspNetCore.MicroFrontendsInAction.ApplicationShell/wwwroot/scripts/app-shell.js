const ApplicationShell = (function () {
    function initializeFooter() {
        const randomString = Math.round(Math.random() * 2e5).toString(16);
        document.querySelector("footer strong").innerHTML = randomString
    };

    return {
        initialize: function () {
            initializeFooter();
        }
    };
})();

ApplicationShell.initialize();