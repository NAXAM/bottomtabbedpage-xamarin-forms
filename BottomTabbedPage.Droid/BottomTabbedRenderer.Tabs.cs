using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Support.Design.Internal;
using Android.Views;
using Com.Ittianyu.Bottomnavigationviewex;
using Naxam.Controls.Platform.Droid.Utils;
using Xamarin.Forms.Platform.Android;

namespace Naxam.Controls.Platform.Droid
{
    public partial class BottomTabbedRenderer : BottomNavigationViewEx.IOnNavigationItemSelectedListener
    {
        public static int? ItemBackgroundResource;
        public static ColorStateList ItemIconTintList;
        public static ColorStateList ItemTextColor;
        public static Color? BackgroundColor;
        public static Typeface Typeface;
        public static float? IconSize;
        public static float? FontSize;
        public static float? ItemPadding;

        BottomNavigationMenu _menu;
        BottomNavigationMenu menu => (_menu = _menu ?? (BottomNavigationMenu)bottomNav.Menu);

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var index = menu.FindItemIndex(item.ItemId);
            var pageIndex = index % Element.Children.Count;

            Element.CurrentPage = Element.Children[pageIndex];

            return true;
        }

        void SetupTabItems()
        {
            var tabsCount = Math.Min(Element.Children.Count, bottomNav.MaxItemCount);
            for (int i = 0; i < tabsCount; i++)
            {
                var page = Element.Children[i];
                var menuItem = bottomNav.Menu.Add(0, i, 0, page.Title);
                var tabIconId = ResourceManagerEx.IdFromTitle(page.Icon, ResourceManager.DrawableClass);
                menuItem.SetIcon(tabIconId);
            }
            if (Element.Children.Count > 0)
            {
                bottomNav.EnableShiftingMode(false);//remove shifting mode
                bottomNav.EnableItemShiftingMode(false);//remove shifting mode
                bottomNav.EnableAnimation(false);//remove animation

                if (Typeface != null)
                {
                    bottomNav.SetTypeface(Typeface);
                }
                if (IconSize.HasValue)
                {
                    bottomNav.SetIconSize(IconSize.Value, IconSize.Value);
                }
                if (FontSize.HasValue)
                {
                    bottomNav.SetTextSize(FontSize.Value);
                }

                bottomNav.TextAlignment = Android.Views.TextAlignment.Center;
            }
        }

        void SetupEventHandlers()
        {
            bottomNav.SetOnNavigationItemSelectedListener(this);
        }
    }
}
