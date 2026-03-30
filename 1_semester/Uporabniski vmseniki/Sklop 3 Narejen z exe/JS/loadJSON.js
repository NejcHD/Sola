
// Shranimo začetno stanje 
const zacetneObjave = JSON.parse(JSON.stringify(vseObjave));

//  prebere 
function loadJSONFile(file) {
    const reader = new FileReader();

    reader.onload = (e) => {
        try {
            //  Branje iz JSON 
            const nove = JSON.parse(e.target.result).map(item => ({
                naslov: item.naslov || "Brez naslova",
                avtor: item.avtor || "Neznan avtor",
                vsebina: item.vsebina || "",
                datum: item.datum || new Date(),
                vsec: item.vsec || 0,
                slikaPot: item.slikaPot || "DATA/Profil1.jpg"
            }));

            // Združevanje 
            const skupaj = zacetneObjave.concat(nove);

            // Prikaz 
            osveziPrikaz(skupaj);

            //  shranjevanja 
            localStorage.setItem("zadnjeNalozene", JSON.stringify(nove));

        } catch (err) {
            alert("Napaka v JSON datoteki.");
        }
    };

    reader.readAsText(file);
}


document.addEventListener("DOMContentLoaded", () => {

    // Gumb Naloži podatke
    document.getElementById("load-json-btn")?.addEventListener("click", () => {
        
        const input = document.createElement("input");
        input.type = "file";
        input.accept = ".json"; // izbira json 

        input.onchange = e => {
            if (e.target.files[0]) {
                loadJSONFile(e.target.files[0]);
            }
        };

        input.click(); // Odpre sistemsko okno za izbiro datoteke
    });

    //  avtomatskega shranjevanja ob zagonu
    const shranjeno = localStorage.getItem("zadnjeNalozene");

    
    if (shranjeno) {
        osveziPrikaz(zacetneObjave.concat(JSON.parse(shranjeno)));
    } else {
        osveziPrikaz(zacetneObjave);
    }
});