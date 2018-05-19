using Naxam.Controls.Forms;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace DemoTest
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new MainPage());
            //InitTabPage();

        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        private void InitTabPage()
        {
            var tabs = new BottomTabbedPage();

            //var icons = Plugin.Iconize.Iconize.Modules.FirstOrDefault()?.Keys.Take(5) ?? new string[0];
            tabs.Children.Add(new ContentPage
            {
                Title = "Tab 1",
                Icon = "ic_audiotrack_black_24dp",
                BackgroundColor = Color.Aqua,
                Content = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = "BottomTabbedPage - A Xamarin.Forms page with tabs at the bottom.",
                    TextColor = Color.DarkCyan,
                    Margin = new Thickness(16)
                }
            });
            tabs.Children.Add(new ContentPage
            {
                Title = "Tab 2",
                Icon = "ic_backup_black_24dp",
                BackgroundColor = Color.Beige,
                Content = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = "BottomTabbedPage internally uses BottomNavigationViewEx.",
                    TextColor = Color.Green,
                    Margin = new Thickness(16)
                }
            });
            tabs.Children.Add(new ContentPage
            {
                Title = "Tab 3",
                Icon = "ic_camera_black_24dp",
                BackgroundColor = Color.BlueViolet,
                Content = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = "BottomTabbedPage could be embedded inside a NavigationPage.",
                    TextColor = Color.Aqua,
                    Margin = new Thickness(16)
                }
            });
            var stackLayout = new StackLayout();
            stackLayout.Children.Add(new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Text = "BottomTabbedPage is a product developed by NAXAM",
                TextColor = Color.DarkGreen,
                Margin = new Thickness(16)
            });
            stackLayout.Children.Add(new Button
            {
                Text = "ÍË³ö",
                Command = new Command(() =>
                {
                    MainPage = new NavigationPage(new MainPage());
                })
            });
            tabs.Children.Add(new ContentPage
            {
                Title = "Tab 4",
                Icon = "ic_favorite_black_24dp",
                BackgroundColor = Color.Bisque,
                Content = stackLayout

            });

            MainPage = tabs;
        }
    }
}
