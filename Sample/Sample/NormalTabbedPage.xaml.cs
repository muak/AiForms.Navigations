using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Platform = Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Sample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NormalTabbedPage : Xamarin.Forms.TabbedPage
    {
        public NormalTabbedPage()
        {
            InitializeComponent();
            this.On<Platform.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            this.On<Platform.Android>().SetIsSmoothScrollEnabled(false);
            this.On<Platform.Android>().SetIsSwipePagingEnabled(false);
        }
    }
}
