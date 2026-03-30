using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using Humanizer;
using MiniExcelLibs;

namespace Dodatna
{
    public class Opravilo
    {
        public DateTime Ustvarjeno { get; set; } = DateTime.Now;
        public DateTime Rok { get; set; } = DateTime.Now;
        public string Opis { get; set; } = "";
    }

    public partial class MainWindow : Window
    {
        private List<Opravilo> vsaOpravila = new List<Opravilo>();
        private string datoteka = "podatki.csv";

        public MainWindow()
        {
            InitializeComponent();

            // ce ze obstaja
            if (File.Exists(datoteka))
            {
                vsaOpravila = MiniExcel.Query<Opravilo>(datoteka).ToList();
            }

            OsveziSeznam();
        }

        public void Iskalnik_TextChanged(object? sender, TextChangedEventArgs e) => OsveziSeznam();
        public void FilterSort_SelectionChanged(object? sender, SelectionChangedEventArgs e) => OsveziSeznam();

        public void OsveziSeznam()
        {
            
            if (SeznamPrikaz == null || Iskalnik == null)
            {
                return;
            }

            // Vzamemo besedilo iz iskalnika, ga spremenimo v male črke 
            string iskaniTekst = Iskalnik.Text?.ToLower() ?? "";

      
            // Ustvarimo nov prazen seznam, v katerega bomo dali samo tista opravila
            List<Opravilo> filtriranaOpravila = new List<Opravilo>();

            foreach (var opravilo in vsaOpravila)
            {
                if (opravilo.Opis.ToLower().Contains(iskaniTekst))
                {
                    filtriranaOpravila.Add(opravilo);
                }
            }

            
            // preverimo katera možnost je izbrana 
            if (FilterSort.SelectedIndex == 1)
            {
                // razvrstimo po roku 
                filtriranaOpravila = filtriranaOpravila.OrderBy(o => o.Rok).ToList();
            }
            else
            {
                // razvrstimo po datumu ustvarjanja 
                filtriranaOpravila = filtriranaOpravila.OrderByDescending(o => o.Ustvarjeno).ToList();
            }

            
            // Ustvarimo seznam besedil 
            List<string> vrsticeZaPrikaz = new List<string>();
            CultureInfo slovenskaKultura = new CultureInfo("sl-SI");

            foreach (var o in filtriranaOpravila)
            { 
                // ce je predolgo se zbrise
                string kratekOpis = o.Opis.Truncate(25, "...");
                // Humanize pa spremeni datum v "čez 2 dni".
                string prijazenDatum = o.Rok.Humanize(culture: slovenskaKultura);

                // sestavimo celotno vrstico
                string vrstica = $"{o.Rok:dd.MM.yyyy} | {kratekOpis} ({prijazenDatum})";

                vrsticeZaPrikaz.Add(vrstica);
            }

            SeznamPrikaz.ItemsSource = vrsticeZaPrikaz;
        }

        public void GumbIzvoz_Click(object? sender, RoutedEventArgs e)
        {
            string pot = "Izvoz_Opravil.xlsx";
            try
            {
                if (File.Exists(pot)) File.Delete(pot);

                var cfg = new MiniExcelLibs.OpenXml.OpenXmlConfiguration { AutoFilter = true };
                MiniExcel.SaveAs(pot, vsaOpravila, configuration: cfg);

                VnosOpravila.Text = "Izvoz uspel!";
            }
            catch
            {
                VnosOpravila.Text = "Zapri Excel datoteko!";
            }
        }

        public void GumbDodaj_Click(object? sender, RoutedEventArgs e)
        {
            // Preprečimo dodajanje, če uporabnik ni vpisal ničesar
            if (string.IsNullOrWhiteSpace(VnosOpravila.Text)) return;

            vsaOpravila.Add(new Opravilo
            {
                Rok = IzbiraDatuma.SelectedDate?.DateTime.Date ?? DateTime.Now.Date,
                Opis = VnosOpravila.Text
            });

            VnosOpravila.Text = ""; // počistimo polje za vnos
            Shrani();                  // shranimo v CSV datoteko
            OsveziSeznam();         // posodobimo prikaz na zaslonu
        }

        public void GumbIzbrisi_Click(object? sender, RoutedEventArgs e)
        {
            // ce je v seznamu nekaj izbrano, to odstranimo
            if (SeznamPrikaz.SelectedIndex >= 0)
            {
                vsaOpravila.RemoveAt(SeznamPrikaz.SelectedIndex);
                Shrani();
                OsveziSeznam();
            }
        }

        private void Shrani()
        {
            try
            {
                // miniExcel prepiše staro CSV datoteko z novimi podatki iz seznama
                if (File.Exists(datoteka)) File.Delete(datoteka);
                MiniExcel.SaveAs(datoteka, vsaOpravila);
            }
            catch { }
        }
    }
}