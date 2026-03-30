document.addEventListener('DOMContentLoaded', () => {
    console.log("Settings loaded");

    const themeOptions = document.querySelectorAll('.theme-option');
    const closeBtn = document.getElementById('close-btn');

    themeOptions.forEach(option => {
        option.addEventListener('click', function() {
            const theme = this.dataset.theme;
            console.log("Uporabnik je izbral:", theme);

            const themeLink = document.getElementById('theme-css');
            if (themeLink) {
                themeLink.href = `../StyleCss/${theme}.css`;
            }

            themeOptions.forEach(opt => opt.classList.remove('active'));
            this.classList.add('active');

            if (window.electronAPI && window.electronAPI.changeTheme) {
                window.electronAPI.changeTheme(theme);
            }
        });
    });

    if (closeBtn) {
        closeBtn.addEventListener('click', function() {
            window.close();
        });
    }

    const currentTheme = localStorage.getItem('theme') || 'light';
    themeOptions.forEach(option => {
        if (option.dataset.theme === currentTheme) {
            option.classList.add('active');
        }
    });
});