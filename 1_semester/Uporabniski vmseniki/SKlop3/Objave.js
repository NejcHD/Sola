class Objava {
    constructor(naslov, vsebina, avtor, vsec) {
        this.naslov = naslov;    
        this.vsebina = vsebina;  
        this.avtor = avtor;      
        this.vsec = vsec;       
        this.datum = new Date();
        this.slika = slika;
        this.avtorInitals = this.getInitals(avtor);
    }

    getInitals(iem) {
        return ime.split(' ')
            .map(part => part[0])
            .join('')
            .toUpperCase()
            .substring(0,2);
    }
}

const VseObjave = [
    new Objava("Slika pod knjiznico","sem sel se uciti v knjiznico za kolokvij", "Janez Novak",3,"Images/slika2.jpg")
];


consoel.log("Wpf objave prensene v elektron");