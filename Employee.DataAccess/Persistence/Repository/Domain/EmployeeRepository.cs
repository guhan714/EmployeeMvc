using Employee.BusinessLogic;
using Employee.DataAccess.Persistence.DbContexts;
using Employee.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Employee.BusinessLogic.Interfaces.Repository;
using ZLinq;

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
                query = query.Where(e => EF.Functions.Like(e.FirstName, $"%{name}%"));

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
                    LastName = e.LastName ?? string.Empty,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber!,
                    Department = e.Department.DepartmentName,
                    Designation = e.Designation!,
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
            employee.ModifiedBy = user;
            
            if(existingEmployee?.FirstName != employee.FirstName)
                existingEmployee.FirstName = employee.FirstName;
            
            if(existingEmployee.LastName != employee.LastName)
                existingEmployee.LastName = employee.LastName;
            
            if(existingEmployee.Email != employee.Email)
                existingEmployee.Email = employee.Email;
            
            if(existingEmployee.PhoneNumber != employee.PhoneNumber)
                existingEmployee.PhoneNumber = employee.PhoneNumber;
            
            if(existingEmployee.NationalIdNumber != employee.NationalIdNumber)
                existingEmployee.NationalIdNumber = employee.NationalIdNumber;
            
            if(existingEmployee.Address != employee.Address)
                existingEmployee.Address = employee.Address;
            
            if(existingEmployee.DateOfBirth != employee.DateOfBirth)
                existingEmployee.DateOfBirth = employee.DateOfBirth;
            
            if(existingEmployee.Gender != employee.Gender)
                existingEmployee.Gender = employee.Gender;
            
            if(existingEmployee.DepartmentId != employee.DepartmentId)
                existingEmployee.DepartmentId = employee.DepartmentId;
            
            if(existingEmployee.Designation != employee.Designation)
                existingEmployee.Designation = employee.Designation;
            
            if(existingEmployee.DateOfJoining != employee.DateOfJoining)
                existingEmployee.DateOfJoining = employee.DateOfJoining;
            
            if(existingEmployee.UpdatedAt != employee.UpdatedAt)
                existingEmployee.UpdatedAt = DateTime.Now;
            
            if(existingEmployee.BloodGroup != employee.BloodGroup)
                existingEmployee.BloodGroup = employee.BloodGroup;
            
            existingEmployee.ModifiedBy = employee.ModifiedBy;
            
            if(existingEmployee.MaritalStatus != employee.MaritalStatus)
                existingEmployee.MaritalStatus = employee.MaritalStatus;
            
            if(existingEmployee.Salary != employee.Salary)
                existingEmployee.Salary = employee.Salary;
            
            if(existingEmployee.IsActive != employee.IsActive)
                existingEmployee.IsActive = employee.IsActive;
        }
    }
}
