using System;
using System.IO;
using System.IO.Pipes;

namespace Vaja2
{
    class Odjemalec
    {
        public static void Main(string[] args)
        {
            using (NamedPipeClientStream client = new NamedPipeClientStream(".", "Streznik", PipeDirection.InOut))
            {
                Console.WriteLine("Povezuje na server.");
                client.Connect();
                Console.WriteLine("Povezan na ODJEMALEC.");

                using (StreamReader Reader = new StreamReader(client))
                using (StreamWriter Writer = new StreamWriter(client))
                {
                    Writer.AutoFlush = true;
                    bool nadaljuj = true;

                    while (nadaljuj)
                    {
                        string sporocilo = Reader.ReadLine();
                        if (sporocilo == null) break;

                        Console.WriteLine(sporocilo);

                        // Preveri, ali strežnik čaka vnos
                        if (sporocilo.Contains("Vnesite") ||
                            sporocilo.Contains("vnesite") ||
                            sporocilo.Contains("vpiši") ||
                            sporocilo.Contains("Ali želite") ||
                            sporocilo.Contains("DA/NE"))
                        {
                            Console.Write("> ");
                            string odgovor = Console.ReadLine();
                            Writer.WriteLine(odgovor);

                            if (odgovor?.ToUpper() == "NE")
                                nadaljuj = false;
                        }
                    }

                    Console.WriteLine("Konec.");
                }
            }
        }
    }
}
