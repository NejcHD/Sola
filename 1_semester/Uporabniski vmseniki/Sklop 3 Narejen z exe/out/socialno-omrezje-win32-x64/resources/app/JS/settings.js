
document.addEventListener('DOMContentLoaded', () => {
    console.log("Nastavitve so naložene");
    
    const themeOptions = document.querySelectorAll('.theme-option');
    const closeBtn = document.getElementById('close-btn');

    // svetla ali temna
    themeOptions.forEach(option => {
        option.addEventListener('click', function() {
            // Pridobimo izbrano temo 
            const theme = this.dataset.theme;
            console.log("Uporabnik je izbral temo:", theme);
            
            const themeLink = document.getElementById('theme-css');
            if (themeLink) {
                themeLink.href = `StyleCss/style-${theme}.css`;
            }

            
            themeOptions.forEach(opt => opt.classList.remove('active'));
            this.classList.add('active');

            // Pošiljanje izbire v glavni proces
            if (window.AppAPI && window.AppAPI.changeTheme) {
                window.AppAPI.changeTheme(theme);

                // Shranimo izbiro v lokalno shrambo
                localStorage.setItem('theme', theme);
            }
        });
    });

    // Funkcionalnost za zapiranje okna
    if (closeBtn) {
        closeBtn.addEventListener('click', function() {
            window.close();
        });
    }

    //  naložimo nazadnje shranjeno temo
    const currentTheme = localStorage.getItem('theme') || 'light';
    themeOptions.forEach(option => {
        if (option.dataset.theme === currentTheme) {
            option.classList.add('active');
        }
    });
});