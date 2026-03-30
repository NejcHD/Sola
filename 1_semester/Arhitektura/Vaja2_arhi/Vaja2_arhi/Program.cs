using System;
using System.IO;
using System.IO.Pipes;

namespace Vaja2
{
    class Streznik
    {
        public static void Main(String[] args)
        {
            Console.WriteLine("Streznik se je zagnal.");

            using (var server = new NamedPipeServerStream("Streznik"))
            {
                Console.WriteLine("Caka na povezavo odjemalca.");
                server.WaitForConnection();
                Console.WriteLine("Povezan na SERVER");

                using (StreamReader Reader = new StreamReader(server))
                using (StreamWriter Writer = new StreamWriter(server))
                {
                    Writer.AutoFlush = true;

                    Writer.WriteLine("Pozdrav iz strežnika!");
                    Writer.WriteLine("Vnesite prvo število kalkulatorju.");

                    bool nadaljuj = true;
                    double trenutniRezultat = 0;
                    bool jePrvaOperacija = true;

                    while (nadaljuj)
                    {
                        double prvoStevilo;

                        if (jePrvaOperacija)
                        {
                            prvoStevilo = PrvoStevilo(Reader, Writer);
                            jePrvaOperacija = false;
                        }
                        else
                        {
                            prvoStevilo = trenutniRezultat;
                        }

                        Writer.WriteLine($"Izbrali ste {prvoStevilo}."); // <-- POPRAVEK (potrditev)

                        string operacija = Operator(Reader, Writer);
                        double drugoStevilo = DrugoStevilo(Reader, Writer, operacija);
                        trenutniRezultat = Izracunaj(operacija, prvoStevilo, drugoStevilo, Writer);

                        nadaljuj = Nadaljevanje(Reader, Writer);
                    }

                    Writer.WriteLine("Hvala za uporabo kalkulatorja!");
                }
            }
        }

        private static double PrvoStevilo(StreamReader r, StreamWriter w)
        {
            while (true)
            {
                string vnos = r.ReadLine();
                if (double.TryParse(vnos, out double stevilo))
                    return stevilo;
                w.WriteLine("Nepravilni vnos ponovno vnesite število!"); // <-- POPRAVEK
            }
        }

        private static string Operator(StreamReader r, StreamWriter w)
        {
            w.WriteLine("Vnesite operator (+, -, /, *)"); // <-- POPRAVEK
            while (true)
            {
                string op = r.ReadLine();
                if (op == "+" || op == "-" || op == "*" || op == "/")
                {
                    w.WriteLine("Operator sprejet, vnesite število"); // <-- POPRAVEK
                    return op;
                }
                w.WriteLine("Nepravilen vnos, vpiši operator (+, -, /, *)");
            }
        }

        private static double DrugoStevilo(StreamReader r, StreamWriter w, string op) // <-- POPRAVEK (dodan op)
        {
            while (true)
            {
                string vnos = r.ReadLine();
                if (double.TryParse(vnos, out double stevilo))
                {
                    if (op == "/" && stevilo == 0) // <-- POPRAVEK (deljenje z 0)
                    {
                        w.WriteLine("Deljenje z 0 ni dovoljeno, vnesite drugo število!");
                        continue;
                    }
                    return stevilo;
                }
                w.WriteLine("Nepravilni vnos ponovno vnesite število!");
            }
        }

        private static double Izracunaj(string op, double stevilo1, double stevilo2, StreamWriter w)
        {
            double rezultat = 0;
            switch (op)
            {
                case "+": rezultat = stevilo1 + stevilo2; break;
                case "-": rezultat = stevilo1 - stevilo2; break;
                case "*": rezultat = stevilo1 * stevilo2; break;
                case "/": rezultat = stevilo1 / stevilo2; break; // <-- POPRAVEK
            }
            w.WriteLine($"Rezultat: {stevilo1}{op}{stevilo2}={rezultat}");
            return rezultat; // <-- POPRAVEK (vrne rezultat)
        }

        private static bool Nadaljevanje(StreamReader r, StreamWriter w)
        {
            w.WriteLine("Ali želite nadaljevati? DA/NE");
            while (true)
            {
                string odgovor = r.ReadLine();
                if (odgovor == null)
                    return false;

                odgovor = odgovor.Trim().ToUpper();

                if (odgovor == "DA") return true;
                if (odgovor == "NE") return false;

                w.WriteLine("Nepravilen vnos. Prosim, vpiši DA ali NE."); // <-- POPRAVEK
            }
        }
    }
}
