using Windows.UI.Xaml.Controls;

namespace NewSpartan.Core
{
    public abstract class WebviewRenderer
    {
        // WebviewRenderer constructor, require to have a UWP Page as parameter.
        protected WebviewRenderer(Page page) { }

        public abstract void Initialize();

        public abstract void Update();

        public abstract class WebPage
        {
            public abstract string GetTitle { get; }

            public abstract string GetIcon { get; }
        }

        public abstract class Navigation
        {
            public abstract void SetUrl(string url);

            public abstract void GoBack();

            public abstract void GoForward();

            public abstract bool CanGoBack { get; }

            public abstract bool CanGoForward { get; }
        }
    }
}
