using SQLite;
using System;

namespace petcare.Models
{
    [Table("Pets")]
    public class Pet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Breed { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public double Weight { get; set; }

        public DateTime Birthday { get; set; }

        public string MealType { get; set; } = string.Empty;

        public int MealsPerDay { get; set; }

        public string PortionPerMeal { get; set; } = string.Empty;

        public TimeSpan FeedingTime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string PhotoPath { get; set; } = "dog_avatar.png";
    }
}