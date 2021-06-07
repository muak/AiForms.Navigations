using System;
using System.Collections.Generic;
using System.Linq;
using AiForms.Navigations;
using AiForms.Navigations.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AiNavigationPage), typeof(AiNavigationPageRenderer))]
namespace AiForms.Navigations.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class AiNavigationPageRenderer: NavigationRenderer
    {
        public AiNavigationPageRenderer()
        {
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnPropertyChanged;
            }
            if (e.NewElement != null)
            {
                e.NewElement.PropertyChanged += OnPropertyChanged;

            }
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == NavigationPage.BarBackgroundColorProperty.PropertyName)
            {
                SetNeedsStatusBarAppearanceUpdate();
                PreferredStatusBarStyle();
                SetNeedsStatusBarAppearanceUpdate();
            }
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            var navigationPage = Element as NavigationPage;
            return navigationPage.BarBackgroundColor.Luminosity >= 0.5 ? UIStatusBarStyle.DarkContent : UIStatusBarStyle.LightContent;
        }

        public override void PushViewController(UIViewController viewController, bool animated)
        {
            base.PushViewController(viewController, animated);

            var curPage = (Element as NavigationPage).CurrentPage;
            foreach (var item in curPage.ToolbarItems.OfType<AiToolbarItem>())
            {
                item.PropertyChanged += OnToolbarItemPropertyChanged;
            }
            SetToolbarItemVisibility();
        }

        public override UIViewController PopViewController(bool animated)
        {
            var curPage = (Element as NavigationPage).CurrentPage;
            foreach (var item in curPage.ToolbarItems.OfType<AiToolbarItem>())
            {
                item.PropertyChanged -= OnToolbarItemPropertyChanged;
            }

            return base.PopViewController(animated);
        }


        void SetToolbarItemVisibility()
        {
            var curPage = (Element as NavigationPage).CurrentPage;

            if (!curPage.ToolbarItems.OfType<AiToolbarItem>().Any())
            {
                return;
            }

            var ctrl = ViewControllers.Last();
            if (ctrl.NavigationItem.RightBarButtonItems != null)
            {
                for (var i = 0; i < ctrl.NavigationItem.RightBarButtonItems.Length; i++)
                    ctrl.NavigationItem.RightBarButtonItems[i].Dispose();
            }
            if (ToolbarItems != null)
            {
                for (var i = 0; i < ToolbarItems.Length; i++)
                    ToolbarItems[i].Dispose();
            }

            List<UIBarButtonItem> primaries = null;
            List<UIBarButtonItem> secondaries = null;

            foreach (var item in curPage.ToolbarItems.OrderBy(x => x.Priority))
            {
                if (item is AiToolbarItem aiItem)
                {
                    if (!aiItem.IsVisible)
                        continue;
                }

                if (item.Order == ToolbarItemOrder.Secondary)
                    (secondaries = secondaries ?? new List<UIBarButtonItem>()).Add(item.ToUIBarButtonItem(true));
                else
                    (primaries = primaries ?? new List<UIBarButtonItem>()).Add(item.ToUIBarButtonItem());
            }

            if (primaries != null)
                primaries.Reverse();
            ctrl.NavigationItem.SetRightBarButtonItems(primaries == null ? new UIBarButtonItem[0] : primaries.ToArray(), false);
            ToolbarItems = secondaries == null ? new UIBarButtonItem[0] : secondaries.ToArray();

            UpdateToolBarVisible();
        }

        void UpdateToolBarVisible()
        {
            if (!(View is UIToolbar secondaryToolbar))
            {
                return;
            }

            if (secondaryToolbar == null)
                return;
            if (TopViewController != null && TopViewController.ToolbarItems != null && TopViewController.ToolbarItems.Any())
            {
                secondaryToolbar.Hidden = false;
                secondaryToolbar.Items = TopViewController.ToolbarItems;
            }
            else
            {
                secondaryToolbar.Hidden = true;
            }

            TopViewController?.NavigationItem?.TitleView?.SizeToFit();
            TopViewController?.NavigationItem?.TitleView?.LayoutSubviews();
        }

        private void OnToolbarItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == AiToolbarItem.IsVisibleProperty.PropertyName)
            {
                SetToolbarItemVisibility();
            }
        }
    }
}
