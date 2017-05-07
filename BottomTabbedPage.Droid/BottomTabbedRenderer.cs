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

            base.OnLayout(changed, left, top, right, bottom);

            if (width <= 0 || height <= 0)
            {
                return;
            }

            rootLayout.Measure(
                MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));

            TabbedController.ContainerArea = new Rectangle(
                0, 0,
                Context.FromPixels(rootLayout.MeasuredWidth),
                Context.FromPixels(pageContainer.MeasuredHeight)
            );

            rootLayout.Measure(
                MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.Exactly));
            rootLayout.Layout(0, 0, rootLayout.MeasuredWidth, rootLayout.MeasuredHeight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && rootLayout != null)
            {
                //TODO Cleanup
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