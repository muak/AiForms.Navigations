using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AiForms.Navigations;
using AiForms.Navigations.Droid;
using Android.Content;
using AndroidX.AppCompat.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(AiNavigationPage), typeof(AiNavigationPageRenderer))]
namespace AiForms.Navigations.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class AiNavigationPageRenderer : NavigationPageRenderer
    {
        Toolbar _toolbar;

        public AiNavigationPageRenderer(Context context):base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                Element.Popped -= Element_Popped;
            }

            if (e.NewElement != null)
            {
                _toolbar = (Toolbar)GetChildAt(0);
                Element.Popped += Element_Popped;
                if (Element.Parent is Xamarin.Forms.Application)
                {
                    UpdateBackgroundColor();
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if(e.PropertyName == NavigationPage.BarBackgroundColorProperty.PropertyName)
            {
                if (Element.Parent is Xamarin.Forms.Application)
                {
                    UpdateBackgroundColor();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Element != null)
                {
                    Element.Popped -= Element_Popped;
                }
                _toolbar = null;
            }
            base.Dispose(disposing);
        }


        public void UpdateStatusBarColor()
        {
            Color tintColor = Element.BarBackgroundColor;

            if (!tintColor.IsDefault)
            {
                var window = (Context as FormsAppCompatActivity).Window;
                window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
                window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
                window.SetStatusBarColor(tintColor.ToAndroid());
            }                     
        }        

        private void Element_Popped(object sender, NavigationEventArgs e)
        {
            SetToolbarItemVisibility();
        }

        protected override Task<bool> OnPushAsync(Page view, bool animated)
        {
            SetToolbarItemVisibility();
            return base.OnPushAsync(view, animated);
        }

        protected override void OnToolbarItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnToolbarItemPropertyChanged(sender, e);
            if (e.PropertyName == AiToolbarItem.IsVisibleProperty.PropertyName)
            {
                SetToolbarItemVisibility();
            }
        }

        void SetToolbarItemVisibility()
        {
            var curPage = Element.CurrentPage;

            if (!curPage.ToolbarItems.OfType<AiToolbarItem>().Any())
            {
                return;
            }

            var toolbarItems = curPage.ToolbarItems.OrderBy(x => x.Priority).ToList();

            for (var i = 0; i < toolbarItems.Count; i++)
            {
                var item = toolbarItems[i];
                var menuItem = _toolbar.Menu.GetItem(i);

                if (item is AiToolbarItem aiToolbarItem)
                {
                    menuItem.SetVisible(aiToolbarItem.IsVisible);
                }
            }
        }
    }
}
