using System;
using Android.Widget;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NxTabbedPage = Naxam.Controls.Forms.BottomTabbedPage;
using Naxam.Controls.Platform.Droid;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(NxTabbedPage), typeof(BottomTabbedRenderer))]
namespace Naxam.Controls.Platform.Droid
{
    using RelativeLayout = Android.Widget.RelativeLayout;
    using Platform = Xamarin.Forms.Platform.Android.Platform;

    public class BottomTabbedRenderer : VisualElementRenderer<NxTabbedPage>
    {
        RelativeLayout rootLayout;
        FrameLayout pageContainer;

        IPageController TabbedController => Element as IPageController;

        public BottomTabbedRenderer()
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NxTabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                //Cleanup
            }

            if (e.NewElement == null)
            {
                return;
            }

            UpdateIgnoreContainerAreas();

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
            pageContainer.SetBackgroundColor(Android.Graphics.Color.Green);
            rootLayout.AddView(pageContainer, 0, pageParams);

            var barParams = new RelativeLayout.LayoutParams(LayoutParams.MatchParent, 160);
            barParams.AddRule(LayoutRules.AlignParentBottom);

            var barContainer = new FrameLayout(Context)
            {
                LayoutParameters = barParams,
                Id = barId
            };
            barContainer.SetBackgroundColor(Android.Graphics.Color.Magenta);

            rootLayout.AddView(barContainer, 1, barParams);

            AddView(rootLayout);

            SwitchContent(Element.CurrentPage);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "CurrentPage")
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
            //var pcParams = new FrameLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent, GravityFlags.Fill); 
            //pageContainer.AddView(pageContent, 0, pcParams);
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

    internal static class MeasureSpecFactory
    {
        public static int GetSize(int measureSpec)
        {
            const int modeMask = 0x3 << 30;
            return measureSpec & ~modeMask;
        }

        // Literally does the same thing as the android code, 1000x faster because no bridge cross
        // benchmarked by calling 1,000,000 times in a loop on actual device
        public static int MakeMeasureSpec(int size, MeasureSpecMode mode)
        {
            return size + (int)mode;
        }
    }
}