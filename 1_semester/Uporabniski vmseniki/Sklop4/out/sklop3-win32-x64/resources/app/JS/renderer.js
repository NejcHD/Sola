// === renderer.js ===

// Funkcija za generiranje HTML (čista in pregledna)
function ustvariHtmlObjave(objava, index) {
    const datum = new Date(objava.datum).toLocaleDateString();
    return `
        <div class="objava">
            <div class="objava-vsebina-flex">
                <div class="slika-okvir-krog"><img src="${objava.slikaPot}" alt="Avatar"></div>
                <div class="tekst-vsebina">
                    <h3>${objava.naslov}</h3>
                    <p>${objava.vsebina}</p>
                    <small>📅 ${datum} – <b>${objava.avtor}</b></small>
                </div>
                <div class="like-section">
                    <button class="like-btn" data-index="${index}">
                        ❤️ <span id="like-count-${index}">${objava.vsec}</span>
                    </button>
                </div>
            </div>
        </div>`;
}

// Enotna funkcija za izris
function osveziPrikaz(vseSkupaj) {
    window.vseObjave = vseSkupaj;
    const container = document.getElementById("objave-seznam");
    if (container) {
        container.innerHTML = vseSkupaj.map((o, i) => ustvariHtmlObjave(o, i)).join('');
    }
}

// Delegacija klikov (ostane enaka, ker je najbolj učinkovita)
document.addEventListener("click", (e) => {
    const btn = e.target.closest(".like-btn");
    if (!btn) return;
    const index = parseInt(btn.dataset.index);

    window.vseObjave[index].vsec++;
    document.getElementById(`like-count-${index}`).innerText = window.vseObjave[index].vsec;

    window.AppAPI?.sendLike(index);
});