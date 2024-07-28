
    using global::HealthCare.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace HealthCare.Repositories
    {
        public interface IAppUserRepository
        {
            Task<AppUser> GetAppUserById(int id);
            Task<IEnumerable<AppUser>> GetAllAppUsers();
            Task AddAppUser(AppUser appUser);
            Task UpdateAppUser(AppUser appUser);
            Task DeleteAppUser(int id);
        }

        public class AppUserRepository : IAppUserRepository
        {
            private readonly HealthcaredbContext _context;

            public AppUserRepository(HealthcaredbContext context)
            {
                _context = context;
            }

            public async Task<AppUser> GetAppUserById(int id)
            {
                var result = await _context.AppUsers.FindAsync(id);
                return result ?? throw new Exception("AppUser Not Found");
            }

            public async Task<IEnumerable<AppUser>> GetAllAppUsers()
            {
                return await _context.AppUsers.ToListAsync();
            }

            public async Task AddAppUser(AppUser appUser)
            {
                await _context.AppUsers.AddAsync(appUser);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAppUser(AppUser appUser)
            {
                _context.AppUsers.Update(appUser);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAppUser(int id)
            {
                var appUser = await _context.AppUsers.FindAsync(id);
                if (appUser == null)
                {
                    throw new Exception("AppUser Not Found");
                }

                _context.AppUsers.Remove(appUser);
                await _context.SaveChangesAsync();
            }
        }
    }



