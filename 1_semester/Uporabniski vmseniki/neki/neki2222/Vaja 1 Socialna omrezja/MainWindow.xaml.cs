using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Vaja_1_Socialna_omrezja
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new GlavniViewModel();
        }
    }

    
    public class GlavniViewModel : INotifyPropertyChanged
    {
        private string imePriimek = "Janez Novak";
        public string ImePriimek
        {
            get { return imePriimek; }
            set { imePriimek = value; OnPropertyChanged("ImePriimek"); }
        }

        private string biografija = "Študent IKT na FERI Maribor.";
        public string Biografija
        {
            get { return biografija; }
            set { biografija = value; OnPropertyChanged("Biografija"); }
        }

        public ObservableCollection<Objava> Objave { get; set; }

        private Objava izbranaObjava;
        public Objava IzbranaObjava
        {
            get { return izbranaObjava; }
            set
            {
                izbranaObjava = value;
                OnPropertyChanged("IzbranaObjava");
                OnPropertyChanged("DostopniUkazi");
            }
        }

        private string statusGlasa = "Glasovni sistem pripravljen.";
        public string StatusGlasa
        {
            get { return statusGlasa; }
            set { statusGlasa = value; OnPropertyChanged("StatusGlasa"); }
        }

        public string DostopniUkazi => IzbranaObjava == null ? "Ukazi: [Add post], [Exit app]" : "Ukazi: [Like post], [Edit post], [Cancel selection]";

        public ICommand OdpriDodajCommand { get; set; }
        public ICommand OdpriUrediCommand { get; set; }
        public ICommand VsecCommand { get; set; }
        public ICommand OdstraniCommand { get; set; }
        public ICommand PrekliciIzbiroCommand { get; set; }
        public ICommand IzhodCommand { get; set; }

        private SpeechRecognitionEngine prepoznavalnik = new SpeechRecognitionEngine();
        private SpeechSynthesizer govorilec = new SpeechSynthesizer();
        private bool cakamNaPotrditev = false;

        public GlavniViewModel()
        {
            Objave = new ObservableCollection<Objava> {
                new Objava { Naslov = "Pozdrav", Vsebina = "Uspešno narejena vaja.", Vsec = 5 }
            };

            // Nastavitev ukazov
            OdpriDodajCommand = new UkazPomocnik(p => OdpriOknoVnos(false));
            OdpriUrediCommand = new UkazPomocnik(p => OdpriOknoVnos(true));
            VsecCommand = new UkazPomocnik(p => { if (IzbranaObjava != null) IzbranaObjava.Vsec++; });
            OdstraniCommand = new UkazPomocnik(p => { if (IzbranaObjava != null) { cakamNaPotrditev = true; govorilec.SpeakAsync("Are you sure? Say yes or no."); } });
            PrekliciIzbiroCommand = new UkazPomocnik(p => { IzbranaObjava = null; });
            IzhodCommand = new UkazPomocnik(p => Application.Current.Shutdown());

            InicilizirajGovor();
        }

        // G Nastavitev govora
        private void InicilizirajGovor()
        {
            try
            {
                Choices moznosti = new Choices();
                moznosti.Add(new string[] { "add post", "like post", "edit post", "delete post", "cancel selection", "exit app", "yes", "no" });
                GrammarBuilder gb = new GrammarBuilder(moznosti);
                prepoznavalnik.LoadGrammar(new Grammar(gb));
                prepoznavalnik.SpeechRecognized += (s, e) => {
                    string besedilo = e.Result.Text.ToLower();
                    StatusGlasa = "Slišano: " + besedilo;

                    Application.Current.Dispatcher.Invoke(() => {
                        if (cakamNaPotrditev)
                        {
                            if (besedilo == "yes") { Objave.Remove(IzbranaObjava); govorilec.SpeakAsync("Removed."); }
                            else { govorilec.SpeakAsync("Action cancelled."); }
                            cakamNaPotrditev = false; return;
                        }

                        if (besedilo == "add post") OdpriOknoVnos(false);
                        if (besedilo == "like post") VsecCommand.Execute(null);
                        if (besedilo == "edit post") OdpriOknoVnos(true);
                        if (besedilo == "cancel selection") { IzbranaObjava = null; govorilec.SpeakAsync("Selection cleared."); }
                        if (besedilo == "delete post") OdstraniCommand.Execute(null);
                        if (besedilo == "exit app") IzhodCommand.Execute(null);
                    });
                };
                prepoznavalnik.SetInputToDefaultAudioDevice();
                prepoznavalnik.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch { StatusGlasa = "Napaka z mikrofonom."; }
        }

        //  Okno za vnos podatkov
        private void OdpriOknoVnos(bool uredi)
        {
            if (uredi && IzbranaObjava == null) { MessageBox.Show("Izberite objavo!"); return; }

            Window okno = new Window
            {
                Title = uredi ? "Urejanje" : "Dodajanje",
                Width = 350,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(15) };
            TextBox vnosNaslov = new TextBox { Text = uredi ? IzbranaObjava.Naslov : "Vpiši naslov", Margin = new Thickness(10) };
            TextBox vnosVsebina = new TextBox { Text = uredi ? IzbranaObjava.Vsebina : "Vpiši vsebino", Height = 70, TextWrapping = TextWrapping.Wrap };
            Button gumbShrani = new Button { Content = "Shrani", Margin = new Thickness(10), Height = 30 };

            gumbShrani.Click += (s, e) => {
                if (uredi) { IzbranaObjava.Naslov = vnosNaslov.Text; IzbranaObjava.Vsebina = vnosVsebina.Text; }
                else { Objave.Add(new Objava { Naslov = vnosNaslov.Text, Vsebina = vnosVsebina.Text, Vsec = 0 }); }
                okno.Close();
            };

            panel.Children.Add(new TextBlock { Text = "Naslov:" });
            panel.Children.Add(vnosNaslov);
            panel.Children.Add(new TextBlock { Text = "Vsebina:" });
            panel.Children.Add(vnosVsebina);
            panel.Children.Add(gumbShrani);
            okno.Content = panel;
            okno.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    //  model za objavo
    public class Objava : INotifyPropertyChanged
    {
        private string naslov;
        public string Naslov { get { return naslov; } set { naslov = value; OnPropertyChanged("Naslov"); } }

        private string vsebina;
        public string Vsebina { get { return vsebina; } set { vsebina = value; OnPropertyChanged("Vsebina"); } }

        private int vsec;
        public int Vsec { get { return vsec; } set { vsec = value; OnPropertyChanged("Vsec"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    
    public class UkazPomocnik : ICommand
    {
        private Action<object> akcija;
        public UkazPomocnik(Action<object> akcija) => this.akcija = akcija;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => akcija(parameter);
        public event EventHandler CanExecuteChanged;
    }
}