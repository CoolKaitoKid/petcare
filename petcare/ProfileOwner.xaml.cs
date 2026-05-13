using petcare.Models;

namespace petcare;

public partial class ProfileOwner : ContentPage
{
    private User? currentUser;

    public ProfileOwner()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        currentUser = App.LoggedInUser;

        if (currentUser == null)
        {
            await DisplayAlert("Error", "No logged-in user found.", "OK");
            return;
        }

        LblUserName.Text = $"{currentUser.FirstName} {currentUser.LastName}";

        LblFullName.Text = $"{currentUser.FirstName} {currentUser.LastName}";

        LblPhone.Text = string.IsNullOrWhiteSpace(currentUser.PhoneNumber)
            ? "No phone number"
            : currentUser.PhoneNumber;

        LblEmail.Text = string.IsNullOrWhiteSpace(currentUser.Email)
            ? "No email"
            : currentUser.Email;

        LblAddress.Text = string.IsNullOrWhiteSpace(currentUser.Address)
            ? "No address"
            : currentUser.Address;

        if (!string.IsNullOrWhiteSpace(currentUser.ProfilePhotoPath))
        {
            ProfileImage.Source = ImageSource.FromFile(currentUser.ProfilePhotoPath);
        }
        else
        {
            ProfileImage.Source = "profile_default.png";
        }

        SwitchPush.IsToggled = currentUser.PushNotification;
        SwitchEmail.IsToggled = currentUser.EmailUpdates;
        SwitchReminder.IsToggled = currentUser.AppReminders;
    }

    private async void OnEditProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("EditProfilePage");
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }

    private async void OnPetsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MyPets");
    }

    private async void OnAppointmentsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Schedule");
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ProfileOwner");
    }
}