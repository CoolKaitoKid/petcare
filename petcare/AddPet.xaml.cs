using petcare.Models;

namespace petcare;

public partial class AddPet : ContentPage
{
    private string petType = "Dog";
    private string imagePath = "dog_avatar.png";
    private int currentStep = 1;

    public AddPet(string selectedPetType)
    {
        InitializeComponent();

        petType = selectedPetType?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(petType))
        {
            petType = "Other";
        }

        imagePath = GetDefaultImageForPetType(petType);
        PreviewImage.Source = imagePath;

        ShowNameStep();
    }

    private void ShowNameStep()
    {
        currentStep = 1;

        StepNameView.IsVisible = true;
        StepPhotoView.IsVisible = false;

        SkipLabel.IsVisible = false;
        BottomButton.IsVisible = true;
        BottomButton.Text = "That’s my buddy!";

        PetNameQuestionLabel.Text = $"What’s your {petType.ToLower()}’s name?";
        EntryPetName.Focus();
    }

    private void ShowPhotoStep()
    {
        currentStep = 2;

        StepNameView.IsVisible = false;
        StepPhotoView.IsVisible = true;

        SkipLabel.IsVisible = true;
        BottomButton.IsVisible = true;
        BottomButton.Text = "Save";
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (currentStep == 1)
        {
            await Navigation.PopAsync();
        }
        else if (currentStep == 2)
        {
            ShowNameStep();
        }
    }

    private async void OnBottomButtonClicked(object sender, EventArgs e)
    {
        if (currentStep == 1)
        {
            string petName = EntryPetName.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(petName))
            {
                await DisplayAlert("Error", "Please enter your pet's name.", "OK");
                return;
            }

            ShowPhotoStep();
            return;
        }

        if (currentStep == 2)
        {
            await SavePetAsync();
        }
    }

    private async void OnPickPhoto(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick a pet photo",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                imagePath = result.FullPath;
                PreviewImage.Source = ImageSource.FromFile(imagePath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnSkipClicked(object sender, EventArgs e)
    {
        await SavePetAsync();
    }

    private async Task SavePetAsync()
    {
        if (App.LoggedInUser == null)
        {
            await DisplayAlert("Error", "No logged-in user found.", "OK");
            return;
        }

        string petName = EntryPetName.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(petName))
        {
            await DisplayAlert("Error", "Please enter your pet's name.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(petType))
        {
            petType = "Other";
        }

        Pet newPet = new Pet
        {
            UserId = App.LoggedInUser.Id,
            Name = petName,
            Breed = "",
            Type = petType,
            PhotoPath = imagePath,
            CreatedAt = DateTime.UtcNow
        };

        await App.Database.AddPetAsync(newPet);

        await DisplayAlert("Success", "Pet added!", "OK");

        await Navigation.PopToRootAsync();
    }

    private string GetDefaultImageForPetType(string type)
    {
        return type.Trim().ToLowerInvariant() switch
        {
            "cat" => "cat_avatar.png",
            "dog" => "dog_avatar.png",
            _ => "others.jpg"
        };
    }
}