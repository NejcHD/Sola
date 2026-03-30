using System.IO.Pipes;

namespace Streznik
{
    class Program2
    {
        public static void Main(string[] args)
        {
           
            using (var streznik = new NamedPipeServerStream("streznik"))
            {
                Console.WriteLine("Čakam, da se nekdo priključi...");
                streznik.WaitForConnection();
                Console.WriteLine("streznik");

                using (StreamReader bralec = new StreamReader(streznik))
                using (StreamWriter pisec = new StreamWriter(streznik))
                {
                    pisec.AutoFlush = true;

                    
                    pisec.WriteLine("Vnesi prvo stevilko");
                    string st1 = bralec.ReadLine();

                    pisec.WriteLine("Vnesi  operacijo");
                    string op = bralec.ReadLine();

                    pisec.WriteLine("Vnesi drugo stevilko");
                    string st2 = bralec.ReadLine();

                    Console.WriteLine($" Racun je {st1} {op} {st2}"); 

                    double a = double.Parse(st1);
                    double b = double.Parse(st2);
                    double rezultat = 0;
                    switch (op)
                    {
                        case "+":
                            rezultat = a + b;
                            break;
                        case "-":
                            rezultat = a - b;
                            break;
                        case "*":
                            rezultat = a * b;
                            break;
                        case "/":
                            rezultat = a / b;
                            break;

                    }
                    pisec.WriteLine($"Rezultat je = {rezultat}");
                }
            }
        }    
    }
}