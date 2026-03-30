function ustvariHtmlObjave(objava, index) {
    const datum = new Date(objava.datum).toLocaleDateString();
    return `
        <div class="objava" data-index="${index}">
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

    // DODANO: Po prikazu nastavi izbiro objav
    setTimeout(setupPostSelection, 100);
}

// DODANO: Nastavi klik na objave za govorni vmesnik
function setupPostSelection() {
    const objave = document.querySelectorAll('.objava');

    objave.forEach((objavaDiv) => {
        // Dodaj event listener na celotno objavo
        objavaDiv.addEventListener('click', function(event) {
            // Prepreči da bi like gumb prekinil
            if (event.target.closest('.like-btn')) return;

            const index = parseInt(this.getAttribute('data-index'));
            console.log("Izbrana objava:", index);

            // Odstrani izbiro od vseh drugih
            document.querySelectorAll('.objava').forEach(o => {
                o.classList.remove('selected');
            });

            // Označi to objavo
            this.classList.add('selected');

            // Posodobi govorni vmesnik
            if (window.speechManager) {
                window.speechManager.selectedPostIndex = index;
                window.speechManager.state = 'postSelected';
                window.speechManager.currentCommands = window.speechManager.postCommands;
                window.speechManager.updateCommandDisplay();

                const post = window.vseObjave[index];
                window.speechManager.speak("Izbrana objava: " + post.naslov);
            }
        });
    });

    // Dodaj CSS za izbrane objave
    if (!document.querySelector('#post-selection-style')) {
        const style = document.createElement('style');
        style.id = 'post-selection-style';
        style.textContent = `
            .objava.selected {
                border: 2px solid #3498db !important;
                background-color: rgba(52, 152, 219, 0.05) !important;
                box-shadow: 0 4px 12px rgba(52, 152, 219, 0.3);
            }
            
            .objava {
                cursor: pointer;
                transition: all 0.2s;
            }
            
            .objava:hover {
                transform: translateY(-2px);
                box-shadow: 0 4px 8px rgba(0,0,0,0.15);
            }
            
            /* Prepreči da bi like gumb spremenil cursor */
            .like-btn {
                cursor: pointer !important;
            }
            
            .objava:hover .like-btn {
                cursor: pointer !important;
            }
        `;
        document.head.appendChild(style);
    }
}

document.addEventListener("click", (e) => {
    const btn = e.target.closest(".like-btn");
    if (!btn) return;
    const index = parseInt(btn.dataset.index);

    window.vseObjave[index].vsec++;
    document.getElementById(`like-count-${index}`).innerText = window.vseObjave[index].vsec;

    window.AppAPI?.sendLike(index);
});

// Inicializacija
document.addEventListener('DOMContentLoaded', function() {
    // Če že imamo objave, nastavi izbiro
    if (window.vseObjave && window.vseObjave.length > 0) {
        setTimeout(setupPostSelection, 300);
    }
});