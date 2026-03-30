document.addEventListener('DOMContentLoaded', function() {
    // GUMB ZA TEME
    const themeBtn = document.getElementById('toggle-theme');
    if (themeBtn) {
        themeBtn.addEventListener('click', function() {
            const themeLink = document.getElementById('theme-css');

            if (themeLink.href.includes('light.css')) {
                themeLink.href = 'StyleCss/dark.css';
                this.textContent = 'Svetla tema';
                localStorage.setItem('theme', 'dark');

                // posle
                if (window.electronAPI && window.electronAPI.changeTheme) {
                    window.electronAPI.changeTheme('dark');
                }
            } else {
                themeLink.href = 'StyleCss/light.css';
                this.textContent = 'Temna tema';
                localStorage.setItem('theme', 'light');

                // poslje
                if (window.electronAPI && window.electronAPI.changeTheme) {
                    window.electronAPI.changeTheme('light');
                }
            }
        });
    }

    // GUMB ZA NASTAVITVE
    const settingsBtn = document.getElementById('settings-btn');
    if (settingsBtn) {
        settingsBtn.addEventListener('click', function() {
            if (window.electronAPI && window.electronAPI.openSettingsWindow) {
                window.electronAPI.openSettingsWindow();
            } else {
                window.open('settings.html', 'Nastavitve', 'width=400,height=300');
            }
        });
    }

    // Shranjena tema ob zagonu
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        const themeLink = document.getElementById('theme-css');
        const themeBtn = document.getElementById('toggle-theme');

        if (themeLink) {
            themeLink.href = `StyleCss/${savedTheme}.css`;
        }
        if (themeBtn) {
            themeBtn.textContent = savedTheme === 'light' ? 'Temna tema' : 'Svetla tema';
        }
    }
});