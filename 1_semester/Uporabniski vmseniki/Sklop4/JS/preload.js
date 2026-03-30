const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('AppAPI', {
    sendLike: (index) => ipcRenderer.send('vsecek-dogodek', index),
    openSettings: () => ipcRenderer.send('open-settings-window'),
    changeTheme: (theme) => ipcRenderer.send('change-theme', theme),
    onThemeUpdate: (callback) => ipcRenderer.on('theme-changed', (event, theme) => callback(theme))
});

