using NewSpartan.Core;
using System;

namespace NewSpartan.NativeWebviewRenderer
{
    public class NativeWebviewRendererImpl : WebviewRenderer
    {
        private readonly WebRenderer renderer;
        private readonly NativeWebPageImpl webPage;
        private readonly NativeNavigationImpl navigation;

        public NativeWebviewRendererImpl(WebRenderer page) : base(page)
        {
            this.renderer = page;
            this.webPage = new NativeWebPageImpl(renderer);
            this.navigation = new NativeNavigationImpl(renderer);
        }

        public override void Initialize()
        {
            renderer.NativeRenderer.Navigate(new Uri("https://www.google.com"));

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

    public class NativeWebPageImpl : WebPage
    {
        private readonly WebRenderer renderer;

        public NativeWebPageImpl(WebRenderer page)
        {
            this.renderer = page;
        }

        public override void Initialize()
        {
            renderer.NativeRenderer.ContainsFullScreenElementChanged += (s, e) =>
            {
                OnFullscreenChanged(s.ContainsFullScreenElement);
            };
        }

        public override string GetTitle()
        {
            return renderer.NativeRenderer.DocumentTitle;
        }

        public override string GetIcon()
        {
            return renderer.NativeRenderer.Source.Host + "/favicon.ico";
        }
    }

    public class NativeNavigationImpl : Navigation
    {
        private readonly WebRenderer renderer;
        private bool oldCanGoBack = false;
        private bool oldCanGoForward = false;

        public NativeNavigationImpl(WebRenderer page)
        {
            this.renderer = page;
        }

        public override void Initialize()
        {
            renderer.NativeRenderer.NavigationCompleted += (s, e) =>
            {
                OnUrlChanged(renderer.NativeRenderer.Source);

                if (oldCanGoBack != renderer.NativeRenderer.CanGoBack)
                {
                    oldCanGoBack = renderer.NativeRenderer.CanGoBack;
                    OnCanGoBackChanged(oldCanGoBack);
                }
                if (oldCanGoForward != renderer.NativeRenderer.CanGoForward)
                {
                    oldCanGoForward = renderer.NativeRenderer.CanGoForward;
                    OnCanGoForwardChanged(oldCanGoForward);
                }
            };
        }

        public override bool CanGoBack => renderer.NativeRenderer.CanGoBack;
        public override bool CanGoForward => renderer.NativeRenderer.CanGoForward;
        public override void GoBack()
        {
            renderer.NativeRenderer.GoBack();
        }

        public override void GoForward()
        {
            renderer.NativeRenderer.GoForward();
        }

        public override void Reload()
        {
            renderer.NativeRenderer.Refresh();
        }

        public override void SetUrl(Uri url)
        {
            renderer.NativeRenderer.Navigate(url);
        }
    }
}
