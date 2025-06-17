using Employee.BusinessLogic;
using Employee.BusinessLogic.Interfaces.IRepository;
using Employee.DataAccess.Persistence.DbContexts;
using Employee.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Persistence.Repository.Domain
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEmployeeAsync(Employees employee, string createdBy)
        {
            employee.EmployeeId = Guid.NewGuid();
            employee.CreatedAt = DateTime.Now;
            employee.UpdatedAt = DateTime.Now;
            employee.CreatedBy = createdBy;
            employee.ModifiedBy = createdBy;
            await _context.Employees.AddAsync(employee);
            var result = await _context.SaveChangesAsync();
            return result > 0; // Returns true if at least one record was added
        }

        public bool CheckEmployeeExistsAsync(Guid id)
        {
            var exists = _context.Employees.Any(e => e.EmployeeId == id);
            return exists;
        }

        public async Task<(bool, int)> DeleteEmployeeAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Remove(employee!);
            var result = await _context.SaveChangesAsync();
            return (result > 0, result); // Returns true if at least one record was deleted
        }

        public async Task<ValueTuple<List<EmployeeViewModel>, int>> GetAllEmployeesAsync(int start, int length, string? name, string? phone, int departmentId)
        {
            var query = _context.Employees
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(e => e.FirstName.Contains(name));

            if (!string.IsNullOrEmpty(phone))
                query = query.Where(e => e.PhoneNumber == phone);

            if (departmentId != 0)
                query = query.Where(e => e.DepartmentId == departmentId);


            var filterRecords = await query.CountAsync();

            var data = await query
                .OrderBy(e => e.FirstName)
                .ThenBy(a => a.LastName)
                .Skip(start)
                .Take(length)
                .Select(e => new EmployeeViewModel()
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Department = e.Department.DepartmentName,
                    Designation = e.Designation,
                    IsActive = e.IsActive
                })
                .ToListAsync();

            return (data, filterRecords);
        }

        public Task<Employees?> GetEmployeeByIdAsync(Guid id)
        {
            var employee = _context.Employees
                .Where(e => e.EmployeeId == id)
                .Include(a => a.Department)
                .FirstOrDefaultAsync();
            return employee;
        }

        public async Task<(bool, int)> UpdateEmployeeAsync(Employees employee, string? user)
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.EmployeeId);
            UpdateEmployeeProperties(existingEmployee, employee, user);
            var result = await _context.SaveChangesAsync();
            return (result > 0, result); // Returns true if at least one record was updated
        }


        private void UpdateEmployeeProperties(Employees? existingEmployee, Employees employee, string? user)
        {

            employee.ModifiedBy = user;

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.NationalIdNumber = employee.NationalIdNumber;
            existingEmployee.Address = employee.Address;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.Gender = employee.Gender;
            existingEmployee.DepartmentId = employee.DepartmentId;
            existingEmployee.Designation = employee.Designation;
            existingEmployee.DateOfJoining = employee.DateOfJoining;
            existingEmployee.UpdatedAt = DateTime.Now;
            existingEmployee.ModifiedBy = employee.ModifiedBy;
            existingEmployee.BloodGroup = employee.BloodGroup;
            existingEmployee.ModifiedBy = employee.ModifiedBy;
            existingEmployee.MaritalStatus = employee.MaritalStatus;
            existingEmployee.IsActive = employee.IsActive;
        }
    }
}
