using System;
using System.Collections.Generic;
using System.IO;

namespace Bolnisnica
{
    // OSNOVNI RAZRED - Oseba
    abstract class Oseba
    {
        private string _ime;
        public string Ime
        {
            get { return _ime; }
            set { _ime = value; }
        }

        private string _priimek;
        public string Priimek
        {
            get { return _priimek; }
            set { _priimek = value; }
        }

        private int _starost;
        public int Starost
        {
            get { return _starost; }
            set { _starost = value; }
        }

        // Konstruktor za Osebo
        public Oseba() { }

        public Oseba(string ime, string priimek, int starost)
        {
            Ime = ime;
            Priimek = priimek;
            Starost = starost;
        }

        // Virtualna metoda, ki jo lahko prepišejo podrazredi
        public virtual void IzpisiPodatke()
        {
            Console.WriteLine($"{Ime} {Priimek}, {Starost} let");
        }
    }

    // ZDRAVNIK - podrazred Osebe
    class Zdravnik : Oseba
    {
        // List pacientov
        public List<Pacient> Pacienti { get; set; }

        // Konstruktorji
        public Zdravnik()
        {
            Pacienti = new List<Pacient>();
        }

        public Zdravnik(string ime, string priimek, int starost) : base(ime, priimek, starost)
        {
            Pacienti = new List<Pacient>();
        }

        // Metoda za dodajanje pacientov
        public void DodajPacienta(Pacient pacient)
        {
            Pacienti.Add(pacient);
            pacient.Zdravnik = this;  // Avtomatsko povezovanje
        }

        // Prepisana metoda
        public override void IzpisiPodatke()
        {
            Console.WriteLine($"Zdravnik: {Ime} {Priimek}, {Starost} let, Št. pacientov: {Pacienti.Count}");
        }

        // Staticne metode
        public static Zdravnik NajstarejsiZdravnik(List<Zdravnik> zdravniki)
        {
            Zdravnik najstarejsi = null;
            int maxStarost = 0;

            foreach (Zdravnik z in zdravniki)
            {
                if (z.Starost > maxStarost)
                {
                    maxStarost = z.Starost;
                    najstarejsi = z;
                }
            }
            return najstarejsi;
        }

        public static Zdravnik NajvecPacientov(List<Zdravnik> zdravniki)
        {
            Zdravnik najvec = null;
            int maxPacientov = 0;

            foreach (Zdravnik z in zdravniki)
            {
                if (z.Pacienti.Count > maxPacientov)
                {
                    maxPacientov = z.Pacienti.Count;
                    najvec = z;
                }
            }
            return najvec;
        }
    }

    // PACIENT - podrazred Osebe
    class Pacient : Oseba
    {
        private string _bolezen;
        public string Bolezen
        {
            get { return _bolezen; }
            set { _bolezen = value; }
        }

        // Referenca na zdravnika
        public Zdravnik Zdravnik { get; set; }

        // Konstruktorji
        public Pacient() { }

        public Pacient(string ime, string priimek, int starost, string bolezen)
            : base(ime, priimek, starost)
        {
            Bolezen = bolezen;
        }

        // Prepisana metoda
        public override void IzpisiPodatke()
        {
            string zdravnikInfo = Zdravnik != null ?
                $", Zdravnik: {Zdravnik.Ime} {Zdravnik.Priimek}" : ", Brez zdravnika";
            Console.WriteLine($"Pacient: {Ime} {Priimek}, {Starost} let, Bolezen: {Bolezen}{zdravnikInfo}");
        }

        // Staticne metode
        public static List<Pacient> NajdiBolezen(List<Pacient> vsiPacienti, string bolezen)
        {
            List<Pacient> najdeniPacienti = new List<Pacient>();

            foreach (Pacient p in vsiPacienti)
            {
                if (p.Bolezen.ToLower() == bolezen.ToLower())
                {
                    najdeniPacienti.Add(p);
                }
            }
            return najdeniPacienti;
        }
    }

    // MEDICINSKA SESTRA - podrazred Osebe
    class Medicinska_sestra : Oseba
    {
        private int _delovneIzkusnje;
        public int DelovneIzkusnje
        {
            get { return _delovneIzkusnje; }
            set { _delovneIzkusnje = value; }
        }

        // Konstruktorji
        public Medicinska_sestra() { }

        public Medicinska_sestra(string ime, string priimek, int starost, int delovneIzkusnje)
            : base(ime, priimek, starost)
        {
            DelovneIzkusnje = delovneIzkusnje;
        }

        // Prepisana metoda
        public override void IzpisiPodatke()
        {
            Console.WriteLine($"Medicinska sestra: {Ime} {Priimek}, {Starost} let, Delovne izkušnje: {DelovneIzkusnje} let");
        }
    }

