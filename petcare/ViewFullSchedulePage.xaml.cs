using petcare.Models;

namespace petcare;

public partial class ViewFullSchedulePage : ContentPage
{
    private List<Appointment> allAppointments = new();

    // Track logged-in user for greeting
    public User CurrentUser { get; private set; }

    // Track current page for bottom navigation
    private string currentPage = "Appointments";

    // Dynamic icon sources for bottom nav
    public string HomeIcon => currentPage == "Home" ? "home_active.png" : "home.png";
    public string PetsIcon => currentPage == "Pets" ? "pawprint_active.png" : "pawprint.png";
    public string AppointmentsIcon => currentPage == "Appointments" ? "calendar_active.png" : "calendar.png";
    public string ProfileIcon => currentPage == "Profile" ? "user_active.png" : "user.png";

    public ViewFullSchedulePage()
    {
        InitializeComponent();

        // Assign logged-in user
        CurrentUser = App.LoggedInUser ?? new User { FirstName = "Guest" };

        // Use the page itself as BindingContext
        BindingContext = this;

        LoadAppointments();
    }

    private async void LoadAppointments()
    {
        var userId = App.LoggedInUser?.Id;
        if (userId == null)
        {
            await DisplayAlert("Error", "No logged-in user found.", "OK");
            return;
        }

        try
        {
            allAppointments = await App.Database.GetUserAppointmentsAsync(userId.Value);
            AppointmentsList.ItemsSource = allAppointments.OrderBy(a => a.Schedule).ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load appointments: {ex.Message}", "OK");
        }
    }

    private async void OnAppointmentClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Appointment app)
        {
            await DisplayAlert("Appointment",
                $"Title: {app.Title}\nType: {app.Type}\nTime: {app.Schedule}",
                "OK");
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string search = e.NewTextValue?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(search))
        {
            AppointmentsList.ItemsSource = allAppointments.OrderBy(a => a.Schedule).ToList();
        }
        else
        {
            AppointmentsList.ItemsSource = allAppointments
                .Where(a => a.Title.Contains(search, StringComparison.OrdinalIgnoreCase)
                         || a.Type.Contains(search, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.Schedule)
                .ToList();
        }
    }

    private async void OnViewFullScheduleClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ViewFullSchedulePage");
    }

    // Bottom navigation handlers
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        SetCurrentPage("Home");
        await Shell.Current.GoToAsync("HomePage");
    }

    private async void OnPetsClicked(object sender, EventArgs e)
    {
        SetCurrentPage("Pets");
        await Shell.Current.GoToAsync("MyPets");
    }

    private async void OnAppointmentsClicked(object sender, EventArgs e)
    {
        SetCurrentPage("Appointments");
        await Shell.Current.GoToAsync("ViewFullSchedulePage");
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        SetCurrentPage("Profile");
        await Shell.Current.GoToAsync("ProfileOwner");
    }

    private void SetCurrentPage(string page)
    {
        currentPage = page;
        OnPropertyChanged(nameof(HomeIcon));
        OnPropertyChanged(nameof(PetsIcon));
        OnPropertyChanged(nameof(AppointmentsIcon));
        OnPropertyChanged(nameof(ProfileIcon));
    }
}