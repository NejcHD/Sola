module.exports = {
    packagerConfig: {},
    rebuildConfig: {},
    makers: [
        {
            name: '@electron-forge/maker-squirrel', 
            config: {
                name: "socialno_omrezje"
            },
        },
        {
            name: '@electron-forge/maker-zip', 
            platforms: ['win32'],
        }
    ],
};