using System;
using Android.Widget;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Naxam.Controls.Platform.Droid;
using System.ComponentModel;
using Com.Ittianyu.Bottomnavigationviewex;
using Naxam.Controls.Forms;
using Naxam.Controls.Platform.Droid.Utils;
using Android.Support.Design.Internal;

[assembly: ExportRenderer(typeof(BottomTabbedPage), typeof(BottomTabbedRenderer))]
namespace Naxam.Controls.Platform.Droid
{
    using RelativeLayout = Android.Widget.RelativeLayout;
    using Platform = Xamarin.Forms.Platform.Android.Platform;

    public partial class BottomTabbedRenderer : VisualElementRenderer<BottomTabbedPage>
    {
        public static float? BottomBarHeight;

        RelativeLayout rootLayout;
        FrameLayout pageContainer;
        BottomNavigationViewEx bottomNav;

        IPageController TabbedController => Element as IPageController;

        public BottomTabbedRenderer()
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BottomTabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                //TODO Cleanup
            }

            if (e.NewElement == null)
            {
                return;
            }

            UpdateIgnoreContainerAreas();

            if (rootLayout == null)
            {
                SetupNativeView();
                SetupEventHandlers();
            }

            SetupTabItems();
            SwitchContent(Element.CurrentPage);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(TabbedPage.CurrentPage))
            {
                SwitchContent(Element.CurrentPage);
            }
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            TabbedController?.SendAppearing();
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            TabbedController?.SendDisappearing();
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            var width = right - left;
            var height = bottom - top;
            int tabsHeight = 0;




            base.OnLayout(changed, left, top, right, bottom);

            if (width <= 0 || height <= 0)
            {
                return;
            }




            rootLayout.Measure(
                MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));


            tabsHeight = BottomBarHeight.HasValue ? (int)Context.ToPixels(BottomBarHeight.Value) : Math.Min(height, Math.Max(bottomNav.MeasuredHeight, bottomNav.MinimumHeight));




            TabbedController.ContainerArea = new Rectangle(
                0, 0,
                Context.FromPixels(rootLayout.MeasuredWidth),
                Context.FromPixels(pageContainer.MeasuredHeight)
            );



            rootLayout.Measure(
                MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.Exactly));
            rootLayout.Layout(0, 0, rootLayout.MeasuredWidth, rootLayout.MeasuredHeight);

            if (ItemPadding == null)
                ItemPadding = new Thickness(0);

            var item = (ViewGroup)bottomNav.GetChildAt(0);
            item.Measure(
                MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpecFactory.MakeMeasureSpec(tabsHeight, MeasureSpecMode.Exactly));
            item.Layout(0, 0, width, tabsHeight);
            int item_w = width / item.ChildCount;
            for (int i = 0; i < item.ChildCount; i++)
            {
                var frame = (FrameLayout)item.GetChildAt(i);
                frame.Measure(
                MeasureSpecFactory.MakeMeasureSpec(item_w, MeasureSpecMode.Exactly),
                MeasureSpecFactory.MakeMeasureSpec(tabsHeight, MeasureSpecMode.Exactly));
                frame.Layout(i * item_w, 0, i * item_w + item_w, tabsHeight);


                var imgView = bottomNav.GetIconAt(i);
                var baselayout = frame.GetChildAt(1);
                if (baselayout != null)
                {
                    if (baselayout.GetType() == typeof(BaselineLayout))
                    {
                        //Container text
                        var basel = (BaselineLayout)baselayout; 
                        //Small text
                        var small = bottomNav.GetSmallLabelAt(i); 
                        //Large text
                        var large = bottomNav.GetLargeLabelAt(i); 

                        //Height Container text
                        int baselH = Math.Max(small.Height, large.Height) - (int)Context.ToPixels(4);
                        //width Container text
                        int baselW = Math.Min(small.Width, item_w - (int)Context.ToPixels(ItemPadding.Left) - (int)Context.ToPixels(ItemPadding.Right));
                        //Icon Height
                        int imgH = imgView.LayoutParameters.Height;
                        //Icon Width
                        int imgW = Math.Min(imgView.LayoutParameters.Width, item_w - (int)Context.ToPixels(ItemPadding.Left) - (int)Context.ToPixels(ItemPadding.Right));

                        int imgTop = (tabsHeight - imgH - baselH - (int)Context.ToPixels(ItemSpacing.Value)) / 2;
                        int imgLeft = (item_w - imgW) / 2;
                        int topBaseLine = imgTop + imgH + (int)Context.ToPixels(ItemSpacing.Value);
                        int leftBaseLine = (item_w - baselW) / 2;

                        switch (ItemAlign)
                        {
                            case ItemAlignFlags.Default:
                                imgTop = (int)Context.ToPixels(ItemPadding.Top);
                                topBaseLine = tabsHeight - baselH - (int)Context.ToPixels(ItemPadding.Bottom);
                                break;
                            case ItemAlignFlags.Top:
                                imgTop = (int)Context.ToPixels(ItemPadding.Top);
                                topBaseLine = imgTop + imgH + (int)Context.ToPixels(ItemSpacing.Value);
                                break;
                            case ItemAlignFlags.Bottom:
                                imgTop = tabsHeight - imgH - baselH - (int)Context.ToPixels(ItemSpacing.Value) - (int)Context.ToPixels(ItemPadding.Bottom);
                                topBaseLine = imgTop + imgH + (int)Context.ToPixels(ItemSpacing.Value);
                                break;
                        }
                        //layout icon, text
                        imgView.Measure(MeasureSpecFactory.MakeMeasureSpec(imgW, MeasureSpecMode.Exactly), MeasureSpecFactory.MakeMeasureSpec(imgH, MeasureSpecMode.Exactly));
                        imgView.Layout(imgLeft, imgTop, imgW + imgLeft, imgH + imgTop);
                        basel.Measure(MeasureSpecFactory.MakeMeasureSpec(baselW, MeasureSpecMode.Exactly), MeasureSpecFactory.MakeMeasureSpec(tabsHeight, MeasureSpecMode.Exactly));
                        basel.Layout(leftBaseLine, topBaseLine, leftBaseLine + baselW, topBaseLine + baselH);
                       
                        //text break
                        var breaktext = small.Paint.BreakText(small.Text, true, item_w - (int)Context.ToPixels(ItemPadding.Right) - (int)Context.ToPixels(ItemPadding.Left), null);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && rootLayout != null)
            {
                //TODO Cleanup
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

                if (bottomNav != null)
                {
                    bottomNav.SetOnNavigationItemSelectedListener(null);
                    bottomNav.Dispose();
                    bottomNav = null;
                }
                rootLayout.Dispose();
                rootLayout = null;
            }

            base.Dispose(disposing);
        }

        void SetupNativeView()
        {
            rootLayout = new RelativeLayout(Context)
            {
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent),
            };
            var barId = GenerateViewId();
            var pageParams = new RelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            pageParams.AddRule(LayoutRules.Above, barId);

            pageContainer = new FrameLayout(Context)
            {
                LayoutParameters = pageParams,
                Id = GenerateViewId()
            };

            var barParams = new RelativeLayout.LayoutParams(
                LayoutParams.MatchParent,
                BottomBarHeight.HasValue ? (int)Context.ToPixels(BottomBarHeight.Value) : LayoutParams.WrapContent);
            barParams.AddRule(LayoutRules.AlignParentBottom);
            bottomNav = new BottomNavigationViewEx(Context)
            {
                LayoutParameters = barParams,
                Id = barId
            };

            rootLayout.AddView(pageContainer, 0, pageParams);
            rootLayout.AddView(bottomNav, 1, barParams);
            AddView(rootLayout);

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
        }

        void SwitchContent(Page page)
        {
            Context.HideKeyboard(this);
            pageContainer.RemoveAllViews();

            if (page == null)
            {
                return;
            }

            if (Platform.GetRenderer(page) == null)
            {
                Platform.SetRenderer(page, Platform.CreateRenderer(page));
            }
            var pageContent = Platform.GetRenderer(page).ViewGroup;
            pageContainer.AddView(pageContent);
        }

        void UpdateIgnoreContainerAreas()
        {
            foreach (IPageController child in Element.Children)
            {
                child.IgnoresContainerArea = false;
            }
        }
    }
}