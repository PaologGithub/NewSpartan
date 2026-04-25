using System;
using Windows.UI.Xaml.Controls;

namespace NewSpartan.Core
{
    public abstract class WebviewRenderer
    {
        // WebviewRenderer constructor, require to have a UWP Page as parameter.
        protected WebviewRenderer(Page page) { }

        public abstract void Initialize();

        public abstract void Update();

        public abstract WebPage GetWebPage();

        public abstract Navigation GetNavigation();
    }

    public abstract class WebPage
    {
        public abstract void Initialize();

        public abstract string GetTitle();

        public abstract string GetIcon();


        public event EventHandler<bool> FullscreenChanged;

        protected void OnFullscreenChanged(bool isFullscreen)
        {
            FullscreenChanged?.Invoke(this, isFullscreen);
        }
    }

    public abstract class Navigation
    {
        public abstract void Initialize();

        public abstract void SetUrl(Uri url);

        public abstract void GoBack();

        public abstract void GoForward();

        public abstract void Reload();

        public abstract bool CanGoBack { get; }

        public abstract bool CanGoForward { get; }


        public event EventHandler<Uri> URLChanged;

        protected void OnUrlChanged(Uri uri)
        {
            URLChanged?.Invoke(this, uri);
        }

        public EventHandler<bool> CanGoBackChanged;
        public EventHandler<bool> CanGoForwardChanged;

        protected void OnCanGoBackChanged(bool canGoBack)
        {
            CanGoBackChanged?.Invoke(this, canGoBack);
        }

        protected void OnCanGoForwardChanged(bool canGoForward)
        {
            CanGoForwardChanged?.Invoke(this, canGoForward);
        }
    }
}
