using Employee.BusinessLogic.Interfaces.Service.Masters;
using Employee.DataAccess.Persistence.DbContexts;
using Employee.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Persistence.Repository.Masters
{
    public class DepartmentMaster : IDepartmentMaster
    {
        private readonly AppDbContext _context;

        public DepartmentMaster(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAll()
        {
            var departments = await _context.Departments.Select(d => new Department
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            }).ToListAsync();
            return departments;
        }
    }
}
