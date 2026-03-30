using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UP_KOLOKVIJ1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MessageBox.Show("HaLLO NIGA", "HEL=)O");
            // NAMIG 1: Prikažite sporočilo ob zagonu aplikacije
            // Uporabite: MessageBox.Show("tekst", "naslov", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // NAMIG 2: Ko kliknete na Gumb 1, spremenite status tekst
        private void Gumb1_Click(object sender, RoutedEventArgs e)
        {
            // Uporabite: StatusText.Text = "Nov status";
            // Uporabite: MessageBox.Show("Gumb 1 je bil kliknjen!");

            StatusText.Text = "SPREMBA WAWAWAWAWAW";
            MessageBox.Show("SPREMEBA ", "HELasdasdasdasdsadsaO");
        }

        private void Gumb2_Click(object sender, RoutedEventArgs e)
        {
            // NAMIG 3: Spremenite barvo status bara
            // Uporabite: StatusText.Foreground = Brushes.Red;

            StatusText.Background = Brushes.Red;
        }

        private void Gumb3_Click(object sender, RoutedEventArgs e)
        {
            // NAMIG 4: Prikažite trenutni čas
            // Uporabite: DateTime.Now.ToString("HH:mm:ss");

            StatusText.Text = DateTime.Now.ToString("dd:mm:yyyy hh:mm:ss");
        }

        // NAMIG 5: Ko se spremeni tekst v TextBox, posodobite status
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            // Uporabite: StatusText.Text = "Vnesli ste: " + textBox.Text;

          // StatusText.Text = "VNSELI STE TEXT" + textBox.Text;
        }

        // NAMIG 6: Vprašajte uporabnika, ko želi zapreti aplikacijo
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Uporabite: MessageBoxResult result = MessageBox.Show("Ali ste prepričani?", ...);
            // Če je result == MessageBoxResult.No, potem: e.Cancel = true;

            
        }
    }
}