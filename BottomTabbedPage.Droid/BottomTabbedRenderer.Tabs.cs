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

        public enum ItemAlignFlags
        {
            Default, Center, Top, Bottom
        }

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
            var menu = this.menu;
            menu.ClearAll();

            var tabsCount = Math.Min(Element.Children.Count, bottomNav.MaxItemCount);
            for (int i = 0; i < tabsCount; i++)
            {
                var page = Element.Children[i];
                var menuItem = menu.Add(0, i, 0, page.Title);
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

        void SetupBottomBar()
        {
            if (bottomNav != null)
            {
                rootLayout.RemoveView(bottomNav);
                bottomNav.SetOnNavigationItemSelectedListener(null);
            }

            var barParams = new Android.Widget.RelativeLayout.LayoutParams(
                LayoutParams.MatchParent,
                BottomBarHeight.HasValue ? (int)Context.ToPixels(BottomBarHeight.Value) : LayoutParams.WrapContent);
            barParams.AddRule(LayoutRules.AlignParentBottom);
            bottomNav = new BottomNavigationViewEx(Context)
            {
                LayoutParameters = barParams,
                Id = barId
            };
            if (BackgroundColor.HasValue)
            {
                bottomNav.SetBackgroundColor(BackgroundColor.Value);
            }
            if (ItemIconTintList != null)
            {
                bottomNav.ItemIconTintList = ItemIconTintList;
            }
            if (ItemTextColor != null)
            {
                bottomNav.ItemTextColor = ItemTextColor;
            }
            if (ItemBackgroundResource.HasValue)
            {
                bottomNav.ItemBackgroundResource = ItemBackgroundResource.Value;
            }

            bottomNav.SetOnNavigationItemSelectedListener(this);
            rootLayout.AddView(bottomNav, 1, barParams);
        }
    }
}
