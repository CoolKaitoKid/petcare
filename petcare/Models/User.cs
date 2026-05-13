using SQLite;

namespace petcare.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string PasswordSalt { get; set; } = string.Empty;

        // NEW FIELDS
        public string PhoneNumber { get; set; } = string.Empty;

        public string ProfilePhotoPath { get; set; } = "";

        public string Address { get; set; } = string.Empty;

        public bool PushNotification { get; set; } = true;

        public bool EmailUpdates { get; set; } = true;

        public bool AppReminders { get; set; } = true;
    }
}