using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace VarnostNaloga2
{
    public partial class MainWindow : Window
    {

        private Dictionary<char, char> kljuc = new Dictionary<char, char>();  // hrani preslikavo 

        public MainWindow()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // starejsi zapisi
            InitializeComponent();
            PonastaviKljuc();     
        }


        // NALOŽI ŠIFROPIS 
        private void btnNaloziSif_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                txtSifrirano.Text = PreberiDatoteko(ofd.FileName); //prebero datoteko
                PonastaviKljuc();
                PosodobiDesifriranje();
                NarisiGraf(txtSifrirano.Text);
            }
        }

        private string PreberiDatoteko(string pot)
        {

            string text = File.ReadAllText(pot, Encoding.UTF8);

            if (!text.Contains("�"))
                return text;


            text = File.ReadAllText(pot, Encoding.GetEncoding(1250));

            if (!text.Contains("�"))
                return text;


            text = File.ReadAllText(pot, Encoding.Unicode);

            return text;
        }

        // NALOŽI REFERENCO
        private void btnNaloziRef_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                string refText = PreberiDatoteko(ofd.FileName);
                string sifText = txtSifrirano.Text;

                if (string.IsNullOrWhiteSpace(sifText))
                {
                    MessageBox.Show("Najprej naloži šifropis!");
                    return;
                }

                var refCrke = PridobiUrejeneCrkePoFrekvenci(refText);
                var sifCrke = PridobiUrejeneCrkePoFrekvenci(sifText);

                PonastaviKljuc();

                int stevilkaSkupnihCrk = Math.Min(refCrke.Count, sifCrke.Count);

                for (int i = 0; i < stevilkaSkupnihCrk; i++)
                {
                    kljuc[sifCrke[i]] = refCrke[i];
                }

                PosodobiDesifriranje();
                NarisiGraf(refText);

                MessageBox.Show("Frekvenčna analiza končana!");
            }
        }


        

        private void PonastaviKljuc()
        {
            string abeceda = "abcčdefghijklmnoprsštuvzž";
            kljuc.Clear();
            foreach (char c in abeceda)
                kljuc[c] = c;   // shranjuje preslikavo  
        }



        // FREKVENCA
        private Dictionary<char, int> IzracunajFrekvenco(string tekst)
        {
            var frekvence = new Dictionary<char, int>();  // crka ter koliko krat se poajvi

            foreach (char c in tekst.ToLower())  // skozi vse znake  uposteva samo crke
            {
                if (char.IsLetter(c))
                {
                    if (frekvence.ContainsKey(c)) frekvence[c]++;
                    else frekvence[c] = 1;
                }
            }

            return frekvence;
        }

        private List<char> PridobiUrejeneCrkePoFrekvenci(string tekst)
        {
            var frekvence = IzracunajFrekvenco(tekst);

            
            var seznamZaUrejanje = frekvence.ToList();

            // od najpogostejsih do najmanj
            seznamZaUrejanje.Sort((a, b) => b.Value.CompareTo(a.Value));  

            //  poberi samo črke
            List<char> samoCrke = new List<char>();
            foreach (var par in seznamZaUrejanje)
            {
                samoCrke.Add(par.Key);
            }

            return samoCrke;
        }

        

        private void PosodobiDesifriranje()
        {
            StringBuilder zgrajenoBesedilo = new StringBuilder();  // za urejanje stringu

            foreach (char c in txtSifrirano.Text)
            {
                char lower = char.ToLower(c);

                if (kljuc.ContainsKey(lower))  // ce imamo crko v kljucu jo zamnjamo z novo
                {
                    char nova = kljuc[lower];
                    zgrajenoBesedilo.Append(char.IsUpper(c) ? char.ToUpper(nova) : nova); //ohranimo veliksot crk
                }
                else zgrajenoBesedilo.Append(c);  // preskocimo znake ce nis crke
            }

            txtDesifrirano.Text = zgrajenoBesedilo.ToString();
            NarisiGraf(txtDesifrirano.Text);
        }

       

        // zamenjaj 
        private void btnZamenjaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtOd.Text) || string.IsNullOrEmpty(txtV.Text)) return;

            char sifriranaCrka = char.ToLower(txtOd.Text[0]);
            char zeljenaCrka = char.ToLower(txtV.Text[0]);

            // Poiščemo katera črka v šifri je do sedaj kazala na to crko
            char trenutniLastnikZeljene = '\0';
            foreach (var vnos in kljuc)
            {
                if (vnos.Value == zeljenaCrka)
                {
                    trenutniLastnikZeljene = vnos.Key;
                    break;
                }
            }

            if (trenutniLastnikZeljene != '\0')
            {
                // Klasična zamenjava (Swap) dveh vrednosti
                char staraVrednost = kljuc[sifriranaCrka];
                kljuc[sifriranaCrka] = zeljenaCrka;
                kljuc[trenutniLastnikZeljene] = staraVrednost;
            }

            PosodobiDesifriranje();
        }

        // SHRANI
        private void btnShrani_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            if (sfd.ShowDialog() == true)
            {
                File.WriteAllText(sfd.FileName, txtDesifrirano.Text, Encoding.UTF8);
            }
        }

        

        // GRAF 
        private void NarisiGraf(string tekst)
        {
            grafCanvas.Children.Clear();

            var frekvence = IzracunajFrekvenco(tekst);
            if (frekvence.Count == 0) return;

            double sirina = grafCanvas.ActualWidth;
            double visina = grafCanvas.ActualHeight;

            if (sirina == 0 || visina == 0)
            {
                grafCanvas.UpdateLayout();
                sirina = grafCanvas.ActualWidth;
                visina = grafCanvas.ActualHeight;
            }

            double max = frekvence.Values.Max();
            double stolpecSirina = sirina / frekvence.Count;

            int i = 0;
            foreach (var par in frekvence.OrderByDescending(x => x.Value))
            {
                double visinaStolpca = (par.Value / max) * (visina - 30); // koliko bo visok

                Rectangle rect = new Rectangle
                {
                    Width = stolpecSirina - 4,
                    Height = visinaStolpca,
                    Fill = Brushes.Blue
                };

                Canvas.SetLeft(rect, i * stolpecSirina);
                Canvas.SetTop(rect, visina - visinaStolpca - 20);

                grafCanvas.Children.Add(rect);

                TextBlock txt = new TextBlock
                {
                    Text = par.Key.ToString()
                };

                Canvas.SetLeft(txt, i * stolpecSirina);
                Canvas.SetTop(txt, visina - 20);

                grafCanvas.Children.Add(txt);

                i++;
            }
        }
    }
}