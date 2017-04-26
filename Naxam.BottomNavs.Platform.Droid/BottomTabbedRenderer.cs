using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Android.Support.Design.Widget;
using Android.Support.Design.Internal;
using Xamarin.Forms;
using System.ComponentModel;
using Com.Ittianyu.Bottomnavigationviewex;
using Android.Graphics;
using Naxam.BottomNavs.Platform.Droid;
using Naxam.BottomNavs.Platform.Droid.Utils;
using Naxam.BottomNavs.Forms;
using Android.Content.Res;
using Android.Support.V7.View.Menu;

[assembly: ExportRenderer(typeof(BottomTabbedPage), typeof(BottomTabbedRenderer))]
namespace Naxam.BottomNavs.Platform.Droid
{
    using Platform = Xamarin.Forms.Platform.Android.Platform;
    using Forms = Xamarin.Forms.Forms;

    public class BottomTabbedRenderer : VisualElementRenderer<BottomTabbedPage>, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        public static int? ItemBackgroundResource;
        public static int? BottomBarHeight;
        public static ColorStateList ItemIconTintList;
        public static ColorStateList ItemTextColor;
        public static Android.Graphics.Color? BackgroundColor;
        public static Typeface Typeface;
        public static int? IconSize;
        public static int? FontSize;

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var menu = (BottomNavigationMenu)_bottomBar.Menu;
            var index = menu.FindItemIndex(item.ItemId);

            var pageIndex = index % Element.Children.Count;

            OnTabSelected(pageIndex);

