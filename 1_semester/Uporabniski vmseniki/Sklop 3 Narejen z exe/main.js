
const { app, BrowserWindow, ipcMain, session, protocol } = require("electron");
const path = require("path");

let mainWindow;
let settingsWindow;


function setupCSP() {
    session.defaultSession.webRequest.onHeadersReceived((details, callback) => {
        callback({
            responseHeaders: {
                ...details.responseHeaders,
                "Content-Security-Policy": [
                    "default-src 'self' app:; " +
                    "script-src 'self' app:; " +
                    "style-src 'self' app:; " +
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
        const filePath = path.join(__dirname, url);
        callback({ path: filePath });
    });
}

// Kreiranje glavnega okna aplikacije
function createMainWindow() {
    mainWindow = new BrowserWindow({
        width: 1000,
        height: 800,
        webPreferences: {
            contextIsolation: true,
            preload: path.join(__dirname, "preload.js"),
            nodeIntegration: false
        }
    });

    mainWindow.loadFile("index.html");

    mainWindow.on("closed", () => {
        mainWindow = null;
    });
}

// od okna do glavnega procesa vsecki
ipcMain.on("vsecek-dogodek", (event, index) => {
    console.log("❤ Všeček prejet za objavo z indeksom:", index);
});

// od okna do glavnega procesa tema
ipcMain.on("change-theme", (event, themeName) => {
    if (mainWindow) {
        mainWindow.webContents.send("theme-changed", themeName);
    }
});

//Odpiranje okna za nastavitve
ipcMain.on("open-settings-window", () => {
    if (settingsWindow) {
        settingsWindow.focus();
        return;
    }

    settingsWindow = new BrowserWindow({
        width: 400,
        height: 300,
        parent: mainWindow, 
        modal: true,        
        webPreferences: {
            contextIsolation: true,
            preload: path.join(__dirname, "preload.js")
        }
    });

    settingsWindow.loadFile("settings.html");
    settingsWindow.on("closed", () => (settingsWindow = null));
});

// zapiranje aplikacije
ipcMain.on("close-app", () => {
    app.quit();
});

// Zagon aplikacije
app.whenReady().then(() => {
    setupCSP();          
    setupCustomProtocol(); 
    createMainWindow();    //

    app.on("activate", () => {
        if (BrowserWindow.getAllWindows().length === 0) createMainWindow();
    });
});

// Zapiranje vseh oken 
app.on("window-all-closed", () => {
    if (process.platform !== "darwin") app.quit();
});