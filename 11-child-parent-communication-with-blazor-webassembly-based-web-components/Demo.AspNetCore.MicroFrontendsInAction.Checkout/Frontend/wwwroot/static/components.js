window.checkout = (function () {
    return {
        dispatchItemAddedEvent: function (checkoutBuyChildElement) {
            checkoutBuyChildElement.parentElement.dispatchEvent(new CustomEvent("checkout:item_added"));
        }
    };
})();