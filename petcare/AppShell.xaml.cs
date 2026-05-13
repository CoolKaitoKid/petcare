using petcare.Controls;

namespace petcare
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
           
            InitializeComponent();
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(Register));
            Routing.RegisterRoute("HomePage", typeof(HomePage));
            
            Routing.RegisterRoute("MyPets", typeof(MyPets));
            Routing.RegisterRoute("ViewFullSchedule", typeof(ViewFullSchedulePage));
            Routing.RegisterRoute("ProfileOwner", typeof(ProfileOwner));
            Routing.RegisterRoute("Schedule", typeof(Schedule));
            Routing.RegisterRoute("BottomNav", typeof(BottomNav));
            Routing.RegisterRoute("AddPetPage", typeof(AddPet));
            Routing.RegisterRoute("SelectPetTypePage", typeof(SelectPetTypePage));
            Routing.RegisterRoute("EditProfilePage", typeof(EditProfilePage));
        }
        
        public static async Task DisplayToastAsync(string message)
        {
            // Kani nga linya mo-satisfy sa 'async' requirement para mawala ang warning
            await Task.CompletedTask;
        }
    }
}
