using Microsoft.Maui.Controls.Shapes;
using petcare.Models;

namespace petcare;

public partial class HomePage : ContentPage
{
    public User CurrentUser { get; private set; }

    public HomePage()
    {
        InitializeComponent();

        CurrentUser = App.LoggedInUser ?? new User { FirstName = "Guest" };
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        CurrentUser = App.LoggedInUser ?? new User { FirstName = "Guest" };
        BindingContext = this;

        LoadAppointmentsOnHome();
    }

    private void LoadAppointmentsOnHome()
    {
        AppointmentsContainer.Children.Clear();

        var upcomingAppointments = AppData.Appointments
            .Where(a => a.Schedule >= DateTime.Now)
            .OrderBy(a => a.Schedule)
            .ToList();

        if (upcomingAppointments.Count == 0)
        {
            AppointmentsContainer.Children.Add(new Label
            {
                Text = "No upcoming appointments.",
                FontSize = 14,
                TextColor = Colors.Gray
            });

            return;
        }

        foreach (var appt in upcomingAppointments.Take(5))
        {
            AppointmentsContainer.Children.Add(CreateAppointmentCard(appt));
        }
    }

    private View CreateAppointmentCard(Appointment appt)
    {
        var content = new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 5,
            InputTransparent = true,

            Children =
            {
                new Image
                {
                    Source = string.IsNullOrWhiteSpace(appt.Image)
                        ? "dog_avatar.png"
                        : appt.Image,
                    HeightRequest = 50,
                    WidthRequest = 50,
                    Aspect = Aspect.AspectFill
                },

                new Label
                {
                    Text = string.IsNullOrWhiteSpace(appt.Title)
                        ? "Appointment"
                        : appt.Title,
                    HorizontalOptions = LayoutOptions.Center,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 13,
                    TextColor = Colors.Black
                },

                new Label
                {
                    Text = string.IsNullOrWhiteSpace(appt.Type)
                        ? "Checkup"
                        : appt.Type,
                    FontSize = 10,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Colors.Black
                },

                new Label
                {
                    Text = appt.Schedule.ToString("MMM dd, hh:mm tt"),
                    FontSize = 10,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Colors.Black
                }
            }
        };

        var card = new Border
        {
            BackgroundColor = Color.FromArgb("#D2B48C"),
            Padding = 10,
            WidthRequest = 140,
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 15
            },
            Content = content
        };

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (s, e) =>
        {
            await GoToScheduleAsync();
        };

        card.GestureRecognizers.Add(tapGesture);

        return card;
    }

    private async Task GoToScheduleAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("Schedule");
        }
        catch
        {
            await Navigation.PushAsync(new Schedule());
        }
    }

    private async Task GoToProfileAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("//ProfileOwner");
        }
        catch
        {
            await Navigation.PushAsync(new ProfileOwner());
        }
    }

    private async void OnScheduleCardTapped(object sender, TappedEventArgs e)
    {
        await GoToScheduleAsync();
    }

    private async void OnViewScheduleClicked(object sender, EventArgs e)
    {
        await GoToScheduleAsync();
    }

    private async void OnProfileAvatarTapped(object sender, TappedEventArgs e)
    {
        await GoToProfileAsync();
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("HomePage");
    }

    private async void OnPetsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("MyPets");
    }

    private async void OnAppointmentsClicked(object sender, EventArgs e)
    {
        await GoToScheduleAsync();
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await GoToProfileAsync();
    }
}