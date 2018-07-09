function fullScrn() {
    //check to see if browser supports fullscreen mode
    var fullscreenEnabled = document.fullscreenEnabled || document.mozFullScreenEnabled || document.webkitFullscreenEnabled;
    //checks to see if anything is in fullscreen mode
    var fullscreenElement = document.fullscreenElement || document.mozFullScreenElement || document.webkitFullscreenElement;

    if (fullscreenEnabled && !fullscreenElement) {
        launchIntoFullscreen(document.documentElement);
    } else {
        exitFullscreen();
    }

    function launchIntoFullscreen(element) {
        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.mozRequestFullScreen) {
            element.mozRequestFullScreen();
        } else if (element.webkitRequestFullscreen) {
            element.webkitRequestFullscreen();
        } else if (element.msRequestFullscreen) {
            element.msRequestFullscreen();
        }

        document.getElementById('fullScreen').style.display = "none";
        document.getElementById('normalScreen').style.display = "block";

    }

    function exitFullscreen() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        }

        document.getElementById('fullScreen').style.display = "block";
        document.getElementById('normalScreen').style.display = "none";
    }
}
