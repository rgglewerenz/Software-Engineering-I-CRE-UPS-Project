var GLOBAL = {};
GLOBAL.DotNetReference = null;
GLOBAL.SetDotnetReference = function (pDotNetReference) {
    GLOBAL.DotNetReference = pDotNetReference;
};

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

function StartWatch(event) {
    try {
        const { remote } = require('electron');
        const win = remote.getCurrentWindow();

        win.on('maximize', onWindowMove);
        win.on('unmaximize', onWindowMove);
        win.on('move', onWindowMove);
        
    } catch {
        return 0;
    }
}


function onWindowMove() {
    GLOBAL.DotNetReference.invokeMethodAsync('OnWindowChange');
}

function closeCurrentWindow() {
    try {
        const { remote } = require('electron');
        var window = remote.getCurrentWindow();
        window.close();
    }
    catch {
        return;
    }
    
}

function getIsFullScreen() {
    try {
        const { remote } = require('electron');
        const window = remote.getCurrentWindow();
        return window.isMaximized();
    }
    catch {
        return false;
    }
}

function minimizeCurrentWindow() {
    try {
        const { remote } = require('electron');
        const window = remote.getCurrentWindow();
        window.minimize();
    }
    catch  {
        return;
    }
}

function maximizeCurrentWindow() {
    try {
        const { remote } = require('electron');
        const window = remote.getCurrentWindow();
        window.maximize();
    }
    catch {
        return;
    }
}

function restoreCurrentWindow() {
    try {
        const { remote } = require('electron');
        const window = remote.getCurrentWindow();
        window.restore();
    }
    catch {
        return;
    }
}