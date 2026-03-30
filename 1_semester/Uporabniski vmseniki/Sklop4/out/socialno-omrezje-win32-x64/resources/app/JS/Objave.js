class Objava {
    constructor(naslov, vsebina, avtor, vsec) {
        this.naslov = naslov;    
        this.vsebina = vsebina;  
        this.avtor = avtor;      
        this.vsec = vsec;       
        this.datum = new Date();
        
    }

    getInitals(iem) {
        return ime.split(' ')
            .map(part => part[0])
            .join('')
            .toUpperCase()
            .substring(0,2);
    }
}

const vseObjave = [
    {
        naslov: "Hallloo!",
        vsebina: "Želim vsem lep in sončen dan na fakulteti.",
        avtor: "Bojan Novak",
        vsec: 24,
        slikaPot: "DATA/Profil1.jpg", 
        datum: new Date("2024-03-15")
    },
    {
        naslov: "Vikend na planini",
        vsebina: "Prečudovit vikend v gorah. Svež zrak in mir.",
        avtor: "Marko Kovač",
        vsec: 18,
        slikaPot: "DATA/Profil2.jpg", 
        datum: new Date("2024-03-10")
    }
];



console.log("Wpf objave prensene v elektron");

