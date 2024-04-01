using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Net.Http;
using BingeTracker.Model;
using System.Text.Json;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BingeTracker.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditStreamPage : Page
    {
        private Streaming _selectedStream;
        public EditStreamPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is Streaming selectedStream)
            {
                _selectedStream = selectedStream;

                var url = $"https://localhost:7132/api/Streamings/{selectedStream.Id}";
                var client = new HttpClient();
                var reponse = await client.GetAsync(url);
                var content = await reponse.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                _selectedStream = JsonSerializer.Deserialize<Streaming>(content, options);


                TitleTB.Text = _selectedStream.Title;
                PlatformTB.Text = _selectedStream.Platform;
                GenreTB.Text = _selectedStream.Genre;
                RatingTb.Text = _selectedStream.Rating.ToString();
                ReleaseDate.Date = DateTime.Now.Date; // Set only the date pa
            }
        }

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private async void UpdateStream_Click(object sender, RoutedEventArgs e)
        {// Haal de nieuwe waarden op uit de invoervelden
            string newTitle = TitleTB.Text;
            string newPlatform = PlatformTB.Text;
            string newGenre = GenreTB.Text;
            string newRating = RatingTb.Text;
            DateOnly newReleaseDate = DateOnly.Parse(ReleaseDate.SelectedDate.ToString().Split(" ")[0]); // ReleaseDate is een DatePicker

            // Controleer of de ingevoerde rating een geldig getal is
            if (!int.TryParse(newRating, out int ratingValue))
            {
                // Toon een foutmelding als de ingevoerde rating geen geldig getal is
                await EditDialog.ShowAsync();
                return;
            }

            // Maak een object aan met de nieuwe gegevens
            var updatedStream = new Streaming
            {
                Id = _selectedStream.Id,
                Title = newTitle,
                Platform = newPlatform,
                Genre = newGenre,
                ReleaseDate = newReleaseDate,
                Rating = int.Parse(newRating)
            };

            // URL voor het bijwerken van de stream op basis van zijn ID
            var url = $"https://localhost:7132/api/Streamings/{updatedStream.Id}";

            // Initialiseer een HttpClient
            var client = new HttpClient();

            // Serialiseer het updatedStream-object naar JSON
            var streamContent = new StringContent(JsonSerializer.Serialize(updatedStream), Encoding.UTF8, "application/json");

            // Stuur een PUT-verzoek naar de server met de bijgewerkte gegevens
            var response = await client.PutAsync(url, streamContent);

            if (response.IsSuccessStatusCode)
            {
                // Als het verzoek met succes is voltooid, navigeer terug naar de hoofdpagina
                this.Frame.Navigate(typeof(MainPage));
            }
            else
            {
                // Als er een fout optreedt, toon een foutmelding aan de gebruiker
                EditDialog.Content = "Er is een fout opgetreden bij het bijwerken van de stream.";
                await EditDialog.ShowAsync();
            }
        }
    }
}
