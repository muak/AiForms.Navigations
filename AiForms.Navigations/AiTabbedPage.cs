using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AiForms.Navigations
{
    public class AiTabbedPage:TabbedPage
    {
		public static BindableProperty SelectedTextColorProperty =
            BindableProperty.Create(
				nameof(SelectedTextColor),
				typeof(Xamarin.Forms.Color),
				typeof(AiTabbedPage),
				Xamarin.Forms.Color.Default,
				defaultBindingMode: Xamarin.Forms.BindingMode.OneWay
        );

        public Xamarin.Forms.Color SelectedTextColor
		{
			get { return (Xamarin.Forms.Color)GetValue(SelectedTextColorProperty); }
			set { SetValue(SelectedTextColorProperty, value); }
		}

		public static BindableProperty UnSelectedTextColorProperty =
			BindableProperty.Create(
				nameof(UnSelectedTextColor),
				typeof(Xamarin.Forms.Color),
				typeof(AiTabbedPage),
				Xamarin.Forms.Color.Default,
				defaultBindingMode: Xamarin.Forms.BindingMode.OneWay
		);

		public Xamarin.Forms.Color UnSelectedTextColor
		{
			get { return (Xamarin.Forms.Color)GetValue(UnSelectedTextColorProperty); }
			set { SetValue(UnSelectedTextColorProperty, value); }
		}

        public static BindableProperty TabFontSizeProperty = BindableProperty.Create(
            nameof(TabFontSize),
            typeof(double),
            typeof(AiTabbedPage),
            -1d,
            defaultBindingMode: BindingMode.OneWay
        );

        public double TabFontSize{
            get { return (double)GetValue(TabFontSizeProperty); }
            set { SetValue(TabFontSizeProperty, value); }
        }

		public static BindableProperty IsTextHiddenProperty =
			BindableProperty.Create(
				nameof(IsTextHidden), typeof(bool),
				typeof(AiTabbedPage),
				false,
				defaultBindingMode: Xamarin.Forms.BindingMode.OneWay
		);

		public bool IsTextHidden
		{
			get { return (bool)GetValue(IsTextHiddenProperty); }
			set { SetValue(IsTextHiddenProperty, value); }
		}

		public static BindableProperty TabBorderColorProperty = BindableProperty.Create(
			nameof(TabBorderColor),
			typeof(Color),
			typeof(AiTabbedPage),
			default(Color),
			defaultBindingMode: BindingMode.OneWay
		);

		public Color TabBorderColor
		{
			get { return (Color)GetValue(TabBorderColorProperty); }
			set { SetValue(TabBorderColorProperty, value); }
		}

		public static BindableProperty TabItemsProperty =
			BindableProperty.Create(
				nameof(TabItems),
				typeof(IList<TabItem>),
				typeof(AiTabbedPage),
				null,
				defaultBindingMode: BindingMode.OneWay
			);

		public IList<TabItem> TabItems
		{
			get { return (IList<TabItem>)GetValue(TabItemsProperty); }
			set { SetValue(TabItemsProperty, value); }
		}

		public AiTabbedPage()
        {
        }
    }
}
