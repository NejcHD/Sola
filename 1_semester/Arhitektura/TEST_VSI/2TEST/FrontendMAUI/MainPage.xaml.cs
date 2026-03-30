using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using FrontendMAUI.Entitete;

namespace FrontendMAUI
{
    public partial class MainPage : ContentPage
    {
        private static readonly HttpClient HttpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        public ObservableCollection<Soba> SeznamSob { get; } = new();

       // public ObservableCollection<Soba> SeznamSob { get; } = new();

        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            _ = InitializeRoomsAsync();
        }

        private async Task InitializeRoomsAsync()
        {
            var rooms = await GetRoomsAsync();
            SeznamSob.Clear();
            foreach (var room in rooms)
            {
                SeznamSob.Add(room);
            }
        }

        private static async Task<List<Soba>> GetRoomsAsync()
        {
            using var response = await HttpClient.GetAsync("/sobe");
            response.EnsureSuccessStatusCode();
            await using var stream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return await JsonSerializer.DeserializeAsync<List<Soba>>(stream, options) ?? new List<Soba>();
        }

    }

}
