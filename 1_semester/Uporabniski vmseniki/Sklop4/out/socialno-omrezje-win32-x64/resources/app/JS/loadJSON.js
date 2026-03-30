const zacetneObjave = JSON.parse(JSON.stringify(vseObjave));

function loadJSONFile(file) {
    const reader = new FileReader();
    reader.onload = (e) => {
        try {
            const nove = JSON.parse(e.target.result).map(item => ({
                naslov: item.naslov || "Brez naslova",
                avtor: item.avtor || "Neznan avtor",
                vsebina: item.vsebina || "",
                datum: item.datum || new Date(),
                vsec: item.vsec || 0,
                slikaPot: item.slikaPot || "DATA/Profil1.jpg"
            }));
            const skupaj = zacetneObjave.concat(nove);
            osveziPrikaz(skupaj);
            localStorage.setItem("zadnjeNalozene", JSON.stringify(nove));
        } catch (err) { alert("Napaka v JSON datoteki."); }
    };
    reader.readAsText(file);
}

document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("load-json-btn")?.addEventListener("click", () => {
        const input = document.createElement("input");
        input.type = "file";
        input.accept = ".json";
        input.onchange = e => e.target.files[0] && loadJSONFile(e.target.files[0]);
        input.click();
    });

    const shranjeno = localStorage.getItem("zadnjeNalozene");
    osveziPrikaz(shranjeno ? zacetneObjave.concat(JSON.parse(shranjeno)) : zacetneObjave);
});