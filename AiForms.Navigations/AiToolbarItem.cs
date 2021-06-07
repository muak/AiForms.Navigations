using System;
using Xamarin.Forms;

namespace AiForms.Navigations
{
    public class AiToolbarItem: ToolbarItem
    {
        public static BindableProperty IsVisibleProperty = BindableProperty.Create(
            nameof(IsVisible),
            typeof(bool),
            typeof(AiToolbarItem),
            true,
            defaultBindingMode: BindingMode.OneWay
        );

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }
    }
}
