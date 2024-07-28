using HealthCare.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> GetPatientById(int id);
        Task<IEnumerable<Patient>> GetAllPatients();
        Task AddPatient(Patient patient);
        Task UpdatePatient(Patient patient);
        Task DeletePatient(int id);
    }

    public class PatientRepository : IPatientRepository
    {
        private readonly HealthcaredbContext _context;

        public PatientRepository(HealthcaredbContext context)
        {
            _context = context;
        }

        public async Task<Patient> GetPatientById(int id)
        {
            var result = await _context.Patients.FindAsync(id);
            return result ?? throw new Exception("Patient Not Found");
        }

        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task AddPatient(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatient(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                throw new Exception("Patient Not Found");
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
}
