using Employee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.BusinessLogic.Interfaces.IRepository
{
    public interface IEmployeeRepository
    {
        public Task<ValueTuple<List<EmployeeViewModel>, int>> GetAllEmployeesAsync(int start, int length, string? name, string? phone, int departmentId);
        Task<Employees?> GetEmployeeByIdAsync(Guid id);
        Task<bool> AddEmployeeAsync(Employees employee, string createdBy);
        Task<ValueTuple<bool, int>> UpdateEmployeeAsync(Employees employee, string user);
        Task<ValueTuple<bool, int>> DeleteEmployeeAsync(Guid id);
        bool CheckEmployeeExistsAsync(Guid id);
    }
}
