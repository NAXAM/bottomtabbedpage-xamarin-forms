using Naxam.Controls.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : BottomTabbedPage
    {
        public MainTabbedPage ()
        {
            InitializeComponent();
            Children.Add(new ContentPage
            {
                Title = "聊天",
                Icon = "Icon"
            });
            Children.Add(new ContentPage
            {
                Title = "工作台",
                Icon = "Icon",
                Content = GetStackLayout()
            });
        }

        private StackLayout GetStackLayout()
        {
            var stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center
            };
            var button = new Button { Text = "退出" };
            button.Clicked += Button_Clicked;
            stackLayout.Children.Add(button);
            return stackLayout;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}