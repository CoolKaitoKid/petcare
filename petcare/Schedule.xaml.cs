using petcare.Models;

namespace petcare;

public partial class Schedule : ContentPage
{
    public Schedule()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadAppointments();
    }

    private void LoadAppointments()
    {
        AppointmentsContainer.Children.Clear();

        foreach (var appt in AppData.Appointments)
        {
            AppointmentsContainer.Children.Add(CreateCard(appt));
        }
    }
    private async void OnAddAppointmentClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AppointmentPage());
    }

    private View CreateCard(Appointment appt)
    {
        return new Border
        {
            Padding = 10,
            StrokeThickness = 0,
            BackgroundColor = Color.FromArgb("#D3C8BC"),
            Content = new HorizontalStackLayout
            {
                Children =
                {
                    new Image
                    {
                        Source = appt.Image,
                        HeightRequest = 45,
                        WidthRequest = 45
                    },
                    new VerticalStackLayout
                    {
                        Children =
                        {
                            new Label { Text = appt.Title, FontAttributes = FontAttributes.Bold },
                            new Label { Text = appt.Type },
                            new Label { Text = appt.Schedule.ToString("MMM dd yyyy hh:mm tt") },
                            new Label { Text = appt.Clinic }
                        }
                    }
                }
            }
        };
    }
}