    // CSV razred
    class Csv
    {
        public static void ShraniVCsv(string pot, List<object> osebe)
        {
            using (StreamWriter writer = new StreamWriter(pot))
            {
                writer.WriteLine("Tip,Ime,Priimek,Starost,DodatneInformacije");

                foreach (object o in osebe)
                {
                    if (o is Zdravnik z)
                    {
                        writer.WriteLine($"Zdravnik,{z.Ime},{z.Priimek},{z.Starost},{z.Pacienti.Count}");
                    }
                    else if (o is Pacient p)
                    {
                        writer.WriteLine($"Pacient,{p.Ime},{p.Priimek},{p.Starost},{p.Bolezen}");
                    }
                    else if (o is Medicinska_sestra s)
                    {
                        writer.WriteLine($"Medicinska sestra,{s.Ime},{s.Priimek},{s.Starost},{s.DelovneIzkusnje}");
                    }
                }
            }
        }
    }

    // GLAVNI PROGRAM
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("BOLNIŠNIČNI SISTEM Z DEDOVANJEM");

            // Ustvarjanje oseb
            Zdravnik zdravnik1 = new Zdravnik("Janez", "Popovic", 45);
            Zdravnik zdravnik2 = new Zdravnik("Simon", "Jenko", 38);

            Medicinska_sestra sestra1 = new Medicinska_sestra("Tina", "Maze", 32, 5);
            Medicinska_sestra sestra2 = new Medicinska_sestra("Jozica", "Pregelj", 28, 10);

            Pacient pacient1 = new Pacient("Peter", "Klepec", 28, "Gripa");
            Pacient pacient2 = new Pacient("Maja", "Bevk", 23, "Prehlad");
            Pacient pacient3 = new Pacient("Ivan", "Horvat", 45, "Zlomljena noga");
            Pacient pacient4 = new Pacient("Nina", "Modic", 51, "Diabetes");
            Pacient pacient5 = new Pacient("Luka", "Zupan", 32, "Gripa");

            // Povezovanje (zdaj z avtomatskim povezovanjem)
            zdravnik1.DodajPacienta(pacient1);
            zdravnik1.DodajPacienta(pacient2);
            zdravnik1.DodajPacienta(pacient3);
            zdravnik2.DodajPacienta(pacient4);
            zdravnik2.DodajPacienta(pacient5);

            // Izpis podatkov z uporabo dedovanja
            Console.WriteLine("\n--- VSE OSEBE V BOLNIŠNICI ---");
            List<Oseba> vseOsebe = new List<Oseba>
            {
                zdravnik1, zdravnik2, sestra1, sestra2,
                pacient1, pacient2, pacient3, pacient4, pacient5
            };

            foreach (Oseba oseba in vseOsebe)
            {
                oseba.IzpisiPodatke();  // POLIMORFIZEM - vsak razred ima svojo implementacijo
            }

            // Testiranje staticnih metod
            Console.WriteLine("\n--- STATISTIKE ---");
            List<Pacient> vsiPacienti = new List<Pacient> { pacient1, pacient2, pacient3, pacient4, pacient5 };
            List<Pacient> pacientiZGripo = Pacient.NajdiBolezen(vsiPacienti, "gripa");

            Console.WriteLine("Pacienti z gripo:");
            foreach (Pacient p in pacientiZGripo)
            {
                p.IzpisiPodatke();
            }

            List<Zdravnik> vsiZdravniki = new List<Zdravnik> { zdravnik1, zdravnik2 };
            Zdravnik najstarejsiZdravnik = Zdravnik.NajstarejsiZdravnik(vsiZdravniki);
            Console.WriteLine($"\nNajstarejši zdravnik:");
            najstarejsiZdravnik.IzpisiPodatke();

            Zdravnik zdravnikNajvecPacientov = Zdravnik.NajvecPacientov(vsiZdravniki);
            Console.WriteLine($"Zdravnik z največ pacienti:");
            zdravnikNajvecPacientov.IzpisiPodatke();

            // Shranjevanje v CSV
            List<object> objektiZaShranjevanje = new List<object>();
            objektiZaShranjevanje.AddRange(vsiZdravniki);
            objektiZaShranjevanje.AddRange(vsiPacienti);
            objektiZaShranjevanje.AddRange(new List<Medicinska_sestra> { sestra1, sestra2 });

            Csv.ShraniVCsv("bolnisnica.csv", objektiZaShranjevanje);
            Console.WriteLine("\nPodatki shranjeni v datoteko: bolnisnica.csv");

            Console.ReadLine();
        }
    }
}