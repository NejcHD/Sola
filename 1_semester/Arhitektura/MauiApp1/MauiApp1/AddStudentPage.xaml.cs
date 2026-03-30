using MauiApp1.Koda;

namespace MauiApp1;

public partial class AddStudentPage : ContentPage
{
    public AddStudentPage()
    {
        InitializeComponent();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // 1. Ustvariva nov objekt študenta iz vnesenih polj
        var novStudent = new Student
        {
            Ime = EntryIme.Text,
            Priimek = EntryPriimek.Text,
            VpisnaStevilka = EntryVpisna.Text
        };

        // 2. Preveriva, če so polja izpolnjena (osnovna validacija)
        if (string.IsNullOrWhiteSpace(novStudent.Ime) || string.IsNullOrWhiteSpace(novStudent.Priimek))
        {
            await DisplayAlert("Napaka", "Prosim izpolni vsa polja!", "OK");
            return;
        }

        // 3. Pošljeva podatke nazaj (zaenkrat se vrnemo na prejšnjo stran)
        // To bomo kasneje povezali z API-jem
        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Samo zapre stran
    }
}