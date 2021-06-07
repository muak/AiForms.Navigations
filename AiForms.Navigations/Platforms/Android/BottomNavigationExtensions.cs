using System;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using Xamarin.Forms.Platform.Android;

namespace AiForms.Navigations.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public static class BottomNavigationExtensions
    {
		public static void SetShiftMode(this BottomNavigationView bottomNavigationView, bool enableShiftMode, bool enableItemShiftMode, AiTabbedPage aiTabbed)
		{
			try
			{
				var menuView = bottomNavigationView.GetChildAt(0) as BottomNavigationMenuView;
				if (menuView == null)
				{
					System.Diagnostics.Debug.WriteLine("Unable to find BottomNavigationMenuView");
					return;
				}

				SetFieldVisibilityMode(menuView.Class, menuView, "labelVisibilityMode", 1);

				for (int i = 0; i < menuView.ChildCount; i++)
				{
					var item = menuView.GetChildAt(i) as BottomNavigationItemView;
					if (item == null)
						continue;

					item.SetShifting(enableItemShiftMode);
					item.SetChecked(item.ItemData.IsChecked);
					SetField(item.Class, item, "shiftAmount", 0);
					SetField(item.Class, item, "scaleUpFactor", 1);
					SetField(item.Class, item, "scaleDownFactor", 1);

					var mLargeLabel = GetField<TextView>(item.Class, item, "largeLabel");
					var mSmallLabel = GetField<TextView>(item.Class, item, "smallLabel");

					if (aiTabbed.TabFontSize > -1)
					{
						mSmallLabel.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)aiTabbed.TabFontSize);
					}
					mLargeLabel.SetTextSize(Android.Util.ComplexUnitType.Px, mSmallLabel.TextSize);

					if (!aiTabbed.SelectedTextColor.IsDefault)
					{
						mLargeLabel.SetTextColor(aiTabbed.SelectedTextColor.ToAndroid());
					}
					if (!aiTabbed.UnSelectedTextColor.IsDefault)
					{
						mSmallLabel.SetTextColor(aiTabbed.UnSelectedTextColor.ToAndroid());
					}
				}

				menuView.UpdateMenuView();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Unable to set shift mode: {ex}");
			}
		}

		static T GetField<T>(Java.Lang.Class targetClass, Java.Lang.Object instance, string fieldName) where T : Java.Lang.Object
		{
			try
			{
				var field = targetClass.GetDeclaredField(fieldName);
				field.Accessible = true;
				var value = field.Get(instance);
				field.Accessible = false;
				field.Dispose();

				return value as T;
			}
			catch
			{
				return null;
			}

		}

		static void SetField(Java.Lang.Class targetClass, Java.Lang.Object instance, string fieldName, Java.Lang.Object value)
		{
			try
			{
				var field = targetClass.GetDeclaredField(fieldName);
				field.Accessible = true;
				field.Set(instance, value);
				field.Accessible = false;
				field.Dispose();
			}
			catch
			{
				return;
			}
		}

		static void SetFieldVisibilityMode(Java.Lang.Class targetClass, Java.Lang.Object instance, string fieldName, int value)
		{
			try
			{
				var field = targetClass.GetDeclaredField(fieldName);
				field.Accessible = true;
				field.SetInt(instance, value);
				field.Accessible = false;
				field.Dispose();
			}
			catch
			{
				return;
			}
		}
	}
}
