using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StrojnoUcenjeProjekt;

namespace StrojnoUcenjeProjekt.Algoritmi
{
    
    public static class K_nn
    {
        
        private static double IzracunajRazdaljo(PodatkovnaTocka t1, PodatkovnaTocka t2)
        {
            double vsotaKvadratov = 0;

            // skozi atribute
            for (int i = 0; i < t1.Atributi.Count; i++)
            {
                // izracunamo razliko obeh tock
                double razlika = t1.Atributi[i] - t2.Atributi[i];

                //Razliko kvadriramo ter pristejemo k vsoti
                vsotaKvadratov += razlika * razlika;
            }

            return Math.Sqrt(vsotaKvadratov);
        }

       
        public static string KlasificirajTocko(PodatkovnaTocka testnaTocka, List<PodatkovnaTocka> ucnaMnozica, int k)
        {
            // list  vsak element ima razdaljo ter besedilo
            List<KeyValuePair<double, string>> razdalja = new List<KeyValuePair<double, string>>();

            foreach (var ucnaTocka in ucnaMnozica)
            {
                double d = IzracunajRazdaljo(testnaTocka, ucnaTocka);  // dolzina neznane tocke z trenutno
                razdalja.Add(new KeyValuePair<double, string>(d, ucnaTocka.Oznaka));  // razdaljo ter ime shrani
            }

            var najblizjiSosedje = razdalja.OrderBy(x => x.Key).Take(k).ToList();  // razvrstimo seznam od najm do najvec uzerme samo prvih k tock

            var zmagovalnaOznaka = najblizjiSosedje
                .GroupBy(x => x.Value)             // zdruzi po oznakah oz imenih
                .OrderByDescending(g => g.Count())  //presteje glasove  oz koliko je na istem kupu
                .First()                            // uzeme najvecji kup
                .Key;                                  //vzame ime najvecjega kupa

            return zmagovalnaOznaka;
        }
    }
}