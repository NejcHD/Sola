using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Vaja
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Ustvari ViewModel in nastavi kot DataContext
            this.DataContext = new ViewModel();
        }

        private void Izhod_Program(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }



    public class RelayCommand : ICommand
    {
        private Action _execute;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute();
        public event EventHandler CanExecuteChanged;
    }


    // VIEWMODEL - nova dodana klasa
    public class ViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<int> Številke { get; set; }
        public ICommand DodajŠtevilkoCommand { get; }
        public ICommand OdstraniŠtevilkoCommand { get; }
        public ICommand PodvojiCommand { get; }

        private int? _izbranaStevilka;  // Uporabi int? da lahko je null
        public int? IzbranaStevilka
        {
            get => _izbranaStevilka;
            set { _izbranaStevilka = value; OnPropertyChanged(nameof(IzbranaStevilka)); }
        }

        private void DodajŠtevilko()
        {
            Številke.Add(Številke.Count + 1);
        }

        private void OdstraniŠtevilko()
        {
            if (Številke.Count > 0)
            {
                Številke.RemoveAt(Številke.Count - 1);
            }
        }

        private void PodvojiStevilko()
        {
            if (IzbranaStevilka.HasValue && Številke.Contains(IzbranaStevilka.Value))
            {
                // Najdi indeks izbrane številke
                int index = Številke.IndexOf(IzbranaStevilka.Value);

                // Podvoji vrednost
                int novaVrednost = IzbranaStevilka.Value * 2;

                // Zamenjaj staro številko z novo
                Številke[index] = novaVrednost;

                // Posodobi izbrano številko na novo vrednost
                IzbranaStevilka = novaVrednost;
            }
        }





        private ObservableCollection<Objava> _objave;
        private Objava _izbranaObjava;

        public ObservableCollection<Objava> Objave
        {
            get => _objave;
            set { _objave = value; OnPropertyChanged(nameof(Objave)); }
        }

        public Objava IzbranaObjava
        {
            get => _izbranaObjava;
            set { _izbranaObjava = value; OnPropertyChanged(nameof(IzbranaObjava)); }
        }

        public ViewModel()
        {

            Številke = new ObservableCollection<int>();
            DodajŠtevilkoCommand = new RelayCommand(DodajŠtevilko);
            OdstraniŠtevilkoCommand = new RelayCommand(OdstraniŠtevilko);

            // Inicializacija seznama objav
            Objave = new ObservableCollection<Objava>
            {
                new Objava
                {
                    Ime = "Nejc",
                    Priimek = "Prezelj",
                    Datum = DateTime.Now,
                    Opis = "TO jes testna objava",
                    Starost = 21
                },
                new Objava
                {
                    Ime = "test",
                    Priimek = "ssssss",
                    Datum = DateTime.Now,
                    Opis = "TO sa",
                    Starost = 2221
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

  






    public class Objava : INotifyPropertyChanged
    {
        private string _ime;
        public string Ime
        {
            get { return _ime; }
            set { _ime = value; OnPropertyChanged(nameof(Ime)); }
        }

        private string _priimek;
        public string Priimek
        {
            get { return _priimek; }
            set { _priimek = value; OnPropertyChanged(nameof(Priimek)); }
        }

        private DateTime _datum;
        public DateTime Datum
        {
            get { return _datum; }
            set { _datum = value; OnPropertyChanged(nameof(Datum)); }
        }

        private string _opis;
        public string Opis
        {
            get { return _opis; }
            set { _opis = value; OnPropertyChanged(nameof(Opis)); }
        }

        private int _starost;
        public int Starost
        {
            get { return _starost; }
            set { _starost = value; OnPropertyChanged(nameof(Starost)); }
        }


        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}