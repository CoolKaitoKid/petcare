
using Microsoft.Maui.Storage;
using petcare.Models;

namespace petcare;

public partial class AppointmentPage : ContentPage
{
    string imagePath = "dog_avatar.png";

    public AppointmentPage()
    {
        InitializeComponent();
    }

    private async void OnPickImage(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            imagePath = result.FullPath;
            PetImage.Source = ImageSource.FromFile(imagePath);
        }
    }

    private async void OnSave(object sender, EventArgs e)
    {
        if (TypePicker.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Select appointment type", "OK");
            return;
        }

        var appointment = new Appointment
        {
            Title = PetNameEntry.Text,
            Type = TypePicker.SelectedItem.ToString(),
            Schedule = DatePicker.Date + TimePicker.Time,
            Clinic = ClinicEntry.Text,
            Image = imagePath
        };

        AppData.Appointments.Add(appointment);

        await Navigation.PopAsync();
    }
}
