const { app, BrowserWindow, ipcMain, session, protocol } = require("electron");
const path = require("path");

let mainWindow;
let settingsWindow;

/**
 * VAJA 7: Nastavitev Content Security Policy (CSP)
 * Preprečuje nalaganje nevarnih zunanjih skript in virov.
 */
function setupCSP() {
    session.defaultSession.webRequest.onHeadersReceived((details, callback) => {
        callback({
            responseHeaders: {
                ...details.responseHeaders,
                "Content-Security-Policy": [
                    "default-src 'self' app:; " +
                    "script-src 'self' app:; " +
                    "style-src 'self' app: 'unsafe-inline'; " +
                    "img-src 'self' app: data:; " +
                    "font-src 'self' app:; " +
                    "connect-src 'self' app:"
                ]
            }
        });
    });
}


function setupCustomProtocol() {
    protocol.registerFileProtocol("app", (request, callback) => {
        const url = request.url.replace("app://", "");
        // Pretvori URL v dejansko pot na disku
        const filePath = path.join(__dirname, url);
        callback({ path: filePath });
    });
}


function createMainWindow() {
    mainWindow = new BrowserWindow({
        width: 1000,
        height: 800,
        webPreferences: {
            contextIsolation: true, // VAJA 6: Varnostna izolacija
            preload: path.join(__dirname, "preload.js"),
            nodeIntegration: false
        }
    });

    mainWindow.loadFile("index.html");

    mainWindow.on("closed", () => {
        mainWindow = null;
    });
}

//vsecki
ipcMain.on("vsecek-dogodek", (event, index) => {
    console.log("❤️ Všeček prejet v Main procesu za objavo z indeksom:", index);
});

//tema
ipcMain.on("change-theme", (event, themeName) => {
    if (mainWindow) {
        mainWindow.webContents.send("theme-changed", themeName);
    }
});


ipcMain.on("open-settings-window", () => {
    if (settingsWindow) {
        settingsWindow.focus();
        return;
    }

    settingsWindow = new BrowserWindow({
        width: 400,
        height: 300,
        parent: mainWindow,
        modal: true, // Okno je modalno (blokira starša)
        webPreferences: {
            contextIsolation: true,
            preload: path.join(__dirname, "preload.js")
        }
    });

    settingsWindow.loadFile("settings.html");
    settingsWindow.on("closed", () => (settingsWindow = null));
});

//  zapiranje aplikacije
ipcMain.on("close-app", () => {
    app.quit();
});



app.whenReady().then(() => {
    setupCSP();
    setupCustomProtocol();
    createMainWindow();

    app.on("activate", () => {
        if (BrowserWindow.getAllWindows().length === 0) createMainWindow();
    });
});

app.on("window-all-closed", () => {
    if (process.platform !== "darwin") app.quit();
});