using NewSpartan.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page https://go.microsoft.com/fwlink/?LinkId=234236

namespace NewSpartan.App.UI.Controls
{
    public sealed partial class Titlebar : UserControl
    {
        private WebviewRenderer renderer;
        private readonly BitmapImage bitmapImage = new BitmapImage();

#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.
        public Titlebar()
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.
        {
            this.InitializeComponent();
            this.Loaded += Titlebar_Loaded;

            CoreApplicationViewTitleBar titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = true;
        }

        private void Titlebar_Loaded(object sender, RoutedEventArgs e)
        {
            renderer = MainPage.Current.GetWebviewRenderer();
            renderer.GetNavigation().URLChanged += Titlebar_URLChanged;

            Window.Current.SetTitleBar(DragRegion);

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;

            ShowFallbackIcon();
            Icon.Source = bitmapImage;
            bitmapImage.ImageFailed += (s, e) => ShowFallbackIcon();
        }

        private void Titlebar_URLChanged(object? sender, Uri e)
        {
            Title.Text = renderer.GetWebPage().GetTitle();
            try
            {
                bitmapImage.UriSource = new Uri(renderer.GetWebPage().GetIcon());
            } catch
            {
                ShowFallbackIcon();
            }
        }
        private void ShowFallbackIcon()
        {
            bitmapImage.UriSource = new Uri("ms-appx:///Assets/StoreLogo.png");
        }
    }
}
