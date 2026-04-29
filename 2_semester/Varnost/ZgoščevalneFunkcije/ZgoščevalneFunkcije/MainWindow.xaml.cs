using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Org.BouncyCastle.Crypto.Digests;
using Konscious.Security.Cryptography;

namespace ZgoščevalneFunkcije
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {        
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Vse datoteke (*.*)|*.*|Dokumenti (*.pdf;*.docx)|*.pdf;*.docx";

            if (openFileDialog.ShowDialog() == true)
            {
                // Shranimo pot do datoteke v TextBox, ki smo ga poimenovali v XAML
                TxtFilePath.Text = openFileDialog.FileName;
            }
        }
        private void BtnBrowse2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Vse datoteke (*.*)|*.*|Dokumenti (*.pdf;*.docx)|*.pdf;*.docx";
            if (openFileDialog.ShowDialog() == true)
            {
                TxtFilePath2.Text = openFileDialog.FileName;
            }
        }

        // spremeni v hash
        public string IzracunajSHA256(string potDoDatoteke)
        {
            using (SHA256 mojeSHA256 = SHA256.Create())
            {
                using (FileStream stream = File.OpenRead(potDoDatoteke))
                {

                    byte[] hashBiti = mojeSHA256.ComputeHash(stream);

                    return BitConverter.ToString(hashBiti).Replace("-", "").ToLower();  // pretvori bajte v Hex nit
                }
             
            }
        }

        public string IzracunajSHA512(string potDoDatoteke)
        {
            using (SHA512 mojeSHA512 = SHA512.Create())
            {
                using (FileStream stream = File.OpenRead(potDoDatoteke))
                {

                    byte[] hashBiti = mojeSHA512.ComputeHash(stream);

                    return BitConverter.ToString(hashBiti).Replace("-", "").ToLower();  // pretvori bajte v Hex nit
                }

            }
        }


        private void BtnHash_Click(object sender, RoutedEventArgs e)
        {
            string pot = TxtFilePath.Text;

            if (File.Exists(pot))
            {
                string rezultat = "";

                if(ComboAlgo.Text == "SHA-256")
                {
                    rezultat = IzracunajHash(pot, SHA256.Create()); //Poklic metodo za hashirat ter zapisemo v textBox
                }
                else if(ComboAlgo.Text == "SHA-512")
                {
                    rezultat = IzracunajHash(pot, SHA512.Create());
                }
                else if (ComboAlgo.Text == "SHA-3 (256-bit)") 
                {
                    rezultat = IzracunajSHA3_256(pot);
                }


                TxtHashResult.Text = rezultat;
            }
            else
            {
                MessageBox.Show("Najprej izberi veljavno datoteko");
            }
        }

        public string IzracunajHash(string potDoDatoteke, HashAlgorithm algoritem)
        {
            using (algoritem)
            {
                using (FileStream stream = File.OpenRead(potDoDatoteke))  // bere po kosckih
                {
                    byte[] hashBiti = algoritem.ComputeHash(stream);
                    return BitConverter.ToString(hashBiti).Replace("-","").ToLower(); //spremeni bajte v hex zapis
                }
            }

        }

        public string IzracunajSHA3_256(string potDoDatoteke)
        {
            // Ustvarimo SHA3-256 digest
            Sha3Digest digest = new Sha3Digest(256);
            byte[] vsebina = File.ReadAllBytes(potDoDatoteke);

            digest.BlockUpdate(vsebina, 0, vsebina.Length);   // po koliko naj vzame
            byte[] hashBiti = new byte[digest.GetDigestSize()];
            digest.DoFinal(hashBiti, 0);

            return BitConverter.ToString(hashBiti).Replace("-", "").ToLower();
        }

        public byte[] GenerirajSol(int dolzina = 16)
        {
            byte[] sol = new byte[dolzina];
            using (var rng = RandomNumberGenerator.Create()) 
            {
                rng.GetBytes(sol);
            }
            return sol;    // zgenerira random nit bytov 16
        }

        public byte[] IzracunajArgon2(string geslo, byte[] sol)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(geslo));

            argon2.Salt = sol;
            argon2.DegreeOfParallelism = 8;   // Stevilo jeder
            argon2.Iterations = 4;            // stevilo prehodov
            argon2.MemorySize = 1024 * 64;   // 64 MB rama

            return argon2.GetBytes(32);
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string uporabnik = TxtUsername.Text;
            string geslo = TxtPassword.Password;   

            if(string.IsNullOrEmpty(uporabnik) || string.IsNullOrEmpty(geslo))
            {
                MessageBox.Show("Vpisi geslo ter uporabnika");
                return;

            }

            byte[] sol = GenerirajSol(16);  

            byte[] hash = IzracunajArgon2(geslo, sol);  // desifrira geslo z dodanim soljo

            string solBase64 = Convert.ToBase64String(sol);
            string hashBase64 = Convert.ToBase64String(hash);  // pretvorimo v text

            string vrstica = $"{uporabnik}|{solBase64}|{hashBase64}";    //shranim v datoteko
            File.AppendAllLines("uporabnik.txt", new[] { vrstica });


            MessageBox.Show("Uspesno registtriran");
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string vnesenUporabnik = TxtUsername.Text; 
            string vnesenoGeslo = TxtPassword.Password;

            if (!File.Exists("uporabnik.txt"))
            {
                MessageBox.Show("Uporabnik ni registriran");
            }


            // preberemo vrstice v datoteki
            string[] vrstica = File.ReadAllLines("uporabnik.txt");
            bool uspeh = false;

            foreach(string v in vrstica)
            {
               
                string[] deli = v.Split('|');

                if (deli[0].Trim() == vnesenUporabnik) 
                {
                    // pretvorimo v text 
                    byte[] shranjenaSol = Convert.FromBase64String(deli[1].Trim());   // hash sol
                    string shranjenHashBase64 = deli[2].Trim();   //hash geslo

                    // preverja geslo z dodano soljo
                    byte[] novHash = IzracunajArgon2(vnesenoGeslo, shranjenaSol);
                    string novHashBase64 = Convert.ToBase64String(novHash);


                    // primerjamo
                    if (shranjenHashBase64 == novHashBase64)
                    {
                        uspeh = true;
                        break;
                    }
                }
            }
            if (uspeh)
            {
                MessageBox.Show("Prijava uspela");
            }
            else
            {
                MessageBox.Show("Napacno geslo ali uporabnik");
            }
        }

        private void BtnCompare_Click(object sender, RoutedEventArgs e)
        {
            
            string pot1 = TxtFilePath.Text;
            string pot2 = TxtFilePath2.Text;

            if (File.Exists(pot1) && File.Exists(pot2))
            {
                string h1 = IzracunajSHA256(pot1);
                string h2 = IzracunajSHA256(pot2);

                if (h1 == h2) MessageBox.Show("Datoteki sta identični");
                else MessageBox.Show("Datoteki sta različni");
            }
        }
    }
}