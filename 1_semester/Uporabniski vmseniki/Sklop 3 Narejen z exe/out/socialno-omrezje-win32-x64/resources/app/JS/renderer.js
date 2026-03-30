// Funkcija za ustvarjanje HTML
function ustvariHtmlObjave(objava, index) {
    // Formatiranje datuma v berljivo obliko
    const datum = new Date(objava.datum).toLocaleDateString();

    return `
        <div class="objava">
            <div class="objava-vsebina-flex">
                <div class="slika-okvir-krog">
                    <img src="${objava.slikaPot}" alt="Avatar">
                </div>
                <div class="tekst-vsebina">
                    <h3>${objava.naslov}</h3>
                    <p>${objava.vsebina}</p>
                    <small>${datum} – <b>${objava.avtor}</b></small>
                </div>
                <div class="like-section">
                    <button class="like-btn" data-index="${index}">
                        ❤<span id="like-count-${index}">${objava.vsec}</span>
                    </button>
                </div>
            </div>
        </div>`;
}


function osveziPrikaz(vseSkupaj) {
    
    window.vseObjave = vseSkupaj;
    const container = document.getElementById("objave-seznam");
    
    if (container) {
        container.innerHTML = vseSkupaj.map((o, i) => ustvariHtmlObjave(o, i)).join('');
    }
}

// IPC - všečkanje
document.addEventListener("click", (e) => {
  
    const btn = e.target.closest(".like-btn");
    if (!btn) return;
    const index = parseInt(btn.dataset.index);
    window.vseObjave[index].vsec++;
    
    const likeDisplay = document.getElementById(`like-count-${index}`);
    if (likeDisplay) {
        likeDisplay.innerText = window.vseObjave[index].vsec;
    }

    // pošljemo dogodek v glavno nit 
    if (window.AppAPI && window.AppAPI.sendLike) {
        window.AppAPI.sendLike(index);
    }
});

// Poslušanje spremembe teme iz glavnega procesa 
if (window.AppAPI && window.AppAPI.onThemeUpdate) {
    window.AppAPI.onThemeUpdate((themeName) => {
        console.log("Prejeta zahteva za spremembo teme: " + themeName);

       
        const themeLink = document.getElementById("theme-link");
        if (themeLink) {
            themeLink.href = themeName === "dark" ? "style-dark.css" : "style-light.css";
        }
    });
}