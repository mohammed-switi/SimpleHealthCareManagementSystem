using HealthCare.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Repositories
{
    public interface ITestRepository
    {
        Task<Test> GetTestById(int id);
        Task<IEnumerable<Test>> GetAllTests();
        Task AddTest(Test test);
        Task UpdateTest(Test test);
        Task DeleteTest(int id);
    }

    public class TestRepository : ITestRepository
    {
        private readonly HealthcaredbContext _context;

        public TestRepository(HealthcaredbContext context)
        {
            _context = context;
        }

        public async Task<Test> GetTestById(int id)
        {
            var result = await _context.Tests.FindAsync(id);
            return result ?? throw new Exception("Test Not Found");
        }

        public async Task<IEnumerable<Test>> GetAllTests()
        {
            return await _context.Tests.ToListAsync();
        }

        public async Task AddTest(Test test)
        {
            await _context.Tests.AddAsync(test);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTest(Test test)
        {
            _context.Tests.Update(test);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null)
            {
                throw new Exception("Test Not Found");
            }

            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
        }
    }
}
