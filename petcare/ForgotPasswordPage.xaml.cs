using petcare.Models;


namespace petcare;

public partial class ForgotPasswordPage : ContentPage
{
    public ForgotPasswordPage()
    {
        InitializeComponent();
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        string email = EntryEmail.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(email))
        {
            await DisplayAlert("Error", "Please enter your email.", "OK");
            return;
        }

        var user = await App.Database.GetUserByEmailAsync(email);

        if (user == null)
        {
            await DisplayAlert("Error", "No account found with this email.", "OK");
            return;
        }

        string otpCode = Random.Shared.Next(100000, 999999).ToString();

        var resetOtp = new PasswordResetOtp
        {
            Email = email,
            Code = otpCode,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false
        };

        await App.Database.SavePasswordResetOtpAsync(resetOtp);

        var result = await EmailOtpService.SendOtpAsync(email, otpCode);

        if (!result.Success)
        {
            await DisplayAlert("OTP Error", result.Message, "OK");
            return;
        }

        await DisplayAlert("OTP Sent", "Please check your email for the OTP code.", "OK");

        await Navigation.PushAsync(new VerifyOtpPage(email));
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}