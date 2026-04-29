using System;
using System.Collections.Generic;

namespace StrojnoUcenjeProjekt
{
  
    public class PodatkovnaTocka
    {
       
        public List<double> Atributi { get; set; } = new List<double>();

        // Dejanski razred 
        public string Oznaka { get; set; } = "";

        
        public string NapovedanaOznaka { get; set; } = "";
    }

   
    public class Vozlisce
    {
        public bool JeList { get; set; }           
        public string Oznaka { get; set; }        // ce je list kateri razred je
        public int AtributIndeks { get; set; }    // Indeks stolpca
        public double Meja { get; set; }          
        public Vozlisce Levo { get; set; }         // Poddrevo za vrednosti <= Meja
        public Vozlisce Desno { get; set; }        // Poddrevo za vrednosti > Meja
    }
}