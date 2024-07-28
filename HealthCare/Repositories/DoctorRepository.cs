using HealthCare.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Repositories
{
    public interface IDoctorRepository
    {
        Task<Doctor> GetDoctorById(int id);
        Task<IEnumerable<Doctor>> GetAllDoctors();
        Task AddDoctor(Doctor doctor);
        Task UpdateDoctor(Doctor doctor);
        Task DeleteDoctor(int id);
    }

    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthcaredbContext _context;

        public DoctorRepository(HealthcaredbContext context)
        {
            _context = context;
        }

        public async Task<Doctor> GetDoctorById(int id)
        {
            var result = await _context.Doctors.FindAsync(id);
            return result ?? throw new Exception("Doctor Not Found");
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task AddDoctor(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDoctor(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                throw new Exception("Doctor Not Found");
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
    }
}
