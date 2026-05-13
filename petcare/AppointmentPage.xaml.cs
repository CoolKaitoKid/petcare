using Microsoft.Maui.Storage;
using petcare.Models;

namespace petcare;

public partial class AppointmentPage : ContentPage
{
    private string imagePath = "dog_avatar.png";

    public AppointmentPage()
    {
        InitializeComponent();
    }

    private async void OnPickImage(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Choose pet image"
            });

            if (result != null)
            {
                imagePath = result.FullPath;
                PetImage.Source = ImageSource.FromFile(imagePath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnSave(object sender, EventArgs e)
    {
        string petName = PetNameEntry.Text?.Trim() ?? "";
        string clinic = ClinicEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(petName))
        {
            await DisplayAlert("Error", "Please enter pet name.", "OK");
            return;
        }

        if (TypePicker.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Select appointment type.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(clinic))
        {
            await DisplayAlert("Error", "Please enter clinic name.", "OK");
            return;
        }

        DateTime appointmentDateTime = DatePicker.Date + TimePicker.Time;

        var appointment = new Appointment
        {
            Title = petName,
            Type = TypePicker.SelectedItem?.ToString() ?? "Checkup",
            Schedule = appointmentDateTime,
            Clinic = clinic,
            Image = imagePath,
            Done = false
        };

        AppData.Appointments.Add(appointment);

        await DisplayAlert("Success", "Appointment saved!", "OK");

        await Shell.Current.GoToAsync("HomePage");
    }
}