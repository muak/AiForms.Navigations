using System;
using System.Threading.Tasks;
using AiForms.Navigations;
using AiForms.Navigations.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AiTabbedPage), typeof(AiTabbedPageRenderer))]
namespace AiForms.Navigations.iOS
{
	[Foundation.Preserve(AllMembers = true)]
	public class AiTabbedPageRenderer: TabbedRenderer
    {
        public AiTabbedPageRenderer()
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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Element.PropertyChanged -= OnPropertyChanged;
			}
			base.Dispose(disposing);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			SetUpTab();
		}

		private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == TabbedPage.SelectedTabColorProperty.PropertyName ||
				e.PropertyName == TabbedPage.UnselectedTabColorProperty.PropertyName ||
				e.PropertyName == AiTabbedPage.SelectedTextColorProperty.PropertyName ||
				e.PropertyName == AiTabbedPage.UnSelectedTextColorProperty.PropertyName ||
				e.PropertyName == AiTabbedPage.BarTextColorProperty.PropertyName ||
				e.PropertyName == AiTabbedPage.BarBackgroundColorProperty.PropertyName ||
				e.PropertyName == AiTabbedPage.IsTextHiddenProperty.PropertyName)
			{
				SetUpTab();
			}
		}

		void SetUpTab()
		{
			var aiTabbed = Element as AiTabbedPage;

			var selectedTextColor = aiTabbed.BarTextColor.ToUIColor();
			if(aiTabbed.SelectedTextColor != Color.Default)
            {
				selectedTextColor = aiTabbed.SelectedTextColor.ToUIColor();
            }

			var unselectedTextColor = aiTabbed.BarTextColor.ToUIColor();
			if (aiTabbed.UnSelectedTextColor != Color.Default)
			{
				unselectedTextColor = aiTabbed.UnSelectedTextColor.ToUIColor();
			}

			var isFontSizeChange = aiTabbed.TabFontSize != -1d;
			var font = UIFont.SystemFontOfSize((float)aiTabbed.TabFontSize);

			for (var i = 0; i < Tabbed.Children.Count; i++)
			{
				Page child = Tabbed.Children[i];
				//var item = aiTabbed.TabItems[i];

				var vc = Platform.GetRenderer(child).ViewController;

				//if (item.Icon != null)
				//            {
				//	var icons = await LoadIcon(item, aiTabbed);
				//	vc.TabBarItem.SetFinishedImages(icons.selctedIcon, icons.unselectedIcon);
				//}

				//vc.TabBarItem.Title = item.Title;

				var selectedTextAttr = new UITextAttributes
				{
					TextColor = selectedTextColor
				};
				var unselectedTextAttr = new UITextAttributes
				{
					TextColor = unselectedTextColor
				};
                if (isFontSizeChange)
                {
					selectedTextAttr.Font = font;
					unselectedTextAttr.Font = font;
				}				

				vc.TabBarItem.SetTitleTextAttributes(
					selectedTextAttr,
					UIControlState.Selected
				);
				vc.TabBarItem.SetTitleTextAttributes(
					unselectedTextAttr,
					UIControlState.Normal
				);		

				if (aiTabbed.IsTextHidden)
				{
					vc.TabBarItem.Title = null;
					vc.TabBarItem.ImageInsets = new UIEdgeInsets(6, 0, -6, 0);
				}
			}
			
			//UITabBarController tabctrl = Platform.GetRenderer(Tabbed.Children[0]).ViewController.TabBarController;
			//tabctrl.TabBar.TintColor = selectedTextColor;			
		}

		async Task<(UIImage selctedIcon,UIImage unselectedIcon)> LoadIcon(TabItem tabItem,AiTabbedPage aiTabbed)
        {

			var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(tabItem.Icon.GetType());

			UIImage uiImage = await handler.LoadImageAsync(tabItem.Icon);

			if(aiTabbed.SelectedTabColor == Color.Default && aiTabbed.UnselectedTabColor == Color.Default)
            {
				var original = uiImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
				return (original, original);
            }

			var selectedColor = aiTabbed.SelectedTabColor.ToUIColor();
			var unselectedColor = aiTabbed.UnselectedTabColor.ToUIColor();

			var selected = uiImage.ApplyTintColor(selectedColor,UIImageRenderingMode.AlwaysTemplate);
			var unselected = uiImage.ApplyTintColor(unselectedColor, UIImageRenderingMode.AlwaysTemplate);

			return (selected, unselected);
		}
	}
}
