using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Naxam.Demo
{
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();

            btnNav.Clicked += async delegate {
                await Navigation?.PushAsync(new TestPage {
                    Title = Title + ":1"
                }, true);
            };
        }
    }
}
