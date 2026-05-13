using petcare.Models;

namespace petcare;

public partial class EditProfilePage : ContentPage
{
    private User? currentUser;
    private string selectedPhotoPath = "";

    public EditProfilePage()
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
            await Navigation.PopAsync();
            return;
        }

        LoadUserInfo();
    }

    private void LoadUserInfo()
    {
        if (currentUser == null)
            return;

        EntryFirstName.Text = currentUser.FirstName;
        EntryLastName.Text = currentUser.LastName;
        EntryPhone.Text = currentUser.PhoneNumber;
        EntryEmail.Text = currentUser.Email;
        EntryAddress.Text = currentUser.Address;

        EditSwitchPush.IsToggled = currentUser.PushNotification;
        EditSwitchEmail.IsToggled = currentUser.EmailUpdates;
        EditSwitchReminder.IsToggled = currentUser.AppReminders;

        selectedPhotoPath = currentUser.ProfilePhotoPath ?? "";

        if (!string.IsNullOrWhiteSpace(selectedPhotoPath))
        {
            EditProfileImage.Source = ImageSource.FromFile(selectedPhotoPath);
        }
        else
        {
            EditProfileImage.Source = "profile_default.png";
        }
    }

    private async void OnChangePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Choose profile picture",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null)
                return;

            selectedPhotoPath = result.FullPath;
            EditProfileImage.Source = ImageSource.FromFile(selectedPhotoPath);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (currentUser == null)
        {
            await DisplayAlert("Error", "No logged-in user found.", "OK");
            return;
        }

        string firstName = EntryFirstName.Text?.Trim() ?? "";
        string lastName = EntryLastName.Text?.Trim() ?? "";
        string phone = EntryPhone.Text?.Trim() ?? "";
        string email = EntryEmail.Text?.Trim() ?? "";
        string address = EntryAddress.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(firstName))
        {
            await DisplayAlert("Error", "First name is required.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            await DisplayAlert("Error", "Last name is required.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            await DisplayAlert("Error", "Email is required.", "OK");
            return;
        }

        currentUser.FirstName = firstName;
        currentUser.LastName = lastName;
        currentUser.PhoneNumber = phone;
        currentUser.Email = email;
        currentUser.Address = address;
        currentUser.ProfilePhotoPath = selectedPhotoPath;

        currentUser.PushNotification = EditSwitchPush.IsToggled;
        currentUser.EmailUpdates = EditSwitchEmail.IsToggled;
        currentUser.AppReminders = EditSwitchReminder.IsToggled;

        await App.Database.UpdateUserAsync(currentUser);

        App.LoggedInUser = currentUser;

        await DisplayAlert("Success", "Profile updated!", "OK");

        await Navigation.PopAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}