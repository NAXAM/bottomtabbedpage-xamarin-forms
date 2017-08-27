﻿
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Naxam.Controls.Platform.Droid;
using Naxam.Demo;
using Plugin.Iconize;
using Plugin.Iconize.Droid.Controls;

namespace Naxam.IconizeDemo.Droid
{
    [Activity(Label = "IconizedBottomTabbedQs", Icon = "@mipmap/ic_launcher", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            SetupBottomTabs();

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Plugin.Iconize.Iconize
                  .With(new Plugin.Iconize.Fonts.FontAwesomeModule());

            //Plugin.Iconize.Iconize.Init(Resource.Id.Toolbar, Resource.Id.Tabbar);
            LoadApplication(new App());
        }

        void SetupBottomTabs()
        {
            var stateList = new Android.Content.Res.ColorStateList(
                new int[][] {
                    new int[] { Android.Resource.Attribute.StateChecked
                },
                new int[] { Android.Resource.Attribute.StateEnabled
                }
                },
                new int[] {
                    Color.White, //Selected
                    Color.White //Normal
	            });

            BottomTabbedRenderer.BackgroundColor = new Color(0x9C, 0x27, 0xB0);
            BottomTabbedRenderer.FontSize = 12f;
            BottomTabbedRenderer.IconSize = 16;
            BottomTabbedRenderer.ItemTextColor = stateList;
            BottomTabbedRenderer.ItemIconTintList = stateList;
            BottomTabbedRenderer.Typeface = Typeface.CreateFromAsset(this.Assets, "architep.ttf");
            BottomTabbedRenderer.ItemBackgroundResource = Resource.Drawable.bnv_selector;
            BottomTabbedRenderer.ItemSpacing = 4;
            //BottomTabbedRenderer.ItemPadding = new Xamarin.Forms.Thickness(6);
            BottomTabbedRenderer.BottomBarHeight = 56;
            BottomTabbedRenderer.ItemAlign = ItemAlignFlags.Center;
            BottomTabbedRenderer.MenuItemIconSetter = (menuItem, iconSource, selected) => {
				var iconized = Iconize.FindIconForKey(iconSource.File);
				if (iconized == null)
				{
                    BottomTabbedRenderer.DefaultMenuItemIconSetter.Invoke(menuItem, iconSource, selected);

					return;
				}

                var drawable = new IconDrawable(this, iconized).Color(Color.White).SizeDp(30);

				menuItem.SetIcon(drawable);
            };
        }
    }
}
