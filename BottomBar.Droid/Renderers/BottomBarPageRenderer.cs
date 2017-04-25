/*
 * BottomNavigationBar for Xamarin Forms
 * Copyright (c) 2016 Thrive GmbH and others (http://github.com/thrive-now).
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using BottomBar.XamarinForms;

using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using BottomBar.Droid.Renderers;
using BottomBar.Droid.Utils;
using Android.Support.Design.Widget;
using Android.Support.Design.Internal;
using Android.Support.V7.View.Menu;

[assembly: ExportRenderer(typeof(BottomBarPage), typeof(BottomBarPageRenderer))]

namespace BottomBar.Droid.Renderers
{
    public class BottomBarPageRenderer : VisualElementRenderer<BottomBarPage>, BottomNavigationView.IOnNavigationItemSelectedListener
	{
		bool _disposed;
		BottomNavigationView _bottomBar;
        BottomNavigationMenu _menu;
        FrameLayout _frameLayout;
		IPageController _pageController;
        private LinearLayout _rootLayout;

        public BottomBarPageRenderer ()
		{
			AutoPackage = false;
		}

		#region IOnTabClickListener
		public void OnTabSelected (int position)
		{
			//Do we need this call? It's also done in OnElementPropertyChanged
			SwitchContent(Element.Children [position]);
			var bottomBarPage = Element as BottomBarPage;
			bottomBarPage.CurrentPage = Element.Children[position];
		}

		public void OnTabReSelected (int position)
		{
		}
		#endregion

		protected override void Dispose (bool disposing)
		{
			if (disposing && !_disposed) {
				_disposed = true;

				RemoveAllViews ();

				foreach (Page pageToRemove in Element.Children) {
					IVisualElementRenderer pageRenderer = Platform.GetRenderer (pageToRemove);

					if (pageRenderer != null) {
						pageRenderer.ViewGroup.RemoveFromParent ();
						pageRenderer.Dispose ();
					}
				}

				if (_bottomBar != null)
                {
                    _bottomBar.SetOnNavigationItemSelectedListener(null);
                    _bottomBar.Dispose ();
					_bottomBar = null;
				}

				if (_frameLayout != null) {
					_frameLayout.Dispose ();
					_frameLayout = null;
				}
			}

			base.Dispose (disposing);
		}

		protected override void OnAttachedToWindow ()
		{
			base.OnAttachedToWindow ();
			_pageController.SendAppearing ();
		}

		protected override void OnDetachedFromWindow ()
		{
			base.OnDetachedFromWindow ();
			_pageController.SendDisappearing ();
		}


		protected override void OnElementChanged (ElementChangedEventArgs<BottomBarPage> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement != null) {

				BottomBarPage bottomBarPage = e.NewElement;

                if (_bottomBar == null) {
                    _pageController = PageController.Create(bottomBarPage);

                    _rootLayout = new LinearLayout(Context) {
                        LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                    };
                    AddView(_rootLayout);

                    // create a view which will act as container for Page's
                    _frameLayout = new FrameLayout(Context)
                    {
                        LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                    };
                    _rootLayout.AddView(_frameLayout);

                    _bottomBar = new BottomNavigationView(Context)
                    {
                        LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                    };
                    _rootLayout.AddView(_bottomBar);

                    _bottomBar.SetOnNavigationItemSelectedListener(this);
                    _bottomBar.SetBackgroundColor(new Android.Graphics.Color(23, 31, 50));
                    
                    var stateList = new Android.Content.Res.ColorStateList(
                        new int[][] {
                            new int[] { Android.Resource.Attribute.StateChecked },
                            new int[] { Android.Resource.Attribute.StateEnabled }
                        },
                        new int[] {
                            new Android.Graphics.Color(67, 163, 245), //Selected
                            new Android.Graphics.Color(187, 188, 190) //Normal
                        });
                    _bottomBar.ItemIconTintList = stateList;
                    _bottomBar.ItemTextColor = stateList;

                    _menu = (BottomNavigationMenu) _bottomBar.Menu;

                    UpdateTabs();
				}

				if (bottomBarPage.CurrentPage != null) {
					SwitchContent (bottomBarPage.CurrentPage);
				}
			}
		}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == nameof (TabbedPage.CurrentPage)) {
				SwitchContent (Element.CurrentPage);
			}
		}

		protected virtual void SwitchContent (Page view)
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

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			int width = r - l;
			int height = b - t;

			if (width > 0 && height > 0) {
                var context = Context;
                _rootLayout.Measure(MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.AtMost), MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));
                _rootLayout.Layout(0, 0, _rootLayout.MeasuredWidth, _rootLayout.MeasuredHeight);

                _bottomBar.Measure(MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly), MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));
                int tabsHeight = (int) Context.ToPixels(59);//  Math.Min(height, Math.Max(_bottomBar.MeasuredHeight, _bottomBar.MinimumHeight));

                _frameLayout.Layout(0, 0, width, height - tabsHeight);
                
                _pageController.ContainerArea = new Rectangle(0, 0, context.FromPixels(width), context.FromPixels(_frameLayout.Height));
				_bottomBar.Layout (0, height- tabsHeight, width, height);
			}

			base.OnLayout (changed, l, t, r, b);
		}
        
		void UpdateTabs ()
		{
			// create tab items
			SetTabItems ();

			// set tab colors
			SetTabColors ();
		}

		void SetTabItems ()
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
        }


        void SetTabColors ()
		{
			//for (int i = 0; i < Element.Children.Count; ++i) {
			//	Page page = Element.Children [i];

			//	Color? tabColor = page.GetTabColor ();

			//	if (tabColor != null) {
			//		_bottomBar.MapColorForTab (i, tabColor.Value.ToAndroid ());
			//	}
			//}
		}

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var menu = (BottomNavigationMenu) _bottomBar.Menu;
            var index = menu.FindItemIndex(item.ItemId);

            var pageIndex = index % Element.Children.Count;

            OnTabSelected(pageIndex);

            return true;
        }
    }
}

