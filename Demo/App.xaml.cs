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
            var fPage = new TestPage {
                Title = "Test 1"
            };
            fPage.ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command((obj) =>
                {
                    //               var currentIndex = page.Children.IndexOf(page.CurrentPage);

                    //if (currentIndex == 0)
                    //{
                    //	return;
                    //}

                    //var leftPage = page.Children[currentIndex - 1];
                    //page.Children[currentIndex - 1] = page.CurrentPage;
                    //page.Children[currentIndex] = leftPage;

                    page.Children.Add(new NavigationPage(
               new TestPage
               {
                   Title = "Test X1"
               })
                    {
                        Title = "Test X1",
                        Icon = "icon.png"
                    });

                    page.Children.Add(new NavigationPage(
               new TestPage
               {
                   Title = "Test X2"
               })
                    {
                        Title = "Test X2",
                        Icon = "icon.png"
                    });
                }),
                Text = "Add"
            });
            fPage.ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command((obj) =>
                {
                    //               var currentIndex = page.Children.IndexOf(page.CurrentPage);

                    //               if (currentIndex == page.Children.Count - 1) {
                    //                   return;
                    //               }

                    //var nextPage = page.Children[currentIndex + 1];
                    //page.Children[currentIndex + 1] = page.CurrentPage;
                    //               page.Children[currentIndex] = nextPage;
                    page.Children.Remove(page.CurrentPage);
                }),
                Text = "Remove"
            });

            page.Children.Add(new NavigationPage(fPage)
            {
                Title = "Test 1 Test 1 Test 1",
                Icon = "icon.png"
            });
            page.Children.Add(new NavigationPage(
			   new TestPage
			   {
				   Title = "Test 2"
			   })
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
