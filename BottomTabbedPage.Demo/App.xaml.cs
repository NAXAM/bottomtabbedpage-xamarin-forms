using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Naxam.BottomTabbedPage.Demo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var page = new Naxam.Controls.Forms.BottomTabbedPage();

            page.Children.Add(new NavigationPage (
                new TestPage()) {
                Title = "Test",
                Icon = "icon.png"
            });
			page.Children.Add(new TestPage()
			{
				Title = "Test",
				Icon = "icon.png"
			});

			MainPage = page;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
