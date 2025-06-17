using Employee.BusinessLogic.Interfaces.Service.Masters;
using Employee.DataAccess.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Persistence.Repository.Masters
{
    public class EmployeeQueryService : IQueryService
    {
        private readonly AppDbContext _context;

        public EmployeeQueryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCount()
        {
            var count = await _context.Employees.CountAsync();
            return count;
        }

        public async Task<int> GetFilteredCount(string? name, string? phone, int departmentId)
        {
            var query = _context.Employees.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(e => e.FirstName.Contains(name));

            if (!string.IsNullOrEmpty(phone))
                query = query.Where(e => e.PhoneNumber == phone);

            if (departmentId != 0)
                query = query.Where(e => e.DepartmentId == departmentId);

            return await query.CountAsync();
        }

    }
}
