const { app, BrowserWindow } = require('electron');

let mainWindow;

app.whenReady().then(() => {
    console.log('Elektron je prpravljen');
    
    mainWindow = new BrowserWindow({
        width: 800,
        height: 600,
        show: true,
        webPreferences: {
            nodeIntegration: true,
        }
    });
    
    mainWindow.loadFile('index.html');
    mainWindow.on('ready-to-show'), () => {
        mainWindow.show();
        console.log('Oknow je prikazano');
    };
});

app.on('window-all-closed', () => {
    if(process.platform !== 'darwin') {
        app.quit();
    }
})