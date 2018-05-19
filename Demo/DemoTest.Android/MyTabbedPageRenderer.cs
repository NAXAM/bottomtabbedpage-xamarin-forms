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
using DemoTest;
using DemoTest.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyTabbedPage), typeof(MyTabbedPageRenderer))]
namespace DemoTest.Droid
{
    public class MyTabbedPageRenderer : TabbedRenderer
    {
        public MyTabbedPageRenderer(Context context) : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing&& Element !=null)
            {

            }
            //base.Dispose(disposing);
        }
    }
}