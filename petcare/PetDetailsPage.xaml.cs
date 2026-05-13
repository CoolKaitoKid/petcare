using petcare.Models;

namespace petcare;

public partial class PetDetailsPage : ContentPage
{
    private Pet pet;

    public PetDetailsPage(Pet selectedPet)
    {
        InitializeComponent();

        pet = selectedPet;

        LoadPetDetails();
    }

    private void LoadPetDetails()
    {
        string photoPath = string.IsNullOrWhiteSpace(pet.PhotoPath)
            ? GetDefaultImageForPetType(pet.Type)
            : pet.PhotoPath;

        PetImage.Source = photoPath;

        PetNameLabel.Text = string.IsNullOrWhiteSpace(pet.Name)
            ? "Unnamed Pet"
            : pet.Name;

        PetBreedLabel.Text = string.IsNullOrWhiteSpace(pet.Breed)
            ? "No breed added"
            : pet.Breed;

        PetTypeLabel.Text = string.IsNullOrWhiteSpace(pet.Type)
            ? "Unknown type"
            : pet.Type;

        // These are placeholders for now unless your Pet model has these fields already.
        PetWeightLabel.Text = "Not added";
        PetGenderLabel.Text = "Not added";
        PetBirthdayLabel.Text = pet.CreatedAt == default
            ? "Not added"
            : pet.CreatedAt.ToLocalTime().ToString("MMM dd, yyyy");

        MealTypeLabel.Text = "Select meal type";
        MealsPerDayLabel.Text = "Enter meals per day";
        PortionLabel.Text = "Select portion";
        FeedingTimesLabel.Text = "Set time";

        NextVisitLabel.Text = "• None scheduled";
        ReminderLabel.Text = "• None";
        HealthStatusLabel.Text = "No health notes added";
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        string newName = await DisplayPromptAsync(
            "Edit Pet",
            "Pet name:",
            initialValue: pet.Name
        );

        if (newName == null)
            return;

        if (string.IsNullOrWhiteSpace(newName))
        {
            await DisplayAlert("Error", "Pet name cannot be empty.", "OK");
            return;
        }

        string newBreed = await DisplayPromptAsync(
            "Edit Pet",
            "Breed:",
            initialValue: pet.Breed
        );

        if (newBreed == null)
            return;

        string newType = await DisplayPromptAsync(
            "Edit Pet",
            "Pet type:",
            initialValue: pet.Type
        );

        if (newType == null)
            return;

        pet.Name = newName.Trim();
        pet.Breed = newBreed.Trim();
        pet.Type = string.IsNullOrWhiteSpace(newType)
            ? "Other"
            : newType.Trim();

        await App.Database.UpdatePetAsync(pet);

        await DisplayAlert("Success", "Pet profile updated!", "OK");

        LoadPetDetails();
    }

    private async void OnEditFeedingClicked(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Feeding Plan",
            "Feeding plan editing is not connected yet. You need to add feeding fields to your database/model first.",
            "OK"
        );
    }

    private async void OnEditVisitClicked(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Vet Visit",
            "Vet visit editing is not connected yet. You need to add vet visit fields to your database/model first.",
            "OK"
        );
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        string petName = string.IsNullOrWhiteSpace(pet.Name)
            ? "this pet"
            : pet.Name;

        bool confirm = await DisplayAlert(
            "Delete Pet",
            $"Are you sure you want to delete {petName}?",
            "Delete",
            "Cancel"
        );

        if (!confirm)
            return;

        await App.Database.DeletePetAsync(pet);

        await DisplayAlert("Deleted", "Pet deleted successfully.", "OK");

        await Navigation.PopAsync();
    }

    private string GetDefaultImageForPetType(string type)
    {
        return type?.Trim().ToLowerInvariant() switch
        {
            "cat" => "cat_avatar.png",
            "dog" => "dog_avatar.png",
            _ => "others.jpg"
        };
    }
}