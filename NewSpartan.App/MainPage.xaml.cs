using NewSpartan.Core;
using System;
using System.Collections.Generic;
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

            CurrentWebviewRenderer = new EdgeWebviewRenderer.EdgeWebviewRendererImpl(WebRenderer);
            Initialize();
        }

        async void Initialize()
        {
            WebRenderer.EdgeRenderer.CoreWebView2Initialized += (s, e) =>
            {
                CurrentWebviewRenderer.Initialize();
            };

             await WebRenderer.EdgeRenderer.EnsureCoreWebView2Async();
        }

        public WebviewRenderer GetWebviewRenderer()
        {
            return CurrentWebviewRenderer;
        }
    }
}
