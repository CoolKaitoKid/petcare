using petcare.Models;

namespace petcare;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string email = TxtEmail.Text?.Trim() ?? string.Empty;
        string firstName = TxtFirstName.Text?.Trim() ?? string.Empty;
        string lastName = TxtLastName.Text?.Trim() ?? string.Empty;
        string password = TxtPassword.Text ?? string.Empty;
        string confirmPassword = TxtConfirmPassword.Text ?? string.Empty;

        // Validation
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }

        if (!email.Contains("@") || !email.Contains("."))
        {
            await DisplayAlert("Error", "Please enter a valid email address.", "OK");
            return;
        }

        if (password.Length < 6)
        {
            await DisplayAlert("Error", "Password must be at least 6 characters.", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        // Create a User object
        var newUser = new User
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            PasswordHash = password // In production, hash this!
        };

        try
        {
            // Call the database method with a User object
            await App.Database.RegisterUserAsync(newUser);

            await DisplayAlert("Success", "Registration successful!", "OK");

            // Navigate back (or to login/home page)
            await Shell.Current.GoToAsync("LoginPage");
        }
        catch (Exception ex)
        {
            // Handle email already exists or other errors
            await DisplayAlert("Registration Failed", ex.Message, "OK");
        }
    }

    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("LoginPage");
    }
}