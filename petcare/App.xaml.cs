using petcare.Models;


namespace petcare
{
    public partial class App : Application
    {

        public static UserDatabase Database { get; private set; } = null!;
        public static User? LoggedInUser { get; set; }
        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "petcare.db");
            Database = new UserDatabase(dbPath);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}


