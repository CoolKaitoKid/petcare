using Microsoft.Maui.Controls.Shapes;
using petcare.Models;

namespace petcare;

public partial class MyPets : ContentPage
{
    private List<Pet> allPets = new();

    public MyPets()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPets();
    }

    private async Task LoadPets()
    {
        if (App.LoggedInUser == null)
        {
            allPets = new List<Pet>();
            DisplayPets(allPets);
            await DisplayAlert("Error", "No logged-in user found.", "OK");
            return;
        }

        allPets = await App.Database.GetPetsAsync(App.LoggedInUser.Id) ?? new List<Pet>();

        // latest pets first
        allPets = allPets.OrderByDescending(p => p.CreatedAt).ToList();

        DisplayPets(allPets);
    }

    private void DisplayPets(List<Pet> pets)
    {
        PetsContainer.Children.Clear();

        if (pets.Count == 0)
        {
            PetsContainer.Children.Add(new Label
            {
                Text = "No pets added yet.",
                FontSize = 16,
                TextColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20, 0, 0)
            });
            return;
        }

        foreach (var pet in pets)
        {
            PetsContainer.Children.Add(CreatePetCard(pet));
        }
    }

    private View CreatePetCard(Pet pet)
    {
        string photoPath = string.IsNullOrWhiteSpace(pet.PhotoPath) ? "dog_avatar.png" : pet.PhotoPath;
        string petName = string.IsNullOrWhiteSpace(pet.Name) ? "Unnamed Pet" : pet.Name;
        string breed = string.IsNullOrWhiteSpace(pet.Breed) ? "No breed added" : pet.Breed;
        string type = string.IsNullOrWhiteSpace(pet.Type) ? "Unknown type" : pet.Type;
        string addedDate = pet.CreatedAt == default ? "Unknown date" : pet.CreatedAt.ToLocalTime().ToString("MMM dd, yyyy");

        // Image and Name Labels
        var image = new Image
        {
            Source = photoPath,
            HeightRequest = 80,
            WidthRequest = 80,
            Aspect = Aspect.AspectFill
        };

        var nameLabel = new Label
        {
            Text = petName,
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.Black
        };

        // Tap gesture to open full pet details page
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (s, e) =>
        {
            await Navigation.PushAsync(new PetDetailsPage(pet));
        };
        image.GestureRecognizers.Add(tapGesture);
        nameLabel.GestureRecognizers.Add(tapGesture);


        return new Border
        {
            StrokeThickness = 0,
            BackgroundColor = Colors.White,
            StrokeShape = new RoundRectangle { CornerRadius = 18 },
            Padding = 12,
            Margin = new Thickness(0, 0, 0, 10),
            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Children =
                {
                    new HorizontalStackLayout
                    {
                        Spacing = 15,
                        Children =
                        {
                            image,
                            new VerticalStackLayout
                            {
                                Spacing = 4,
                                VerticalOptions = LayoutOptions.Center,
                                Children =
                                {
                                    nameLabel,
                                    new Label { Text = $"Type: {type}", FontSize = 14, TextColor = Colors.DarkGray },
                                    new Label { Text = $"Breed: {breed}", FontSize = 14, TextColor = Colors.Gray },
                                    new Label { Text = $"Added: {addedDate}", FontSize = 12, TextColor = Colors.Gray }
                                }
                            }
                        }
                    },
                    new HorizontalStackLayout
                    {
                        Spacing = 10,
                        HorizontalOptions = LayoutOptions.End
                    }
                }
            }
        };
    }

    private void OnSearchChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(searchText))
        {
            DisplayPets(allPets);
            return;
        }

        var filtered = allPets
            .Where(p =>
                (!string.IsNullOrWhiteSpace(p.Name) &&
                 p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(p.Breed) &&
                 p.Breed.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(p.Type) &&
                 p.Type.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        DisplayPets(filtered);
    }

    private async void OnAddPetClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SelectPetTypePage());
    }

}