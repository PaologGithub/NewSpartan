using NewSpartan.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page https://go.microsoft.com/fwlink/?LinkId=234236

namespace NewSpartan.App.UI.Controls
{
    public sealed partial class ControlBar : UserControl
    {
        private WebviewRenderer renderer;
        private CancellationTokenSource cts;
        private int requestId = 0;
        private static readonly HttpClient httpClient = new HttpClient();
        private ObservableCollection<string> suggestions = new ObservableCollection<string>();


#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.
        public ControlBar()
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.
        {
            this.InitializeComponent();

            this.Loaded += ControlBar_Loaded;
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0");

            SearchBox.ItemsSource = suggestions;
        }

        private void ControlBar_Loaded(object sender, RoutedEventArgs e)
        {
            renderer = MainPage.Current.GetWebviewRenderer();

            renderer.GetNavigation().URLChanged += ControlBar_URLChanged;
            renderer.GetNavigation().CanGoBackChanged += (s, canGoBack) => BackButton.IsEnabled = canGoBack;
            renderer.GetNavigation().CanGoForwardChanged += (s, canGoForward) => ForwardButton.IsEnabled = canGoForward;
        }

        private ApplicationView appView = ApplicationView.GetForCurrentView();

        private void ControlBar_URLChanged(object? sender, Uri e)
        {
            if (e == null) return;

            if (e.Host.Contains("google.") &&
                e.AbsolutePath == "/search")
            {
#pragma warning disable CS8600 // Conversion de littéral ayant une valeur null ou d'une éventuelle valeur null en type non-nullable.
                string query = HttpUtility.ParseQueryString(e.Query).Get("q");
#pragma warning restore CS8600 // Conversion de littéral ayant une valeur null ou d'une éventuelle valeur null en type non-nullable.

                if (!string.IsNullOrEmpty(query))
                {
                    SearchBox.Text = query;

                    appView.Title = $"Google Search - \"{query}\"";

                    return;
                }
            }

            SearchBox.Text = e.ToString();

            // Will change this later !
            appView.Title = renderer.GetWebPage().GetTitle();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (renderer.GetNavigation().CanGoBack)
            {
                renderer.GetNavigation().GoBack();
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (renderer.GetNavigation().CanGoForward)
            {
                renderer.GetNavigation().GoForward();
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            renderer.GetNavigation().Reload();
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string input = args.QueryText.Trim();

            if (string.IsNullOrEmpty(input)) return;

#pragma warning disable CS8600 // Conversion de littéral ayant une valeur null ou d'une éventuelle valeur null en type non-nullable.
            if (Uri.TryCreate(input, UriKind.Absolute, out Uri uri) && (uri.Scheme == "http" || uri.Scheme == "https"))
            {
                renderer.GetNavigation().SetUrl(uri);
            }
            else
            {
                string searchUrl = $"https://www.google.com/search?q={Uri.EscapeDataString(input)}";
                renderer.GetNavigation().SetUrl(new Uri(searchUrl));
            }
#pragma warning restore CS8600 // Conversion de littéral ayant une valeur null ou d'une éventuelle valeur null en type non-nullable.

            FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
        }

        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;

            var localId = ++requestId;

            cts?.Cancel();
            cts = new CancellationTokenSource();
            var token = cts.Token;

            try { await Task.Delay(200, token); }
            catch { return; }

            string query = sender.Text.Trim();
            if (string.IsNullOrEmpty(query)) return;

            await FetchGoogleSuggestionsAsync(query);
            if (token.IsCancellationRequested || localId != requestId) return;
        }

        private async Task FetchGoogleSuggestionsAsync(string query)
        {
            try
            {
                string url = $"https://suggestqueries.google.com/complete/search?client=firefox&q={Uri.EscapeDataString(query)}";
                string json = await httpClient.GetStringAsync(url);

                using var doc = JsonDocument.Parse(json);
                var array = doc.RootElement[1];

                var newItems = new HashSet<string>();

                foreach (var item in array.EnumerateArray())
                {
                    var value = item.GetString();
                    if (!string.IsNullOrEmpty(value))
                        newItems.Add(value);
                }

                UpdateSuggestions(newItems);
            }
            catch
            {
            }
        }

        private void UpdateSuggestions(HashSet<string> newItems)
        {
            for (int i = suggestions.Count - 1; i >= 0; i--)
            {
                if (!newItems.Contains(suggestions[i]))
                {
                    suggestions.RemoveAt(i);
                }
            }

            foreach (var item in newItems)
            {
                if (!suggestions.Contains(item))
                {
                    suggestions.Add(item);
                }
            }
        }

        private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem.ToString();
        }
    }
}
