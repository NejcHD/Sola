using System;
using System.IO;
using System.IO.Pipes;

namespace NalogaPipe
{
    public class StreznikAplikacije
    {
        private const string ImeCevi = "Pogovor";

        public static void Main(string[] args)
        {
            Console.WriteLine("--- STREŽNIK: ZAGON ---");
            Console.WriteLine($"Čakam na povezavo na cevi '{ImeCevi}'...");

            using (var cevStreznik = new NamedPipeServerStream(ImeCevi, PipeDirection.InOut))
            {
                try
                {
                    // Streznik caka, da se odjemalec poveze
                    cevStreznik.WaitForConnection();
                }
                catch (Exception)
                {
                    Console.WriteLine("Napaka pri povezovanju. Zaključujem.");
                    return;
                }

                Console.WriteLine("Odjemalec povezan. Zacetek klepeta.");








                using (StreamWriter pisec = new StreamWriter(cevStreznik))
                using (StreamReader bralec = new StreamReader(cevStreznik))
                {
                    // To je KLJUČNO, da se sporočilo takoj pošlje
                    pisec.AutoFlush = true;
                    string prejetoSporocilo;
                    string odgovor;

                    while (true)
                    {
                        try
                        {
                            // 1. STREŽNIK BERE: Caka na sporocilo od odjemalca
                            Console.WriteLine("\n--- Čakam na sporočilo od Odjemalca... ---");
                            prejetoSporocilo = bralec.ReadLine();

                            // Preverjanje prekinitve s strani odjemalca
                            if (prejetoSporocilo == null || prejetoSporocilo.ToLower() == "exit")
                            {
                                Console.WriteLine("Odjemalec je zaključil pogovor.");
                                break;
                            }

                            Console.WriteLine($"Odjemalec: {prejetoSporocilo}");

                            // 2. STREŽNIK PIŠE: Bere vnos in pošlje odgovor
                            Console.Write("Strežnik (Vaš odgovor): ");
                            odgovor = Console.ReadLine();

                            pisec.WriteLine(odgovor);

                            // Preverjanje prekinitve s strani strežnika
                            if (odgovor.ToLower() == "exit")
                            {
                                Console.WriteLine("Strežnik pošilja 'exit' in zaključuje.");
                                break;
                            }
                        }
                        catch (IOException)
                        {
                            Console.WriteLine("Povezava je bila nepričakovano prekinjena.");
                            break;
                        }
                    }
                }
            }
            Console.WriteLine("Strežnik zaključen.");
        }
    }
}