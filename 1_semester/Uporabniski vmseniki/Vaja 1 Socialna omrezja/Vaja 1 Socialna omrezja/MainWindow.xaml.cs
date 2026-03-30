using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            this.DataContext = new ViewModel();
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        private string _imePriimek = "Janez Novak";
        public string ImePriimek
        {
            get => _imePriimek;
            set
            {
                _imePriimek = value;
                OnPropertyChanged(nameof(ImePriimek));
            }
        }

        private string _biografija = "Student Feri v Mariboru, na smeri Ikt vš. 2 letnik";
        public string Biografija
        {
            get => _biografija;
            set
            {
                _biografija = value;
                OnPropertyChanged(nameof(Biografija));
            }
        }

        public ObservableCollection<Objava> Objave { get; set; }

        private Objava _izbranaObjava;
        public Objava IzbranaObjava
        {
            get => _izbranaObjava;
            set
            {
                _izbranaObjava = value;
                OnPropertyChanged(nameof(IzbranaObjava));
                OnPropertyChanged(nameof(JeIzbranaObjava));
            }
        }

        public bool JeIzbranaObjava => IzbranaObjava != null;

        public ICommand IzhodCommand { get; }
        public ICommand DodajStaticnoCommand { get; }
        public ICommand UrediStaticnoCommand { get; }
        public ICommand DodajCommand { get; }
        public ICommand UrediCommand { get; }
        public ICommand OdstraniCommand { get; }
        public ICommand VsecCommand { get; }
        public ICommand CommentCommand { get; }

        private EditObjavaWindow _editOkno;

        public ViewModel()
        {
            IzhodCommand = new Command(IZHOD);
            DodajStaticnoCommand = new Command(DODAJ_STATICNO);
            UrediStaticnoCommand = new Command(UREDI_STATICNO);
            DodajCommand = new Command(DODAJ);
            UrediCommand = new Command(UREDI);
            OdstraniCommand = new Command(ODSTRANI);
            VsecCommand = new Command(LIKE);
            CommentCommand = new Command(KOMENTIRAJ);

            Objave = new ObservableCollection<Objava> {
                new Objava
                {
                    Naslov = "Slika pred knjiznico",
                    Vsebina = "Sem sel se malo uciti za kolokvije",
                    Datum = DateTime.Now,
                    Avtor = "Janez Novak",
                    Vsec = 3,
                    SlikaObjava = "/Images/slika2.jpg"
                },
                new Objava
                {
                    Naslov = "Opravil Faks",
                    Vsebina = "Dans se ga pije",
                    Datum = DateTime.Now,
                    Avtor = "Maja Kranjc",
                    Vsec = 7,
                    SlikaObjava = "/Images/download.jpg"
                }
            };
        }

        private void IZHOD(object p)
        {
            Application.Current.Shutdown();
        }

        private void DODAJ_STATICNO(object p)
        {
            var novaObjava = new Objava
            {
                Naslov = "Nova statična objava",
                Vsebina = "To je nova statično dodana objava",
                Datum = DateTime.Now,
                Avtor = ImePriimek,
                Vsec = 0,
                SlikaObjava = "/Images/slika2.jpg"
            };
            Objave.Add(novaObjava);
        }

        private void UREDI_STATICNO(object p)
        {
            if (IzbranaObjava != null)
            {
                IzbranaObjava.Naslov = "Statično spremenjen naslov";
                IzbranaObjava.Vsebina = "Statično spremenjena vsebina";
            }
        }

        private void DODAJ(object p)
        {
            var addWindow = new AddObjavaWindow();
            addWindow.Owner = Application.Current.MainWindow;

            if (addWindow.ShowDialog() == true)
            {
                var novaObjava = addWindow.NovaObjava;
                novaObjava.Datum = DateTime.Now;
                Objave.Add(novaObjava);
            }
        }

        private void UREDI(object p)
        {
            if (IzbranaObjava == null) return;

            if (_editOkno == null)
            {
                _editOkno = new EditObjavaWindow(this);
                _editOkno.Owner = Application.Current.MainWindow;
                _editOkno.Closed += ZAPRI_OKNO;
                _editOkno.Show();
            }
            else
            {
                _editOkno.Activate();
            }
        }

        private void ZAPRI_OKNO(object sender, EventArgs e)
        {
            _editOkno = null;
        }

        private void ODSTRANI(object p)
        {
            if (IzbranaObjava != null)
            {
                Objave.Remove(IzbranaObjava);
            }
        }

        private void LIKE(object p)
        {
            if (IzbranaObjava != null)
            {
                IzbranaObjava.Vsec++;
            }
        }

        private void KOMENTIRAJ(object p)
        {
            MessageBox.Show("komentiranje");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string ime)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(ime));
        }
    }

    public class Command : ICommand
    {
        private readonly Action<object> _execute;

        public Command(Action<object> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }

    public class Objava : INotifyPropertyChanged
    {
        private string _naslov;
        public string Naslov
        {
            get => _naslov;
            set { _naslov = value; OnPropertyChanged(nameof(Naslov)); }
        }

        private string _vsebina;
        public string Vsebina
        {
            get => _vsebina;
            set { _vsebina = value; OnPropertyChanged(nameof(Vsebina)); }
        }

        private DateTime _datum;
        public DateTime Datum
        {
            get => _datum;
            set { _datum = value; OnPropertyChanged(nameof(Datum)); }
        }

        private int _vsec;
        public int Vsec
        {
            get => _vsec;
            set { _vsec = value; OnPropertyChanged(nameof(Vsec)); }
        }

        private string _avtor;
        public string Avtor
        {
            get => _avtor;
            set { _avtor = value; OnPropertyChanged(nameof(Avtor)); }
        }

        private string _slikaObjava;
        public string SlikaObjava
        {
            get => _slikaObjava;
            set { _slikaObjava = value; OnPropertyChanged(nameof(SlikaObjava)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string ime)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(ime));
        }
    }

    public partial class AddObjavaWindow : Window
    {
        public Objava NovaObjava { get; private set; }

        public AddObjavaWindow()
        {
            InitializeComponent();
            NovaObjava = new Objava();
            DataContext = this;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {

            //preveri ce so polja prazna
            if (string.IsNullOrWhiteSpace(NovaObjava.Naslov))
            {
                MessageBox.Show("Naslov je obvezen!");
                return;
            }

            if (string.IsNullOrWhiteSpace(NovaObjava.Vsebina))
            {
                MessageBox.Show("Vsebina je obvezna!");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void InitializeComponent()
        {
            //  okno
            this.Title = "Dodaj objavo";
            this.Width = 500;
            this.Height = 400;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            //  glavni panel
            StackPanel glavniPanel = new StackPanel();
            glavniPanel.Margin = new Thickness(15);  // odaljenost od roba je  Thickness
            glavniPanel.Width = 450;

            //naslov okna
            TextBlock naslovOkna = new TextBlock();
            naslovOkna.Text = "Dodaj novo objavo";
            naslovOkna.FontWeight = FontWeights.Bold;
            naslovOkna.FontSize = 16;
            naslovOkna.Margin = new Thickness(0, 0, 0, 10);

            glavniPanel.Children.Add(naslovOkna);

            // polje za naslov
            Label labelNaslov = new Label();
            labelNaslov.Content = "Naslov:";
            labelNaslov.Width = 100;

            TextBox poljeNaslov = new TextBox();
            poljeNaslov.Height = 25;
           // poveze z podatki
            poljeNaslov.SetBinding(TextBox.TextProperty, new System.Windows.Data.Binding("NovaObjava.Naslov"));

            DockPanel panelNaslov = new DockPanel();
            panelNaslov.Margin = new Thickness(0, 5, 0, 0);
            panelNaslov.Children.Add(labelNaslov);
            panelNaslov.Children.Add(poljeNaslov);

            glavniPanel.Children.Add(panelNaslov);

            // polje  vsebina
            Label labelVsebina = new Label();
            labelVsebina.Content = "Vsebina:";
            labelVsebina.Width = 100;

            TextBox poljeVsebina = new TextBox();
            poljeVsebina.Height = 60;
            poljeVsebina.TextWrapping = TextWrapping.Wrap;
            poljeVsebina.AcceptsReturn = true;
            
            poljeVsebina.SetBinding(TextBox.TextProperty, new System.Windows.Data.Binding("NovaObjava.Vsebina"));

            DockPanel panelVsebina = new DockPanel();
            panelVsebina.Margin = new Thickness(0, 5, 0, 0);
            panelVsebina.Children.Add(labelVsebina);
            panelVsebina.Children.Add(poljeVsebina);

            glavniPanel.Children.Add(panelVsebina);

            //polje za avtorja
            Label labelAvtor = new Label();
            labelAvtor.Content = "Avtor:";
            labelAvtor.Width = 100;

            TextBox poljeAvtor = new TextBox();
            poljeAvtor.Height = 25;
           
            poljeAvtor.SetBinding(TextBox.TextProperty, new System.Windows.Data.Binding("NovaObjava.Avtor"));

            DockPanel panelAvtor = new DockPanel();
            panelAvtor.Margin = new Thickness(0, 5, 0, 0);
            panelAvtor.Children.Add(labelAvtor);
            panelAvtor.Children.Add(poljeAvtor);

            glavniPanel.Children.Add(panelAvtor);

            // gumbe 
            StackPanel panelGumbov = new StackPanel();
            panelGumbov.Orientation = Orientation.Horizontal;
            panelGumbov.HorizontalAlignment = HorizontalAlignment.Right;
            panelGumbov.Margin = new Thickness(0, 15, 0, 0);

            Button gumbOK = new Button();
            gumbOK.Content = "OK";
            gumbOK.Width = 80;
            gumbOK.Height = 25;
            gumbOK.IsDefault = true;
            gumbOK.Click += Ok_Click;

            Button gumbPreklici = new Button();
            gumbPreklici.Content = "Prekliči";
            gumbPreklici.Width = 80;
            gumbPreklici.Height = 25;
            gumbPreklici.IsCancel = true;

            panelGumbov.Children.Add(gumbOK);
            panelGumbov.Children.Add(gumbPreklici);

            glavniPanel.Children.Add(panelGumbov);

            // Dodamo vse v okno
            this.Content = glavniPanel;
        }
    }

    public partial class EditObjavaWindow : Window
    {
        public EditObjavaWindow(ViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void InitializeComponent()
        {
            // Nastavimo okno
            this.Title = "Urejanje objave";
            this.Width = 500;
            this.Height = 400;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            // Glavni panel
            StackPanel glavniPanel = new StackPanel();
            glavniPanel.Margin = new Thickness(15);
            glavniPanel.Width = 450;

            // Naslov okna
            TextBlock naslov = new TextBlock();
            naslov.Text = "Uredi objavo";
            naslov.FontWeight = FontWeights.Bold;
            naslov.FontSize = 16;
            naslov.Margin = new Thickness(0, 0, 0, 10);
            glavniPanel.Children.Add(naslov);

            // Polje za naslov
            Label labelNaslov = new Label();
            labelNaslov.Content = "Naslov:";
            labelNaslov.Width = 100;

            TextBox poljeNaslov = new TextBox();
            poljeNaslov.Height = 25;
            poljeNaslov.SetBinding(TextBox.TextProperty, new System.Windows.Data.Binding("IzbranaObjava.Naslov"));

            DockPanel panelNaslov = new DockPanel();
            panelNaslov.Margin = new Thickness(0, 5, 0, 0);
            panelNaslov.Children.Add(labelNaslov);
            panelNaslov.Children.Add(poljeNaslov);
            glavniPanel.Children.Add(panelNaslov);

            // Polje za vsebino
            Label labelVsebina = new Label();
            labelVsebina.Content = "Vsebina:";
            labelVsebina.Width = 100;

            TextBox poljeVsebina = new TextBox();
            poljeVsebina.Height = 60;
            poljeVsebina.TextWrapping = TextWrapping.Wrap;
            poljeVsebina.AcceptsReturn = true;
            poljeVsebina.SetBinding(TextBox.TextProperty, new System.Windows.Data.Binding("IzbranaObjava.Vsebina"));

            DockPanel panelVsebina = new DockPanel();
            panelVsebina.Margin = new Thickness(0, 5, 0, 0);
            panelVsebina.Children.Add(labelVsebina);
            panelVsebina.Children.Add(poljeVsebina);
            glavniPanel.Children.Add(panelVsebina);

            // Polje za avtorja
            Label labelAvtor = new Label();
            labelAvtor.Content = "Avtor:";
            labelAvtor.Width = 100;

            TextBox poljeAvtor = new TextBox();
            poljeAvtor.Height = 25;
            poljeAvtor.SetBinding(TextBox.TextProperty, new System.Windows.Data.Binding("IzbranaObjava.Avtor"));

            DockPanel panelAvtor = new DockPanel();
            panelAvtor.Margin = new Thickness(0, 5, 0, 0);
            panelAvtor.Children.Add(labelAvtor);
            panelAvtor.Children.Add(poljeAvtor);
            glavniPanel.Children.Add(panelAvtor);

            // Gumb za zapiranje
            Button gumbZapri = new Button();
            gumbZapri.Content = "Zapri";
            gumbZapri.Width = 80;
            gumbZapri.Height = 25;
            gumbZapri.HorizontalAlignment = HorizontalAlignment.Right;
            gumbZapri.Margin = new Thickness(0, 15, 0, 0);
            gumbZapri.Click += ZapriOkno;

            glavniPanel.Children.Add(gumbZapri);

            // Dodaj vse v okno
            this.Content = glavniPanel;
        }

        // Posebna metoda za zapiranje okna
        private void ZapriOkno(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}