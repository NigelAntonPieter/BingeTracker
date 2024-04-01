using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using BingeTracker.Model;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BingeTracker.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewStreamPage : Page
    {
        public NewStreamPage()
        {
            this.InitializeComponent();
        }

        private async void saveNewStreamer_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleTB.Text.Trim();
            var platform = platformTB.Text.Trim();
            var genre = genreTB.Text.Trim();

            // Controleer of een datum is geselecteerd
            if (ReleaseDate.SelectedDate == null)
            {
                // Toon een foutmelding als er geen datum is geselecteerd
                await ErrorMessage.ShowAsync();
                return;
            }

            // Probeer de geselecteerde datum te converteren naar een DateOnly object
            if (!DateOnly.TryParse(ReleaseDate.SelectedDate.Value.ToString("yyyy-MM-dd"), out DateOnly releasedate))
            {
                // Toon een foutmelding als de datum niet kan worden geconverteerd
                await ErrorMessage.ShowAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(platform) ||
                string.IsNullOrWhiteSpace(genre) ||
                string.IsNullOrWhiteSpace(ratingTB.Text))
            {
                // Toon een foutmelding als een van de velden leeg is
                await ErrorMessage.ShowAsync();
                return;
            }

            bool isTitleValid = !int.TryParse(title, out _);
            bool isPlatformValid = !int.TryParse(platform, out _);
            bool isGenreValid = !int.TryParse(genre, out _);
            bool isRatingValid = int.TryParse(ratingTB.Text, out int rating);

            // Controleer of de ingevoerde waarden geldig zijn
            if (isTitleValid && isPlatformValid && isGenreValid && isRatingValid)
            {
                // Controleer of de rating een geldig getal is
                if (!isRatingValid)
                {
                    // Toon een foutmelding als de rating geen geldig getal is
                    await ErrorMessage.ShowAsync();
                    return;
                }

                var url = "https://localhost:7132/api/Streamings";
                var client = new HttpClient();

                var newStream = new Streaming
                {
                    Title = title,
                    Platform = platform,
                    Genre = genre,
                    ReleaseDate = releasedate,
                    Rating = rating,
                };

                var jsonContent = JsonSerializer.Serialize(newStream);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                var contents = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                var answerJson = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                this.Frame.Navigate(typeof(MainPage));
            }
            else
            {
                // Toon een foutmelding als de invoer ongeldig is
                await ErrorMessage.ShowAsync();
            }
        }

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
