using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Platform = Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using AiForms.Navigations;

namespace Sample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPage : AiTabbedPage
    {
        public RootPage()
        {           
            InitializeComponent();            
        }
    }
}
