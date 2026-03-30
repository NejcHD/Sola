using System.Collections.ObjectModel;
using MauiApp1.Koda;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        // Nujno mora pisati: public ObservableCollection
        public ObservableCollection<Student> Studenti { get; set; } = new ObservableCollection<Student>();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void OnOsveziClicked(object sender, EventArgs e)
        {

            await DisplayAlert("Test", "Gumb deluje!", "OK");
            try
            {
                using var client = new HttpClient();

                
                var url = "https://localhost:7237/api/Student";

                var response = await client.GetStringAsync(url);

                // Pretvorba besedila iz interneta v seznam študentov
                var podatki = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Student>>(response);

                Studenti.Clear();
                if (podatki != null)
                {
                    foreach (var s in podatki)
                    {
                        Studenti.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                // Če pride do napake (npr. certifikat), se izpiše tukaj
                await DisplayAlert("Napaka", "Ni se mogoče povezati: " + ex.Message, "OK");
            }
        }
        private async void OnPojdiNaDodajanjeClicked(object sender, EventArgs e)
        {
            // To ukaže aplikaciji, naj odpre novo stran AddStudentPage
            await Navigation.PushAsync(new AddStudentPage());
        }
    }
}