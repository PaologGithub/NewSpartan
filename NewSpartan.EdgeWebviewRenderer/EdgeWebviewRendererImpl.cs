using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewSpartan.Core;
using Windows.UI.Xaml.Controls;

namespace NewSpartan.EdgeWebviewRenderer
{
    public class EdgeWebviewRendererImpl : WebviewRenderer
    {
        private WebRenderer renderer;

        public EdgeWebviewRendererImpl(WebRenderer page) : base(page)
        {
            renderer = page;
        }

        public override void Initialize()
        {
            renderer.EdgeRenderer.Source = new Uri("https://www.google.com");
        }

        public override void Update()
        {
            
        }
    }
}
