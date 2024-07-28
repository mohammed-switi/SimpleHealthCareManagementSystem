using HealthCare.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Repositories
{
    public interface IVisitRepository
    {
        Task<Visit> GetVisitById(int id);
        Task<IEnumerable<Visit>> GetAllVisits();
        Task AddVisit(Visit visit);
        Task UpdateVisit(Visit visit);
        Task DeleteVisit(int id);
    }

    public class VisitRepository : IVisitRepository
    {
        private readonly HealthcaredbContext _context;

        public VisitRepository(HealthcaredbContext context)
        {
            _context = context;
        }

        public async Task<Visit> GetVisitById(int id)
        {
            var result = await _context.Visits.FindAsync(id);
            return result ?? throw new Exception("Visit Not Found");
        }

        public async Task<IEnumerable<Visit>> GetAllVisits()
        {
            return await _context.Visits.ToListAsync();
        }

        public async Task AddVisit(Visit visit)
        {
            await _context.Visits.AddAsync(visit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVisit(Visit visit)
        {
            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVisit(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                throw new Exception("Visit Not Found");
            }

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
        }
    }
}
