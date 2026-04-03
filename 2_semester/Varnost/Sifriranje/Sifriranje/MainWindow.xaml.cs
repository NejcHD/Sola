using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;



namespace Sifriranje
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string _izbranaPot = "";  // ulr do datoteke
        private byte[] _aesKey; // Tukaj bodo shranjeni bajti ključa
        private byte[] _aesIV;  // Tukaj bo shranjen IV

        private RSAParameters _rsaPublicKey;  // javni
        private RSAParameters _rsaPrivateKey;  // zasebni

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _izbranaPot = openFileDialog.FileName;
                txtFilePath.Text = _izbranaPot;
                lblStatus.Text = "Datoteka izbrana.";
            }



        }


        private void GenerirajAesKljuce()
        {
            int izbranaDolzina = int.Parse(((ComboBoxItem)cbAesSize.SelectedItem).Content.ToString()); //preberemo izbiro

            using (Aes mojAes = Aes.Create())
            {
                mojAes.KeySize = izbranaDolzina;  //nastavimo dolzino 128, 192, 256
                mojAes.GenerateKey();
                mojAes.GenerateIV();

                _aesKey = mojAes.Key;
                _aesIV = mojAes.IV;
            }

            lblStatus.Text = $"AES ključ ({izbranaDolzina} bit) in IV sta generirana!";  //izpis vizualno potrdimo
        }


        private void BtnEBtnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_izbranaPot))
            {
                MessageBox.Show("Najprej izberi datoteko");
                return;
            }

            GenerirajAesKljuce();  //zgeneriramo kljuc

            string potSifriraneDatoteke = _izbranaPot + ".ene";


            using (Aes mojAes = Aes.Create())
            {
                mojAes.Key = _aesKey;
                mojAes.IV = _aesIV;
                mojAes.Mode = CipherMode.CBC; // Standarni nacin  da so podatki povezani ce spremnis 1 se vsi drugi
                mojAes.Padding = PaddingMode.PKCS7; // Dopolne kar manka na koncu

                ICryptoTransform encryptor = mojAes.CreateEncryptor();

                // odpremo Datoteke za branje in pisnanje
                using (FileStream fsInput = new FileStream(_izbranaPot, FileMode.Open))  // prebere
                using (FileStream fsOutput = new FileStream(potSifriraneDatoteke, FileMode.Create))  //izpise
                using (CryptoStream cs = new CryptoStream(fsOutput, encryptor, CryptoStreamMode.Write))   // predela datoteke
                {
                    fsInput.CopyTo(cs);  // vsebina prekopiramo skozi sifrantor v novo datoteko
                }
            }

            lblStatus.Text = "Uspešno šifrirano v: " + System.IO.Path.GetFileName(potSifriraneDatoteke);
        }


        private void BtnDecrypt_Clicl(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_izbranaPot) || _aesKey == null)
            {
                MessageBox.Show("Izberi .enc datoteko in poskrbi da je kljuc v spominu");
                return;
            }

            string potDesifriraneDatoteke = _izbranaPot.Replace(".enc", "") + "_decrypted.txt";


            try
            {
                using (Aes mojAes = Aes.Create())
                {
                    mojAes.Key = _aesKey;
                    mojAes.IV = _aesIV;
                    mojAes.Mode = CipherMode.CBC;
                    mojAes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = mojAes.CreateDecryptor();  // za dekriptat

                    using (FileStream fsInput = new FileStream(_izbranaPot, FileMode.Open))
                    using (FileStream fsOutput = new FileStream(potDesifriraneDatoteke, FileMode.Create))
                    using (CryptoStream cs = new CryptoStream(fsInput, decryptor, CryptoStreamMode.Read))
                    {
                        cs.CopyTo(fsOutput);
                    }
                }

                lblStatus.Text = "Dešifrirano v: " + System.IO.Path.GetFileName(potDesifriraneDatoteke);
            }
            catch
            {
                MessageBox.Show("Napaka: Napačen ključ ali poškodovana datoteka!");
            }

        }



        private void GenerirajRsaKljuc()
        {
            int rsaSize = int.Parse(((ComboBoxItem)cbRsaSize.SelectedItem).Content.ToString());

            using (RSA rsa = RSA.Create(rsaSize))
            {
                _rsaPublicKey = rsa.ExportParameters(false);   //  false samo javni kljuc
                _rsaPrivateKey = rsa.ExportParameters(true);   // true = vkljucuje zasebne kljuc
            }

            lblStatus.Text = $"RSA-{rsaSize} ključa sta pripravljena.";
        }


        private void BtnSaveKeys_Click(object sender, RoutedEventArgs e)
        {
            if (_aesKey == null || _aesIV == null)
            {
                MessageBox.Show("Najprej šifriraj datoteko, da dobiš AES ključe!");
                return;
            }

            // sifriramo aes kljuc z rsa javnim kljucem
            GenerirajRsaKljuc();  // ustvarimo rsa par
            byte[] encryptedAesKey;


            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(_rsaPublicKey);
                // OaepSHA256 je Padding
                encryptedAesKey = rsa.Encrypt(_aesKey, RSAEncryptionPadding.OaepSHA256);  // enkriptamo
            }

            SaveFileDialog sfd = new SaveFileDialog { FileName = "Kljuci.dat", Filter = "Key files (*.dat)|*.dat" };
            if (sfd.ShowDialog() == true)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(sfd.FileName, FileMode.Create)))
                {
                    writer.Write(encryptedAesKey.Length); // Dolžina šifriranega ključa
                    writer.Write(encryptedAesKey);
                    writer.Write(_aesIV.Length);          // Dolžina IV
                    writer.Write(_aesIV);


                    // Shranimo še RSA zasebni ključ kot XML, da ga lahko naložimo nazaj
                    using (RSA rsaForExport = RSA.Create())
                    {
                        rsaForExport.ImportParameters(_rsaPrivateKey);
                        writer.Write(rsaForExport.ToXmlString(true));
                    }
                }

                lblStatus.Text = "Ključi varno shranjeni in zaščiteni z RSA!";
            }
        }

        private void BtnLoadKeys_Clicl(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog { Filter = "Key files (*.dat)|*.dat" };
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(ofd.FileName, FileMode.Open)))
                    {
                        // preberemo aes kljuc
                        int encryptedKeyLength = reader.ReadInt32();
                        byte[] encryptedAesKey = reader.ReadBytes(encryptedKeyLength);

                        // oreberemo IV
                        int ivLength = reader.ReadInt32();
                        _aesIV = reader.ReadBytes(ivLength);


                        //Preberemo Rsa zasebni kljuc v xml
                        string rsaXml = reader.ReadString();

                        //upporabino rsa da desifriramo aes
                        using (RSA rsa = RSA.Create())
                        {
                            rsa.FromXmlString(rsaXml);
                            _aesKey = rsa.Decrypt(encryptedAesKey, RSAEncryptionPadding.OaepSHA256);
                        }

                    }
                    lblStatus.Text = "Ključi uspešno naloženi in RSA odklepanje končano!";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Napaka pri nalaganju ključev: " + ex.Message);
                }

            }

        }

    }
}
