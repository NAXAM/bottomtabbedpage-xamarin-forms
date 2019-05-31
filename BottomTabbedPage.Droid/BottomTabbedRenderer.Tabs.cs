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
        public static bool? VisibleTitle;

        internal int CurrentMenuItemId = 0;

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            this.SwitchPage(item);
            return true;
        }

        internal void SetupTabItems()
        {
            this.SetupTabItems(bottomNav);
        }

        internal void SetupBottomBar()
        {
            bottomNav = this.SetupBottomBar(rootLayout, bottomNav, barId);
        }
    }

    public enum ItemAlignFlags
    {
        Default, Center, Top, Bottom
    }

}
