using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrekvencnaAnaliza
{
    public class AnalizatorSifre
    {
        public Dictionary<char, int> AnalizaFrekvence(string besedilo)
        {
            var freq = new Dictionary<char, int>();
            foreach (char c in besedilo.ToUpper())
            {
                if (char.IsLetter(c))
                {
                    if (!freq.ContainsKey(c)) freq[c] = 0;
                    freq[c]++;
                }
            }
            return freq.OrderByDescending(x => x.Value)
                       .ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<char, double> RelativneFrekvence(string besedilo)
        {
            var abs = AnalizaFrekvence(besedilo);
            int skupaj = abs.Values.Sum();
            if (skupaj == 0) return new Dictionary<char, double>();
            return abs.ToDictionary(x => x.Key, x => Math.Round(100.0 * x.Value / skupaj, 2));
        }

        public string UporabiKljuc(string sifra, Dictionary<char, char> kljuc)
        {
            var sb = new StringBuilder();
            foreach (char c in sifra)
            {
                char v = char.ToUpper(c);
                if (char.IsLetter(v) && kljuc.ContainsKey(v))
                {
                    char z = kljuc[v];
                    sb.Append(char.IsLower(c) ? char.ToLower(z) : z);
                }
                else sb.Append(c);
            }
            return sb.ToString();
        }

        public void ZamenjajCrki(Dictionary<char, char> kljuc, char a, char b)
        {
            if (!kljuc.ContainsKey(a)) kljuc[a] = a;
            if (!kljuc.ContainsKey(b)) kljuc[b] = b;
            char stA = kljuc[a], stB = kljuc[b];
            kljuc[a] = stB; kljuc[b] = stA;
            foreach (char k in kljuc.Keys.ToList())
            {
                if (k == a || k == b) continue;
                if (kljuc[k] == stA) kljuc[k] = stB;
                else if (kljuc[k] == stB) kljuc[k] = stA;
            }
        }

        public void NastaviPreslikavo(Dictionary<char, char> kljuc, char sifr, char jasna)
        {
            sifr = char.ToUpper(sifr); jasna = char.ToUpper(jasna);
            char stara = kljuc.ContainsKey(sifr) ? kljuc[sifr] : sifr;
            foreach (char k in kljuc.Keys.ToList())
            {
                if (k == sifr) continue;
                if (kljuc[k] == jasna) { kljuc[k] = stara; break; }
            }
            kljuc[sifr] = jasna;
        }

        public Dictionary<char, char> UstvariInicialniKljuc(
            Dictionary<char, int> freqS, Dictionary<char, int> freqR)
        {
            var kljuc = new Dictionary<char, char>();
            for (char c = 'A'; c <= 'Z'; c++) kljuc[c] = c;
            var sK = freqS.Keys.ToList(); var rK = freqR.Keys.ToList();
            int min = Math.Min(sK.Count, rK.Count);
            for (int i = 0; i < min; i++) kljuc[sK[i]] = rK[i];
            return kljuc;
        }
    }
}