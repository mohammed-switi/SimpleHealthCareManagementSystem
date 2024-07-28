using HealthCare.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HealthCare.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department> GetDepartmentById(int id);
        Task<IEnumerable<Department>> GetAllDepartments();
        Task AddDepartment(Department department);
        Task UpdateDepartment(Department department);
        Task DeleteDepartment(int id);
    }

    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HealthcaredbContext _context;

        public DepartmentRepository(HealthcaredbContext context)
        {
            _context = context;
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            var result = await _context.Departments.FindAsync(id);
            return result ?? throw new Exception("Department Not Found");
        }

        public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task AddDepartment(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepartment(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                throw new Exception("Department Not Found");
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
