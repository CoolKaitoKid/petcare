using SQLite;

namespace petcare.Models
{
    [Table("Appointments")]
    public class Appointment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PetId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public DateTime Schedule { get; set; }

        public string Clinic { get; set; } = string.Empty;

        public string Image { get; set; } = "dog_avatar.png";

        public bool Done { get; set; } = false;
    }
}