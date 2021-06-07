using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiForms.Navigations;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyTabbedPage : AiTabbedPage
    {
        public MyTabbedPage()
        {
            InitializeComponent();
        }
    }
}
