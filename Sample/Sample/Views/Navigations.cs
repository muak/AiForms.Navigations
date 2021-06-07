using System;
using Xamarin.Forms;

namespace Sample.Views
{
    public class NaviA : NavigationPage
    {
        public NaviA(RootPage root)
        {
            IconImageSource = "Icon1";
            Title = "Tab1";
        }        
    }

    public class NaviB:NavigationPage
    {
        public NaviB()
        {
            IconImageSource = "Icon1";
            Title = "Tab2";
        }
    }

    public class NaviC : NavigationPage
    {
        public NaviC()
        {
            IconImageSource = "Icon1";
            Title = "Tab3";
        }
    }

    public class NaviD : NavigationPage
    {
        public NaviD()
        {
            IconImageSource = "Icon1";
            Title = "Tab4";
        }
    }

    public class NaviE : NavigationPage
    {
        public NaviE()
        {
            IconImageSource = "Icon1";
            Title = "Tab5";
        }
    }
}
