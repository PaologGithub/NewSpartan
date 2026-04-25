using NewSpartan.Core;
using System;
using System.Collections.Generic;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NewSpartan.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a <see cref="Frame">.
    /// </summary>
    public sealed partial class MainPage : Page
    {
#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.
        public static MainPage Current { get; private set; }
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur autre que Null lors de la fermeture du constructeur. Envisagez d’ajouter le modificateur « required » ou de déclarer le champ comme pouvant accepter la valeur Null.

        private readonly WebviewRenderer CurrentWebviewRenderer;

        public MainPage()
        {
            Current = this;

            InitializeComponent();

            CurrentWebviewRenderer = new EdgeWebviewRenderer.EdgeWebviewRendererImpl(WebRendererControl);
            Initialize();
        }

        async void Initialize()
        {
            WebRendererControl.EdgeRenderer.CoreWebView2Initialized += (s, e) =>
            {
                CurrentWebviewRenderer.Initialize();
            };
            CurrentWebviewRenderer.GetWebPage().FullscreenChanged += (sender, fullscreen) =>
            {
                ApplicationView view = ApplicationView.GetForCurrentView();
                if (fullscreen)
                {
                    if (view.TryEnterFullScreenMode())
                    {
                        GoFullscreen();
                    } else
                    {
                        ExitFullscreen();
                    };
                } else
                {
                    view.ExitFullScreenMode();

                    ExitFullscreen();
                }
            };

             await WebRendererControl.EdgeRenderer.EnsureCoreWebView2Async();
        }

        private void GoFullscreen()
        {
            TitlebarControl.Visibility = Visibility.Collapsed;
            ControlBarControl.Visibility = Visibility.Collapsed;

            TitlebarRow.Height = new GridLength(0);
            ControlBarRow.Height = new GridLength(0);
        }

        private void ExitFullscreen()
        {
            TitlebarControl.Visibility = Visibility.Visible;
            ControlBarControl.Visibility = Visibility.Visible;

            TitlebarRow.Height = new GridLength(32);
            ControlBarRow.Height = new GridLength(40);
        }

        public WebviewRenderer GetWebviewRenderer()
        {
            return CurrentWebviewRenderer;
        }
    }
}
