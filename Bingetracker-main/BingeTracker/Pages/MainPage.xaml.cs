using BingeTracker.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BingeTracker.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<Streaming> _allStreams;
        public MainPage()
        {
            this.InitializeComponent();
            
            loadBingeTracker();
        }

        public async void loadBingeTracker()
        {
            var url = "https://localhost:7132/api/Streamings";
            var client = new HttpClient();
            var reponse = await client.GetAsync(url);
            var content = await reponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var list = JsonSerializer.Deserialize <List<Streaming>>(content, options);

            FilmListView.ItemsSource= list;

            _allStreams = list;

        }

        private void NewStream_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewStreamPage));
        }

        private async void FilmListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedStream = e.ClickedItem as Streaming;
            if (selectedStream != null)
            {
                this.Frame.Navigate(typeof(EditStreamPage), selectedStream);
            }
        }



        private async void FilmListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            // Get the clicked item from the event arguments
            var clickedItem = (FrameworkElement)e.OriginalSource;

            // Assuming the DataContext of the clicked item is of type Streaming
            var selectedStream = clickedItem.DataContext as Streaming;

            if (selectedStream != null)
            {
                var url = $"https://localhost:7132/api/Streamings/{selectedStream.Id}";

                var client = new HttpClient();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var response = await client.DeleteAsync(url);
                response.EnsureSuccessStatusCode(); // Ensure HTTP status code is success

                // Optionally, you can update your local list after successful deletion
                var list = FilmListView.ItemsSource as List<Streaming>;
                    if (list != null)
                    {   
                        list.Remove(selectedStream);
                        FilmListView.ItemsSource = list;
                    }

                    this.Frame.Navigate(typeof (MainPage));
            }
        }

        private void searchTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchInput = searchTextbox.Text.ToLower();

            FilmListView.ItemsSource = _allStreams.Where(s => s.Title.ToLower().Contains(searchInput) || s.Platform.ToLower(). Contains(searchInput) || s.Genre.ToLower(). Contains(searchInput));
                

        }
    }
}