            return true;
        }


        bool _disposed;
        BottomNavigationViewEx _bottomBar;
        BottomNavigationMenu _menu;
        FrameLayout _frameLayout;
        Utils.IPageController _pageController;
        private LinearLayout _rootLayout;

        public BottomTabbedRenderer()
        {
            AutoPackage = false;

        }

        #region IOnTabClickListener
        public void OnTabSelected(int position)
        {
            //Do we need this call? It's also done in OnElementPropertyChanged
            // SwitchContent(Element.Children[position]); 
            Element.CurrentPage = Element.Children[position];
        }

        public void OnTabReSelected(int position)
        {
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;

                RemoveAllViews();

                foreach (Page pageToRemove in Element.Children)
                {
                    IVisualElementRenderer pageRenderer = Platform.GetRenderer(pageToRemove);

                    if (pageRenderer != null)
                    {
                        pageRenderer.ViewGroup.RemoveFromParent();
                        pageRenderer.Dispose();
                    }
                }

                if (_bottomBar != null)
                {
                    _bottomBar.SetOnNavigationItemSelectedListener(null);
                    _bottomBar.Dispose();
                    _bottomBar = null;
                }

                if (_frameLayout != null)
                {
                    _frameLayout.Dispose();
                    _frameLayout = null;
                }
            }

            base.Dispose(disposing);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            _pageController.SendAppearing();
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            _pageController.SendDisappearing();
        }


        protected override void OnElementChanged(ElementChangedEventArgs<BottomTabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {

                BottomTabbedPage bottomBarPage = e.NewElement;

                if (_bottomBar == null)
                {
                    _pageController = PageController.Create(bottomBarPage);

                    _rootLayout = new LinearLayout(Context)
                    {
                        LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                    };
                    AddView(_rootLayout);

                    // create a view which will act as container for Page's
                    _frameLayout = new FrameLayout(Context)
                    {
                        LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                    };
                    _rootLayout.AddView(_frameLayout);

                    _bottomBar = new BottomNavigationViewEx(Context)
                    {
                        LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                    };

                    _rootLayout.AddView(_bottomBar);

                    _bottomBar.SetOnNavigationItemSelectedListener(this);
                    if (BackgroundColor.HasValue)
                    {
                        _bottomBar.SetBackgroundColor(BackgroundColor.Value);
                    }



                    if (ItemIconTintList != null)
                    {
                        _bottomBar.ItemIconTintList = ItemIconTintList;
                    }
                    if (ItemTextColor != null)
                    {
                        _bottomBar.ItemTextColor = ItemTextColor;
                    }
                    if (ItemBackgroundResource.HasValue)
                    {
                        _bottomBar.ItemBackgroundResource = ItemBackgroundResource.Value;
                    }
                    // Resource.Drawable.bnv_selector

                    _menu = (BottomNavigationMenu)_bottomBar.Menu;

                    UpdateTabs();
                }

                if (bottomBarPage.CurrentPage != null)
                {
                    SwitchContent(bottomBarPage.CurrentPage);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(TabbedPage.CurrentPage))
            {
                SwitchContent(Element.CurrentPage);
            }
        }



        protected virtual void SwitchContent(Page view)
        {
            Context.HideKeyboard(this);

            _frameLayout.RemoveAllViews();

            if (view == null)
            {
                return;
            }

            if (Platform.GetRenderer(view) == null)
            {
                Platform.SetRenderer(view, Platform.CreateRenderer(view));
            }

            _frameLayout.AddView(Platform.GetRenderer(view).ViewGroup);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            int width = r - l;
            int height = b - t;

            if (width > 0 && height > 0)
            {
                var context = Context;
                _rootLayout.Measure(MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.AtMost), MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));
                _rootLayout.Layout(0, 0, _rootLayout.MeasuredWidth, _rootLayout.MeasuredHeight);

                _bottomBar.Measure(MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly), MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));
                int tabsHeight = BottomBarHeight.HasValue ? (int)Context.ToPixels(BottomBarHeight.Value) : Math.Min(height, Math.Max(_bottomBar.MeasuredHeight, _bottomBar.MinimumHeight));

                _frameLayout.Layout(0, 0, width, height - tabsHeight);

                _pageController.ContainerArea = new Rectangle(0, 0, context.FromPixels(width), context.FromPixels(_frameLayout.Height));

                _bottomBar.Layout(0, height - tabsHeight, width, height);
            }


            

            base.OnLayout(changed, l, t, r, b);
        }

        void UpdateTabs()
        {
            // create tab items
            SetTabItems();

            // set tab colors
            SetTabColors();
        }

        void SetTabItems()
        {
            //BottomBarTab [] tabs = Element.Children.Select (page => {
            //	var tabIconId = ResourceManagerEx.IdFromTitle (page.Icon, ResourceManager.DrawableClass);
            //	return new BottomBarTab (tabIconId, page.Title);
            //}).ToArray ();

            //_bottomBar.SetItems (tabs);
            var tabsCount = Math.Min(Element.Children.Count, _bottomBar.MaxItemCount);
            for (int i = 0; i < tabsCount; i++)
            {
                var page = Element.Children[i];
                var menuItem = _menu.Add(0, i, 0, page.Title);
                var tabIconId = ResourceManagerEx.IdFromTitle(page.Icon, ResourceManager.DrawableClass);
                menuItem.SetIcon(tabIconId);
            }
            if (Element.Children.Count > 0)
            {
                _bottomBar.EnableShiftingMode(false);//remove shifting mode
                _bottomBar.EnableItemShiftingMode(false);//remove shifting mode
                _bottomBar.EnableAnimation(false);//remove animation
                if (Typeface != null)
                {
                    _bottomBar.SetTypeface(Typeface);
                }
                if (IconSize.HasValue)
                {
                    _bottomBar.SetIconSize(IconSize.Value, IconSize.Value);
                }
                if (FontSize.HasValue)
                    _bottomBar.SetTextSize(FontSize.Value);
                _bottomBar.TextAlignment = Android.Views.TextAlignment.Center;

            }
        }


        void SetTabColors()
        {
            //for (int i = 0; i < Element.Children.Count; ++i) {
            //	Page page = Element.Children [i];

            //	Color? tabColor = page.GetTabColor ();

            //	if (tabColor != null) {
            //		_bottomBar.MapColorForTab (i, tabColor.Value.ToAndroid ());
            //	}
            //}
        }
    }
}