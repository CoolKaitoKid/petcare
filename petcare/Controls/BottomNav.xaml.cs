using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;

namespace petcare.Controls
{
    public partial class BottomNav : ContentView
    {
        public static readonly BindableProperty CurrentPageProperty =
            BindableProperty.Create(
                nameof(CurrentPage),
                typeof(string),
                typeof(BottomNav),
                "Home",
                propertyChanged: OnCurrentPageChanged);

        public string CurrentPage
        {
            get => (string)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        public string HomeIcon =>
            CurrentPage == "Home"
            ? "home_active.png"
            : "home.png";

        public string PetsIcon =>
            CurrentPage == "Pets"
            ? "pawprint_active.png"
            : "pawprint.png";

        public string AppointmentsIcon =>
            CurrentPage == "Appointments"
            ? "calendar_active.png"
            : "calendar.png";

        public string ProfileIcon =>
            CurrentPage == "Profile"
            ? "user_active.png"
            : "user.png";

        public BottomNav()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private static void OnCurrentPageChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var nav = (BottomNav)bindable;

            nav.OnPropertyChanged(nameof(HomeIcon));
            nav.OnPropertyChanged(nameof(PetsIcon));
            nav.OnPropertyChanged(nameof(AppointmentsIcon));
            nav.OnPropertyChanged(nameof(ProfileIcon));
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(HomePage));
        }

        private async void OnPetsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MyPets));
        }

        private async void OnAppointmentsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Schedule));
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ProfileOwner));
        }
    }
}

