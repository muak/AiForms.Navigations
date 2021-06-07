using System.Collections.Generic;
using System.Reflection;
using AiForms.Navigations;
using AiForms.Navigations.Droid;
using Android.Content;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using XF = Xamarin.Forms;

[assembly: XF.ExportRenderer(typeof(AiTabbedPage), typeof(AiTabbedPageRenderer))]
namespace AiForms.Navigations.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
	public class AiTabbedPageRenderer: TabbedPageRenderer, BottomNavigationView.IOnNavigationItemSelectedListener
	{
		AiTabbedPage _aiTabbed;
        BottomNavigationView _bottomNavigationView;
		Dictionary<IMenuItem, int> _menuDict = new Dictionary<IMenuItem, int>();

        public AiTabbedPageRenderer(Context context):base(context)
        {
        }

		protected override void OnElementChanged(ElementChangedEventArgs<XF.TabbedPage> e)
		{
            Element.OnThisPlatform().SetToolbarPlacement(ToolbarPlacement.Bottom);

            base.OnElementChanged(e);

            var fieldInfo = typeof(TabbedPageRenderer).GetField("_bottomNavigationView", BindingFlags.Instance | BindingFlags.NonPublic);
            _bottomNavigationView = (BottomNavigationView)fieldInfo.GetValue(this);          

            if (e.NewElement != null)
			{
				_aiTabbed = Element as AiTabbedPage;

                _bottomNavigationView.SetOnNavigationItemSelectedListener(this);

                if (!_aiTabbed.TabBorderColor.IsDefault)
                {
                    var layout = _bottomNavigationView.Parent as RelativeLayout;
                    var border = new View(Context);
                    border.SetBackgroundColor(_aiTabbed.TabBorderColor.ToAndroid());
                    border.Alpha = 0.6f;
                    using (var param = new RelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent)
                    {
                        Height = (int)Context.ToPixels(0.5)
                    })
                    {
                        param.AddRule(LayoutRules.Above, _bottomNavigationView.Id);
                        layout.AddView(border, param);
                    }
                }
                

                for (var i = 0; i < _aiTabbed.Children.Count; i++)
                {
                    var item = _bottomNavigationView.Menu.GetItem(i);
                    _menuDict.Add(item, i);
                }

                Element.OnThisPlatform().SetIsSmoothScrollEnabled(false);
                Element.OnThisPlatform().SetIsSwipePagingEnabled(false);
                _bottomNavigationView.SetShiftMode(false, false, _aiTabbed);

                if(_aiTabbed.CurrentPage is XF.NavigationPage navitagionPage)
                {
                    SetStatusBarColor(navitagionPage);
                }
            }			
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
				_bottomNavigationView.SetOnNavigationItemSelectedListener(null);
				_aiTabbed = null;
				_bottomNavigationView = null;
				_menuDict.Clear();
				_menuDict = null;
            }
            base.Dispose(disposing);
        }

        bool BottomNavigationView.IOnNavigationItemSelectedListener.OnNavigationItemSelected(IMenuItem item)
        {
            var baseRet = base.OnNavigationItemSelected(item);
            if (!baseRet)
            {
                return false;
            }

            if (_menuDict.TryGetValue(item, out var index))
            {
                if (_aiTabbed.Children[index] is XF.NavigationPage navigationPage)
                {
                    SetStatusBarColor(navigationPage);                   
                }
            }

            return true;
        }

        void SetStatusBarColor(XF.NavigationPage navigationPage)
		{
			var statusBarColor = navigationPage.BarBackgroundColor;
			if (!statusBarColor.IsDefault)
			{
				var window = (Context as FormsAppCompatActivity).Window;
				window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
				window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
				window.SetStatusBarColor(statusBarColor.ToAndroid());
			}
		}
	}
}
