using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Naxam.Demo
{
    public partial class TestPage : ContentPage
    {
        public TestPage (bool allowsNav=true)
        {
            InitializeComponent ();

            btnNav.Clicked += async delegate {
                if (!allowsNav) {
                    return;
                }

                await Navigation?.PushAsync (new TestPage {
                    Title = Title + ":1"
                }, true);
            };
        }
    }
}
