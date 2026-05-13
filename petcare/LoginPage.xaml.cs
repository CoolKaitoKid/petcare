using System;

namespace petcare;

public partial class LoginPage : ContentPage
{
    private bool _isPasswordVisible = false;

    public LoginPage()
    {
        InitializeComponent();

        TxtPassword.IsPassword = true;
        BtnTogglePassword.Source = ImageSource.FromFile("eye_closed.png");
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = TxtEmail.Text?.Trim() ?? "";
        string password = TxtPassword.Text?.Trim() ?? "";

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Please enter both username and password.", "OK");
            return;
        }

        var user = await App.Database.LoginUserAsync(email, password);

        if (user != null)
        {
            App.LoggedInUser = user;
            await Shell.Current.GoToAsync("HomePage");
        }
        else
        {
            await DisplayAlert("Error", "Invalid username or password.", "OK");
        }
    }

    private async void OnForgotPasswordTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ForgotPasswordPage());
    }

    private async void GoToRegister(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("RegisterPage");
    }

    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;

        TxtPassword.IsPassword = !_isPasswordVisible;

        BtnTogglePassword.Source = ImageSource.FromFile(
            _isPasswordVisible ? "eye_open.png" : "eye_closed.png"
        );
    }
}