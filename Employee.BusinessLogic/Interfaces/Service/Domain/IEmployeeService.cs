using Employee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.BusinessLogic.Results;

namespace Employee.BusinessLogic.Interfaces.Service.Domain
{
    public interface IEmployeeService
    {
        public Task<PaginatedResult<EmployeeViewModel>> GetAllEmployees(int start, int length, string? name, string? phone, int departmentId);
        Task<Employees?> GetEmployeeById(Guid id);
        Task<bool> HireEmployee(Employees employee, string? createdBy);
        Task<ValueTuple<bool, int>> UpdateEmployee(Employees employee, string user);
        Task<ValueTuple<bool, int>> TerminateEmployee(Guid id);
    }
}
