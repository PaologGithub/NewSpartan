using NewSpartan.Core;
using Windows.UI.Xaml.Controls;

namespace NewSpartan.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a <see cref="Frame">.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WebviewRenderer CurrentWebviewRenderer;

        public MainPage()
        {
            InitializeComponent();

            CurrentWebviewRenderer = new EdgeWebviewRenderer.EdgeWebviewRendererImpl(WebRenderer);
            
            Initialize();
        }

        public void Initialize()
        {
            CurrentWebviewRenderer.Initialize();
        }
    }
}
