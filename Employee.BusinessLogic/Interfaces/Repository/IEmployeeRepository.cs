using Employee.Domain.Models;

namespace Employee.BusinessLogic.Interfaces.Repository
{
    public interface IEmployeeRepository
    {
        public Task<(List<EmployeeViewModel>, int)> GetAllEmployeesAsync(int start, int length, string? name, string? phone, int departmentId);
        Task<Employees?> GetEmployeeByIdAsync(Guid id);
        Task<bool> AddEmployeeAsync(Employees employee, string createdBy);
        Task<ValueTuple<bool, int>> UpdateEmployeeAsync(Employees employee, string user);
        Task<ValueTuple<bool, int>> DeleteEmployeeAsync(Guid id);
        bool CheckEmployeeExistsAsync(Guid id);
    }
}
