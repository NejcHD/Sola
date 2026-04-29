using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrojnoUcenjeProjekt.Algoritmi
{
    public static class DrevoAlgoritem
    {
        private static double IzracunajGini(List<PodatkovnaTocka> tocka)
        {
            if (tocka.Count == 0)
            {
                return 0;
            }


            // prestej verjetnost za vsako oznako
            var verjetnost = tocka.GroupBy(t => t.Oznaka)
                                  .Select(g => (double)g.Count() / tocka.Count);


            // formula 1- sum(p^2)
            return 1.0 - verjetnost.Sum(p => p * p);
        }


        public static Vozlisce NauciDrevo(List<PodatkovnaTocka> tocke, int globina)
        {
            // podatke katere obdelamo
            var skupine = tocke.GroupBy(t => t.Oznaka).ToList();

           // globina je na kerem nivuju drevesa  smo
            if (skupine.Count == 1 || globina >= 5)
            {
                return new Vozlisce
                {
                    JeList = true,
                    Oznaka = skupine.OrderByDescending(g => g.Count()).First().Key  // oznaka ka se najveckrat pojavi
                };
            }

            double najboljsiGini = 1.0;
            int najboljsiAtribut = -1;
            double najboljsaMeja = 0;

            // skozi vse podatke ter dobimo mejo
            for (int i = 0; i < tocke[0].Atributi.Count; i++)
            {
                //unikatne vrednosti za meje

                var vrednostiAtributa = tocke.Select(t => t.Atributi[i]).Distinct().OrderBy(v => v).ToList(); 

                // za vsako mejo naredimo 2 kupa   
                foreach (var meja in vrednostiAtributa)
                {
                    // levo vse kar je pod
                    var levo = tocke.Where(t => t.Atributi[i] <= meja).ToList();
                    // desno vse kar je nad
                    var desno = tocke.Where(t => t.Atributi[i] > meja).ToList();

                    if (levo.Count == 0 || desno.Count == 0) continue;

                    // oceni  kako dobro so razvrsceni
                    double trenutniGini = (double)levo.Count / tocke.Count * IzracunajGini(levo) +
                                         (double)desno.Count / tocke.Count * IzracunajGini(desno);


                    // zapomni najbolj cist mejo 
                    if (trenutniGini < najboljsiGini)
                    {
                        najboljsiGini = trenutniGini;
                        najboljsiAtribut = i;
                        najboljsaMeja = meja;
                    }
                }
            }

           // ce zmanka vprasanj nadaljujemo
            if (najboljsiAtribut == -1) return new Vozlisce { JeList = true, Oznaka = skupine.First().Key };


            // nad vsak kup znova sprozimo algoritem
            return new Vozlisce
            {
                JeList = false,
                AtributIndeks = najboljsiAtribut,
                Meja = najboljsaMeja,
                Levo = NauciDrevo(tocke.Where(t => t.Atributi[najboljsiAtribut] <= najboljsaMeja).ToList(), globina + 1),
                Desno = NauciDrevo(tocke.Where(t => t.Atributi[najboljsiAtribut] > najboljsaMeja).ToList(), globina + 1)
            };
        }


        public static string Napovej(Vozlisce vozlisce, PodatkovnaTocka tocka)
        {
            // Če smo prišli do konca ni vprasanj vrne vrednost
            if (vozlisce.JeList) return vozlisce.Oznaka;

            // Preveri pogoje in pojdi levo ali desno
            if (tocka.Atributi[vozlisce.AtributIndeks] <= vozlisce.Meja)
                return Napovej(vozlisce.Levo, tocka);
            else
                return Napovej(vozlisce.Desno, tocka);
        }


    }
}
