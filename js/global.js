ns = function () {
    function init() {
        console.log("HI!");
    }

    function change() {
        console.log("BYE!");
    }
    return {
        init: init,
        change: change
    }
}();
