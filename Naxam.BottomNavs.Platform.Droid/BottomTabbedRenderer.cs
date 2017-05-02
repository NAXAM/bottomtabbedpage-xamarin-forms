using System;
using Android.Content;
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

[assembly: ExportRenderer(typeof(BottomTabbedPage), typeof(BottomTabbedRenderer))]
namespace Naxam.BottomNavs.Platform.Droid
{
    using Platform = Xamarin.Forms.Platform.Android.Platform;

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
        public static int? ItemPadding;

        bool _disposed;
        BottomNavigationViewEx _bottomBar;
        BottomNavigationMenu _menu;
        FrameLayout _frameLayout;
        Utils.IPageController _pageController;
        private LinearLayout _rootLayout;

        public BottomTabbedRenderer()
        {
            AutoPackage = false;
            ItemPadding = ItemPadding ?? 0;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var menu = (BottomNavigationMenu)_bottomBar.Menu;
            var index = menu.FindItemIndex(item.ItemId);
            var pageIndex = index % Element.Children.Count;
            OnTabSelected(pageIndex);
            return true;
        }

        public void OnTabSelected(int position)
        {
            Element.CurrentPage = Element.Children[position];
        }

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
            int tabsHeight = 0;
            if (width > 0 && height > 0)
            {
                var context = Context;
                _rootLayout.Measure(MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.AtMost), MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));
                _rootLayout.Layout(0, 0, _rootLayout.MeasuredWidth, _rootLayout.MeasuredHeight);
                _bottomBar.Measure(MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly), MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));
                tabsHeight = BottomBarHeight.HasValue ? (int)Context.ToPixels(BottomBarHeight.Value) : Math.Min(height, Math.Max(_bottomBar.MeasuredHeight, _bottomBar.MinimumHeight));
                _frameLayout.Layout(0, 0, width, height - tabsHeight);
                _pageController.ContainerArea = new Rectangle(0, 0, context.FromPixels(width), context.FromPixels(_frameLayout.Height));
                _bottomBar.Layout(0, height - tabsHeight, width, height);

                var item = (ViewGroup)_bottomBar.GetChildAt(0);
                item.Measure(width, tabsHeight);
                item.Layout(0, 0, width, tabsHeight);
                int item_w = width / item.ChildCount;

                for (int i = 0; i < item.ChildCount; i++)
                {
                    var frame = (FrameLayout)item.GetChildAt(i);
                    var imgView = _bottomBar.GetIconAt(i);
                    var baselayout = frame.GetChildAt(1);
                    if (baselayout != null)
                    {
                        if (baselayout.GetType() == typeof(BaselineLayout))
                        {
                            var basel = (BaselineLayout)baselayout;
                            var small = _bottomBar.GetSmallLabelAt(i);
                            var large = _bottomBar.GetLargeLabelAt(i);
                            int baselH = Math.Max(small.Height, large.Height);
                            int baselW = Math.Min(small.Width, item_w - (int)Context.ToPixels(ItemPadding.Value));

                            int imgH = imgView.LayoutParameters.Height;
                            int imgW = Math.Min(imgView.LayoutParameters.Width, item_w - (int)Context.ToPixels(ItemPadding.Value));
                            int imgTop = (tabsHeight - imgH - baselH) / 2;
                            int imgLeft = (item_w - imgW) / 2;
                            int topBaseLine = imgTop + imgH + (int)Context.ToPixels(ItemPadding.Value);
                            int leftBaseLine = (item_w - baselW) / 2;

                            imgView.Measure(MeasureSpecFactory.MakeMeasureSpec(imgW, MeasureSpecMode.Exactly), MeasureSpecFactory.MakeMeasureSpec(imgH, MeasureSpecMode.AtMost));
                            imgView.Layout(imgLeft, imgTop, imgW + imgLeft, imgH + imgTop);
                            basel.Measure(MeasureSpecFactory.MakeMeasureSpec(baselW, MeasureSpecMode.Exactly), MeasureSpecFactory.MakeMeasureSpec(tabsHeight, MeasureSpecMode.AtMost));
                            basel.Layout(leftBaseLine, topBaseLine, leftBaseLine + baselW, topBaseLine + baselH);

                            var breaktext = small.Paint.BreakText(small.Text, true, item_w - (int)Context.ToPixels(ItemPadding.Value), null);
                            var text = small.Text;
                            if (text.Length > breaktext)
                            {
                                small.Text = text.Substring(0, breaktext - 1);
                                large.Text = text.Substring(0, breaktext - 1);
                            }
                        }
                    }
                }
            }

            base.OnLayout(changed, l, t, r, b);
        }



        void UpdateTabs()
        {
            SetTabItems();
        }

        void SetTabItems()
        {
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
                {
                    _bottomBar.SetTextSize(FontSize.Value);
                }

                _bottomBar.TextAlignment = Android.Views.TextAlignment.Center;
            }
        }
    }
}