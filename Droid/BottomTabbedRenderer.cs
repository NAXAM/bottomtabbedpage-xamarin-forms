using System;
using Naxam.BottomTabbedPage.Demo.Droid;
using Xamarin.Forms;

[assembly:ExportRenderer(typeof(TabbedPage), typeof(BottomTabbedRenderer))]
namespace Naxam.BottomTabbedPage.Demo.Droid
{
    public class BottomTabbedRenderer : Xamarin.Forms.Platform.Android.VisualElementRenderer<TabbedPage>
    {
        
    }
}