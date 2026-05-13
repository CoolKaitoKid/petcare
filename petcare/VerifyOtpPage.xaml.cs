namespace petcare;

public partial class VerifyOtpPage : ContentPage
{
    private readonly string email;

    public VerifyOtpPage(string userEmail)
    {
        InitializeComponent();
        email = userEmail;
    }

    private async void OnResetPasswordClicked(object sender, EventArgs e)
    {
        string otp = EntryOtp.Text?.Trim() ?? "";
        string newPassword = EntryNewPassword.Text?.Trim() ?? "";
        string confirmPassword = EntryConfirmPassword.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(otp))
        {
            await DisplayAlert("Error", "Please enter the OTP code.", "OK");
            return;
        }

        if (otp.Length != 6)
        {
            await DisplayAlert("Error", "OTP must be 6 digits.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(newPassword))
        {
            await DisplayAlert("Error", "Please enter a new password.", "OK");
            return;
        }

        if (newPassword.Length < 6)
        {
            await DisplayAlert("Error", "Password must be at least 6 characters.", "OK");
            return;
        }

        if (newPassword != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        var validOtp = await App.Database.GetValidPasswordResetOtpAsync(email, otp);

        if (validOtp == null)
        {
            await DisplayAlert("Error", "Invalid or expired OTP.", "OK");
            return;
        }

        await App.Database.UpdateUserPasswordAsync(email, newPassword);
        await App.Database.MarkPasswordResetOtpUsedAsync(validOtp);

        await DisplayAlert("Success", "Password reset successfully. Please log in.", "OK");

        await Navigation.PopToRootAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}