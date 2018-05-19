using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyTabbedPage : TabbedPage
    {
        public MyTabbedPage ()
        {
            InitializeComponent();
            this.Appearing += MyTabbedPage_Appearing;
            this.Disappearing += MyTabbedPage_Disappearing;
        }

        private void MyTabbedPage_Disappearing(object sender, EventArgs e)
        {
            Debug.WriteLine("MyTabbedPage_Disappearing");
        }

        private void MyTabbedPage_Appearing(object sender, EventArgs e)
        {
            Debug.WriteLine("MyTabbedPage_Appearing");
        }

            private void Button_Clicked(object sender, EventArgs e)
            {
                App.Current.MainPage = new NavigationPage(new MainPage());
            }
    }
}