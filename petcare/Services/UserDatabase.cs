using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using petcare.Models;

namespace petcare.Models
{
    public class UserDatabase
    {
        private readonly SQLiteAsyncConnection db;

        public UserDatabase(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);

            db.CreateTableAsync<User>().Wait();
            db.CreateTableAsync<Pet>().Wait();
            db.CreateTableAsync<Appointment>().Wait();
            db.CreateTableAsync<PasswordResetOtp>().Wait();
        }

        // ------------------- Users -------------------

        public async Task<int> RegisterUserAsync(User user)
        {
            var existing = await GetUserByEmailAsync(user.Email);

            if (existing != null)
                throw new Exception("Email already registered");

            return await db.InsertAsync(user);
        }

        public async Task<User?> LoginUserAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null)
                return null;

            if (user.PasswordHash == password)
                return user;

            return null;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await db.Table<User>()
                           .Where(u => u.Email == email)
                           .FirstOrDefaultAsync();
        }

        public async Task<int> AddUserAsync(User user)
        {
            return await db.InsertAsync(user);
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            return await db.UpdateAsync(user);
        }

        public async Task<int> DeleteUserAsync(User user)
        {
            return await db.DeleteAsync(user);
        }

        // ------------------- Pets -------------------

        public async Task<List<Pet>> GetUserPetsAsync(int userId)
        {
            return await db.Table<Pet>()
                           .Where(p => p.UserId == userId)
                           .ToListAsync();
        }

        public Task<List<Pet>> GetPetsByUserAsync(int userId)
        {
            return GetUserPetsAsync(userId);
        }

        public Task<List<Pet>> GetPetsAsync(int userId)
        {
            return db.Table<Pet>()
                     .Where(p => p.UserId == userId)
                     .ToListAsync();
        }

        public async Task<int> AddPetAsync(Pet pet)
        {
            return await db.InsertAsync(pet);
        }

        public async Task<int> UpdatePetAsync(Pet pet)
        {
            return await db.UpdateAsync(pet);
        }

        public async Task<int> DeletePetAsync(Pet pet)
        {
            return await db.DeleteAsync(pet);
        }

        // ------------------- Appointments -------------------

        public async Task<List<Appointment>> GetUserAppointmentsAsync(int userId)
        {
            var userPets = await GetPetsAsync(userId);
            var petIds = userPets.Select(p => p.Id).ToList();

            if (petIds.Count == 0)
                return new List<Appointment>();

            var allAppointments = await db.Table<Appointment>().ToListAsync();

            return allAppointments
                .Where(a => petIds.Contains(a.PetId))
                .OrderBy(a => a.Schedule)
                .ToList();
        }

        public async Task<int> AddAppointmentAsync(Appointment appointment)
        {
            return await db.InsertAsync(appointment);
        }

        public async Task<int> UpdateAppointmentAsync(Appointment appointment)
        {
            return await db.UpdateAsync(appointment);
        }

        public async Task<int> DeleteAppointmentAsync(Appointment appointment)
        {
            return await db.DeleteAsync(appointment);
        }
        public async Task<int> SavePasswordResetOtpAsync(PasswordResetOtp otp)
        {
            return await db.InsertAsync(otp);
        }

        public async Task<PasswordResetOtp?> GetValidPasswordResetOtpAsync(string email, string code)
        {
            return await db.Table<PasswordResetOtp>()
                           .Where(o =>
                               o.Email == email &&
                               o.Code == code &&
                               o.IsUsed == false &&
                               o.ExpiresAt > DateTime.UtcNow)
                           .OrderByDescending(o => o.Id)
                           .FirstOrDefaultAsync();
        }

        public async Task<int> MarkPasswordResetOtpUsedAsync(PasswordResetOtp otp)
        {
            otp.IsUsed = true;
            return await db.UpdateAsync(otp);
        }

        public async Task<int> UpdateUserPasswordAsync(string email, string newPassword)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null)
                return 0;

            user.PasswordHash = newPassword;

            return await db.UpdateAsync(user);
        }
    }
}