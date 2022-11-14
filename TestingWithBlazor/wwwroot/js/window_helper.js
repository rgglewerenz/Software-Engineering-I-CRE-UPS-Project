function isElectron() {
    try {
        const { remote } = require('electron');
        return true;
    }
    catch {
        return false;
    }
}

var r = document.querySelector(':root');


window.addEventListener('load', (event) => {
    if (!isElectron()) {
       r.style.setProperty('--frame-size', '0px');
    }
});