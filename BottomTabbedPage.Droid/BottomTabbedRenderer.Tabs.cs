using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Support.Design.Internal;
using Android.Views;
using Com.Ittianyu.Bottomnavigationviewex;
using Naxam.Controls.Platform.Droid.Utils;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Widget;

namespace Naxam.Controls.Platform.Droid
{
    public partial class BottomTabbedRenderer : BottomNavigationViewEx.IOnNavigationItemSelectedListener
    {
        public static int? ItemBackgroundResource;
        public static ColorStateList ItemIconTintList;
        public static ColorStateList ItemTextColor;
        public static Android.Graphics.Color? BackgroundColor;
        public static Typeface Typeface;
        public static float? IconSize;
        public static float? FontSize;
        public static float ItemSpacing;
        public static ItemAlignFlags ItemAlign;
        public static Thickness ItemPadding;

        BottomNavigationMenu menu => (BottomNavigationMenu)bottomNav.Menu;

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var menu = this.menu;
            var index = menu.FindItemIndex(item.ItemId);
            var pageIndex = index % Element.Children.Count;
            var currentPageIndex = Element.Children.IndexOf(Element.CurrentPage);

            if (currentPageIndex != pageIndex)
            {
                Element.CurrentPage = Element.Children[pageIndex];
            }
            return true;
        }

        void SetupTabItems()
        {
            Element.SetupTabItems(bottomNav);
        }

        void SetupBottomBar()
        {
            this.SetupBottomBar(rootLayout, bottomNav, barId);
        }
    }

    public enum ItemAlignFlags
    {
        Default, Center, Top, Bottom
    }

}
