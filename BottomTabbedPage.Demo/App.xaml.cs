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

            var page = new TabbedPage();

			var page1Content = new Grid
			{
				BackgroundColor = Color.Blue,
                VerticalOptions = LayoutOptions.Fill
			};
			page1Content.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(int.MaxValue, GridUnitType.Star) });
			page1Content.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(int.MaxValue, GridUnitType.Star) });

            page1Content.Children.Add(new BoxView {
                BackgroundColor = Color.Azure,
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill
			});
            Grid.SetRow(page1Content.Children[0], 0);

			page1Content.Children.Add(new BoxView
			{
				BackgroundColor = Color.Violet,
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill
			});
			Grid.SetRow(page1Content.Children[1], 1);

            page1Content.Children.Add(new Label {
                Text = "Page 1",
				TextColor = Color.White,
				FontSize = 36,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
			});
			Grid.SetRow(page1Content.Children[2], 0);
            Grid.SetRowSpan(page1Content.Children[2], 2);

            page.Children.Add(new NavigationPage(new ContentPage {
                Content = page1Content
            }) {
                Title = "Page 1",
                Icon = "icon.png"
            });

            var page2Content = new Grid {
                BackgroundColor = Color.Red,
				VerticalOptions = LayoutOptions.Fill
			};
            page2Content.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(int.MaxValue, GridUnitType.Star) });
			page2Content.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(int.MaxValue, GridUnitType.Star) });

			page2Content.Children.Add(new BoxView
			{
                BackgroundColor = Color.BurlyWood,
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill
			});
			Grid.SetRow(page2Content.Children[0], 0);
			page2Content.Children.Add(new BoxView
			{
                BackgroundColor = Color.Magenta,
				VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill
			});
			Grid.SetRow(page2Content.Children[1], 1);

            page2Content.Children.Add(new Label {
                Text = "Page 2",
                TextColor = Color.White,
                FontSize = 36,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            });
            Grid.SetRow(page2Content.Children[2], 0);
            Grid.SetRowSpan(page2Content.Children[2], 2);

            page.Children.Add(new ContentPage()
            {
                Title = "Page 2",
                Icon = "icon.png",
                Content = page2Content
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
