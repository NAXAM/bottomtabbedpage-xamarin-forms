using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Naxam.Demo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var page = new Naxam.Controls.Forms.BottomTabbedPage();

            page.Children.Add(new NavigationPage (
                new TestPage()) {
                Title = "Test 1 Test 1 Test 1",
                Icon = "icon.png"
            });
            page.Children.Add(new NavigationPage(
               new TestPage())
            {
                Title = "Test 2 Test 2 Test 2",
                Icon = "icon.png"
            });
            page.Children.Add(new TestPage(false)
			{
				Title = "Test 3",
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
