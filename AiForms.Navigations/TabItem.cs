using System;
using Xamarin.Forms;

namespace AiForms.Navigations
{
    public class TabItem:BindableObject
    {
        public static BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(TabItem),
            default(string),
            defaultBindingMode: BindingMode.OneWay
        );

        public string Title{
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static BindableProperty IconProperty = BindableProperty.Create(
            nameof(Icon),
            typeof(ImageSource),
            typeof(TabItem),
            default(ImageSource),
            defaultBindingMode: BindingMode.OneWay
        );

        public ImageSource Icon{
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public TabItem()
        {
        }        
    }
}
