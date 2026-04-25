using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewSpartan.Core;
using Windows.UI.Xaml.Controls;

namespace NewSpartan.EdgeWebviewRenderer
{
    public class EdgeWebviewRendererImpl : WebviewRenderer
    {
        private readonly WebRenderer renderer;
        private readonly EdgeWebPageImpl webPage;
        private readonly EdgeNavigationImpl navigation;

        public EdgeWebviewRendererImpl(WebRenderer page) : base(page)
        {
            renderer = page;
            webPage = new EdgeWebPageImpl(page);
            navigation = new EdgeNavigationImpl(page);
        }

        public override void Initialize()
        {
            renderer.EdgeRenderer.CoreWebView2.Navigate("https://www.google.com");

            webPage.Initialize();
            navigation.Initialize();
        }

        public override void Update()
        {

        }

        public override Navigation GetNavigation()
        {
            return navigation;
        }

        public override WebPage GetWebPage()
        {
            return webPage;
        }
    }

    public class EdgeWebPageImpl : WebPage
    {
        private readonly WebRenderer renderer;

        public EdgeWebPageImpl(WebRenderer page)
        {
            renderer = page;
        }

        public override void Initialize()
        {
            renderer.EdgeRenderer.CoreWebView2.ContainsFullScreenElementChanged += (s, e) =>
            {
                OnFullscreenChanged(s.ContainsFullScreenElement);
            };
        }

        public override string GetTitle()
        {
            return renderer.EdgeRenderer.CoreWebView2.DocumentTitle;
        }

        public override string GetIcon()
        {
            return renderer.EdgeRenderer.CoreWebView2.FaviconUri;
        }
    }

    public class EdgeNavigationImpl : Navigation
    {
        private readonly WebRenderer renderer;
        private bool oldCanGoBack = false;
        private bool oldCanGoForward = false;

        public EdgeNavigationImpl(WebRenderer page)
        {
            renderer = page;
        }

        public override void Initialize()
        {
            renderer.EdgeRenderer.NavigationCompleted += (s, e) =>
            {
                OnUrlChanged(renderer.EdgeRenderer.Source);

                if (oldCanGoBack != renderer.EdgeRenderer.CanGoBack)
                {
                    oldCanGoBack = renderer.EdgeRenderer.CanGoBack;
                    OnCanGoBackChanged(oldCanGoBack);
                }
                if (oldCanGoForward != renderer.EdgeRenderer.CanGoForward)
                {
                    oldCanGoForward = renderer.EdgeRenderer.CanGoForward;
                    OnCanGoForwardChanged(oldCanGoForward);
                }
            };
        }

        public override bool CanGoBack => renderer.EdgeRenderer.CanGoBack;
        public override bool CanGoForward => renderer.EdgeRenderer.CanGoForward;
        public override void GoBack()
        {
            renderer.EdgeRenderer.GoBack();
        }

        public override void GoForward()
        {
            renderer.EdgeRenderer.GoForward();
        }

        public override void Reload()
        {
            renderer.EdgeRenderer.Reload();
        }

        public override void SetUrl(Uri url)
        {
            renderer.EdgeRenderer.CoreWebView2.Navigate(url.ToString());
        }

    }
}