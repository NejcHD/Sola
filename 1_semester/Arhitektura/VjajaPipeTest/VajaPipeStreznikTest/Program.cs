using System;
using System.IO;
using System.IO.Pipes;

namespace NalogaPipe
{
    public class OdjemalecAplikacije
    {
        private const string ImeCevi = "Pogovor";

        public static void Main(string[] args)
        {
            Console.WriteLine("--- ODJEMALEC: ZAGON ---");

            using (var cevOdjemalec = new NamedPipeClientStream(".", ImeCevi, PipeDirection.InOut))
            {
                try
                {
                    Console.WriteLine($"Povezujem se na cev '{ImeCevi}'...");
                    // Povezava caka 5 sekund
                    cevOdjemalec.Connect(5000);
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Napaka: Strežnik ni bil najden. Prepričajte se, da strežnik teče.");
                    return;
                }

                Console.WriteLine("Povezava vzpostavljena. Začetek klepeta.");










                using (StreamWriter pisec = new StreamWriter(cevOdjemalec))
                using (StreamReader bralec = new StreamReader(cevOdjemalec))
                {
                    // To je KLJUČNO, da se sporočila takoj pošljejo
                    pisec.AutoFlush = true;
                    string VnosOdjemalca;
                    string odgovorStreznika;

                    while (true)
                    {
                        // 1. ODJEMALEC PIŠE: Bere vnos uporabnika in ga pošlje
                        Console.Write("\nOdjemalec (Vaš vnos): ");
                        VnosOdjemalca = Console.ReadLine();

                        pisec.WriteLine(VnosOdjemalca);

                        if (VnosOdjemalca.ToLower() == "exit")
                        {
                            Console.WriteLine("Odjemalec pošilja 'exit' in zaključuje.");
                            break;
                        }

                        // 2. ODJEMALEC BERE: Caka na odgovor strežnika
                        Console.WriteLine("--- Čakam na odgovor Strežnika... ---");
                        odgovorStreznika = bralec.ReadLine();

                        // Preverjanje prekinitve s strani strežnika
                        if (odgovorStreznika == null || odgovorStreznika.ToLower() == "exit")
                        {
                            Console.WriteLine("Strežnik je zaključil pogovor.");
                            break;
                        }

                        Console.WriteLine($"Strežnik: {odgovorStreznika}");
                    }
                }
            }
            Console.WriteLine("Odjemalec zaključen.");
        }
    }
}