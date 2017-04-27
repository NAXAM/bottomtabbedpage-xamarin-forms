using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Naxam.BottomNavs.Platform.Droid;
using Android.Graphics;

namespace Demo.Droid
{
    [Activity(Label = "Demo", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            var stateList = new Android.Content.Res.ColorStateList(
                new int[][] {
                    new int[] { Android.Resource.Attribute.StateChecked },
                    new int[] { Android.Resource.Attribute.StateEnabled }
                },
                new int[] {
                    new Android.Graphics.Color(67, 163, 245), //Selected
                    new Android.Graphics.Color(187, 188, 190) //Normal
                });

            BottomTabbedRenderer.BackgroundColor = new Android.Graphics.Color(23, 31, 50);
            BottomTabbedRenderer.FontSize = 10;
            BottomTabbedRenderer.IconSize = 20;
            BottomTabbedRenderer.ItemTextColor = stateList;
            BottomTabbedRenderer.ItemIconTintList = stateList;
            BottomTabbedRenderer.Typeface = Typeface.CreateFromAsset(this.Assets, "HiraginoKakugoProNW3.otf");
            BottomTabbedRenderer.ItemBackgroundResource = Resource.Drawable.bnv_selector;
            BottomTabbedRenderer.ItemPadding = 4;
            BottomTabbedRenderer.BottomBarHeight = 60;
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new Demo.App());
        }
    }
}

