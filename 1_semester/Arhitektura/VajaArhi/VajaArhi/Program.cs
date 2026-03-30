using System.IO.Pipes;

namespace Odjemalec
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var odjemalec = new NamedPipeClientStream(".", "streznik", PipeDirection.InOut))
            {
                Console.WriteLine("Povezujem se s strežnikom...");
                odjemalec.Connect();
                Console.WriteLine("Odjemalec!");

                using (StreamReader bralec = new StreamReader(odjemalec))
                using (StreamWriter pisec = new StreamWriter(odjemalec))
                {
                    pisec.AutoFlush = true;
                    
                    string vprasanje1 = bralec.ReadLine();
                    Console.WriteLine(vprasanje1);
                    Console.WriteLine("Odgovor:");
                    string odgovor1 = Console.ReadLine();
                    pisec.WriteLine(odgovor1);

                    string vprasanje2 = bralec.ReadLine();
                    Console.WriteLine(vprasanje2);
                    Console.WriteLine("Odgovor:");
                    string odgovor2 = Console.ReadLine();
                    pisec.WriteLine(odgovor2);

                    string vprasanje3 = bralec.ReadLine();
                    Console.WriteLine(vprasanje3);
                    Console.WriteLine("Odgovor:");
                    string odgovor3 = Console.ReadLine();
                    pisec.WriteLine(odgovor3);


                    Console.WriteLine(bralec.ReadLine());


                }
            }
        }
    }
 }