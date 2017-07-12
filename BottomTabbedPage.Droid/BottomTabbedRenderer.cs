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
        public static readonly Action<IMenuItem, FileImageSource> DefaultMenuItemIconSetter = (menuItem, icon) =>
		{
			var tabIconId = ResourceManagerEx.IdFromTitle(icon, ResourceManager.DrawableClass);
			menuItem.SetIcon(tabIconId);
		};

		public static Action<IMenuItem, FileImageSource> MenuItemIconSetter;
        public static float? BottomBarHeight;

        RelativeLayout rootLayout;
        FrameLayout pageContainer;
        BottomNavigationViewEx bottomNav;
        readonly int barId;

        IPageController TabbedController => Element as IPageController;

        public BottomTabbedRenderer()
        {
            AutoPackage = false;

            barId = GenerateViewId();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BottomTabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.ChildAdded -= PagesChanged;
                e.OldElement.ChildRemoved -= PagesChanged;
                e.OldElement.ChildrenReordered -= PagesChanged;
            }

            if (e.NewElement == null)
            {
                return;
            }

            UpdateIgnoreContainerAreas();

            if (rootLayout == null)
            {
                SetupNativeView();
            }


            this.HandlePagesChanged();
            SwitchContent(Element.CurrentPage);
            
            Element.ChildAdded += PagesChanged;
            Element.ChildRemoved += PagesChanged;
            Element.ChildrenReordered += PagesChanged;
        }

        void PagesChanged(object sender, EventArgs e)
        {
            this.HandlePagesChanged();
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

            this.Layout(width, height);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
				Element.ChildAdded -= PagesChanged;
				Element.ChildRemoved -= PagesChanged;
				Element.ChildrenReordered -= PagesChanged;

                if (rootLayout != null)
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
            }

            base.Dispose(disposing);
        }

        internal void SetupNativeView()
        {
            rootLayout = this.CreateRoot(barId, GenerateViewId(),out pageContainer);

            AddView(rootLayout);
        }

        void SwitchContent(Page page)
        {
            this.ChangePage(pageContainer, page);
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