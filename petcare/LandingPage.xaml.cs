namespace petcare;

public partial class LandingPage : ContentPage
{
    public LandingPage()
    {
        InitializeComponent();
    }

    // This is the missing event handler
    private async void OnGetStartedClicked(object sender, EventArgs e)
    {
        // Navigate to LoginPage (or RegisterPage as needed)
        await Shell.Current.GoToAsync("LoginPage");
    }
}