using Microsoft.Win32;
using StrojnoUcenjeProjekt.Algoritmi;
using System.Collections.Generic;
using System.IO;        
using System.Linq; 
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

namespace StrojnoUcenjeProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Glavni seznam, kjer bodo shranjeni vsi podatki iz CSV
        private List<PodatkovnaTocka> vsiPodatki = new List<PodatkovnaTocka>();
        private List<PodatkovnaTocka> ucnaMnozica = new List<PodatkovnaTocka>();
        private List<PodatkovnaTocka> testnaMnozica = new List<PodatkovnaTocka>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private PodatkovnaTocka RaclaniiVrstico(string vrstica)
        {
            // razbijemo vrsco na dele glede na vejco
            string[] deli = vrstica.Split(',');
            PodatkovnaTocka novoTocka = new PodatkovnaTocka();

            for (int i = 0; i < deli.Length - 1; i++) 
            {
                // pregelda ce so stevilke ali besede, preveri vse mozne znake ter vejice ali pike
                if (double.TryParse(deli[i], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double vrednost))
                {
                    novoTocka.Atributi.Add(vrednost);
                }
            }

            novoTocka.Oznaka = deli[deli.Length -1].Trim();
            return novoTocka;
        }

        private void NaloziInRazdeliPodatke(string potDatoteke)
        {
            try {
                // preberemo vrstice
                string[] vrstica = File.ReadAllLines(potDatoteke);
                vsiPodatki.Clear();

                // preskocimo prvo vrstico
                for(int i= 1; i < vrstica.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(vrstica[i]))
                    {
                        vsiPodatki.Add(RaclaniiVrstico(vrstica[i]));  // vsako vrstico spremeni v objekt ter shrani
                    }
                }

                Random rnd = new Random();        // Premesamo pdoatke
                vsiPodatki = vsiPodatki.OrderBy(x => rnd.Next()).ToList();

                // razdelimo na 80%  v ucno mnozico ostalo v testno
                int meja = (int)(vsiPodatki.Count * 0.8);
                ucnaMnozica = vsiPodatki.Take(meja).ToList();
                testnaMnozica = vsiPodatki.Skip(meja).ToList();

                TxtLog.AppendText($"Podatki nalozeni,  Ucnih: {ucnaMnozica.Count} Testnih:  { testnaMnozica.Count} ");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Napaka pri nalaganju: " + ex.Message);
            }
        }

        private void IzvediTestiranje()
        {
            if(ucnaMnozica.Count == 0 || testnaMnozica.Count == 0)
            {
                MessageBox.Show("Nalozi pdoatke");
                return;
            }

            // Preberemo vrednost k iz TextBox-a 
            if (!int.TryParse(TxtK.Text, out int k))
            {
                k = 3; // Če uporabnik ne vpiše nič uporabimo 3
            }

            int pravilneNapovedi = 0;

            // Preverimo kateri algoritem je izbran v ComboBoxu
            if (ComboAlgoritem.SelectedIndex == 0) // k-NN
            {
                foreach (var testnaTocka in testnaMnozica)
                {
                    string napoved = K_nn.KlasificirajTocko(testnaTocka, ucnaMnozica, k);  // izvede k-nn fukcijo iz ločene datoteke
                    testnaTocka.NapovedanaOznaka = napoved;  // rezultat shranimo

                    if (napoved == testnaTocka.Oznaka) pravilneNapovedi++;
                }
            }
            else // Odločitveno drevo
            {
                TxtLog.AppendText("\n--- Gradnja drevesa v teku... ---\n");
                // Zgradimo drevo na učnih podatkih
                var koren = DrevoAlgoritem.NauciDrevo(ucnaMnozica, 0);

                foreach (var testnaTocka in testnaMnozica)
                {
                    string napoved = DrevoAlgoritem.Napovej(koren, testnaTocka); // izvede napoved z drevesom
                    testnaTocka.NapovedanaOznaka = napoved;

                    if (napoved == testnaTocka.Oznaka) pravilneNapovedi++;
                }
            }

            double tocnost = (double)pravilneNapovedi / testnaMnozica.Count * 100;
            TxtAccuracy.Text = $"Točnost: {tocnost:F2} %";
            TxtLog.AppendText($"Končano! Od {testnaMnozica.Count} testnih primerov smo jih pravilno uvrstili {pravilneNapovedi}.\n");
        }

        private void IzracunajMetrike()
        {
            // seznam vseh razredu v mnozici
            var unikatniRazred = testnaMnozica.Select(t => t.Oznaka).Distinct().ToList();

            double skupiF1 = 0;
            string izpisF1 = "Tocnost uspeha:\n";

            foreach (var razred in unikatniRazred)
            {
                // true pozitiv   dejanski je razred ter napoved je razred    situacija = 1  algoritem = 1
                int tp = testnaMnozica.Count(t => t.Oznaka == razred && t.NapovedanaOznaka == razred);

                //false pozitiv   dejanski ni razred ter napoved je razred      situacija = 0  algoritem = 1
                int fp = testnaMnozica.Count(t => t.Oznaka != razred && t.NapovedanaOznaka == razred);

                //false negativ   dejanski ej reazred in napoved ni razred      situacija = 1  algoritem = 0
                int fn = testnaMnozica.Count(t => t.Oznaka == razred && t.NapovedanaOznaka != razred);

                // natancnost
                double precision;
                if ((tp + fp) > 0)
                {
                    precision = (double)tp / (tp + fp);
                }
                else
                {
                    precision = 0;
                }

                //Koliko je uspesen pri iskanju
                double recall;
                if ((tp + fn) > 0)
                {
                    recall = (double)tp / (tp + fn);
                }
                else
                {
                    recall = 0;
                }

                // Koliko je tocen uspeh
                double f1;
                if ((precision + recall) > 0)
                {
                    f1 = 2 * (precision * recall) / (precision + recall);
                }
                else
                {
                    f1 = 0;
                }

                skupiF1 += f1;
                izpisF1 += $"{razred}: {f1:F2}\n";
            }

            // Povprečni F1 (Macro-average)
            double povprecniF1 = skupiF1 / unikatniRazred.Count;

            // Izpis v vmesnik
            TxtF1.Text = $"Povprečni razredi: {povprecniF1:F2}";
            TxtLog.AppendText("\n--- Podrobna analiza ---\n" + izpisF1);
        }

        private void BtnLoadData_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "CSV datoteke (*.csv)|*.csv";

            if (dialog.ShowDialog() == true)
            {
                NaloziInRazdeliPodatke(dialog.FileName);
                TxtStatus.Text = "Status: Podatki naloženi.";
            }
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            
            if (ucnaMnozica.Count == 0 || testnaMnozica.Count == 0)
            {
                MessageBox.Show("Prosim, najprej naloži CSV datoteko!");
                return;
            }

            // pobrišemo prejšnje izpise
            TxtLog.Clear();
            TxtStatus.Text = "Status: Klasifikacija teče...";

            
            if (ComboAlgoritem.SelectedIndex == 0)
            {
                TxtLog.AppendText("-- Izvaja se k-NN klasifikacija --\n");
                IzvediTestiranje(); // Ta metoda že interno kliče K_nn.KlasificirajTocko
            }
            else
            {
                TxtLog.AppendText("-- Izvaja se Odločitveno drevo --\n");

                //  gradnja drevesa na učni množici
                Vozlisce koren = DrevoAlgoritem.NauciDrevo(ucnaMnozica, 0);

                // napovedovanje za vsako točko v testni množici
                foreach (var tocka in testnaMnozica)
                {
                    tocka.NapovedanaOznaka = DrevoAlgoritem.Napovej(koren, tocka);
                }
            }

            
            IzracunajMetrike();     
            IzpisiMatrikoZmede();   

            TxtStatus.Text = "Status: Klasifikacija končana.";
        }

        private void IzpisiMatrikoZmede()
        {
            //  dobimo seznam vseh razredov ki se pojavijo v podatkih
            var unikatniRazredi = vsiPodatki.Select(p => p.Oznaka)
                                            .Distinct()
                                            .OrderBy(x => x)  // po abecedi
                                            .ToList();

            TxtLog.AppendText("\n _Matrika zmede_ \n");

            // Glava tabele stolpci napovedni razredi
            string glava = "Dejansko \\ Napovedano\t";

            foreach (var r in unikatniRazredi)
            {
                glava += r + "\t";
            }

            TxtLog.AppendText(glava + "\n");

            // Vrstice tabele
            foreach (var dejanski in unikatniRazredi)  // vrstice resnice
            {
                string vrstica = dejanski + "\t\t";
                foreach (var napovedan in unikatniRazredi) //stolpci napovedi
                {
                    int stevilka = testnaMnozica.Count(t => t.Oznaka == dejanski && t.NapovedanaOznaka == napovedan);
                    vrstica += stevilka + "\t";
                }
                TxtLog.AppendText(vrstica + "\n");
            }
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(TxtLog.Text))
            {
                MessageBox.Show("Log je prazen. Najprej izvedi klasifikacijo!");
                return;
            }

           
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog.FileName = "Rezultati_Klasifikacije.txt"; // Privzeto ime

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    
                    File.WriteAllText(saveFileDialog.FileName, TxtLog.Text);

                    MessageBox.Show("Rezultati so bili uspešno shranjeni!", "Uspeh",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Prišlo je do napake pri shranjevanju: " + ex.Message);
                }
            }
        }
    }
}