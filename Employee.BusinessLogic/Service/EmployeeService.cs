using Employee.BusinessLogic.Interfaces.Repository;
using Employee.BusinessLogic.Interfaces.Service.Domain;
using Employee.BusinessLogic.Interfaces.Service.Masters;
using Employee.BusinessLogic.Results;
using Employee.Domain.Models;


namespace Employee.BusinessLogic.Service
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IQueryService _employeeQueryService;
        public EmployeeService(IEmployeeRepository employeeRepository, IQueryService queryService)
        {
            _employeeRepository = employeeRepository;
            _employeeQueryService = queryService;
        }

        public async Task<bool> HireEmployee(Employees employee, string? createdBy)
        {
            var employeeAdded  = await _employeeRepository.AddEmployeeAsync(employee, createdBy);
            return employeeAdded;
        }

        public async Task<ValueTuple<bool, int>> TerminateEmployee(Guid id)
        {
            var exists = _employeeRepository.CheckEmployeeExistsAsync(id);
            if (!exists)
            {
                return (false, -1); // Employee does not exist
            }

            var deleteResult = await _employeeRepository.DeleteEmployeeAsync(id);
            return deleteResult;
        }

        public async Task<PaginatedResult<EmployeeViewModel>> GetAllEmployees(int start, int length, string? name, string? phone, int departmentId)
        {
            var (data, recordsFiltered) = await _employeeRepository.GetAllEmployeesAsync(start, length, name, phone, departmentId);
            var recordsTotal = await _employeeQueryService.GetCount();

            if (data == null || data.Count == 0)
            {
                return new PaginatedResult<EmployeeViewModel>();
            }

            return new PaginatedResult<EmployeeViewModel>
            {
                TotalCount = recordsTotal,
                FilteredCount = recordsFiltered,
                Items = data
            };
        }

        public async Task<Employees?> GetEmployeeById(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            return employee;
        }

        public async Task<ValueTuple<bool, int>> UpdateEmployee(Employees employee, string? user)
        {
            var exists = _employeeRepository.CheckEmployeeExistsAsync(employee.EmployeeId);
            if (!exists)
            {
                return (false, -1); 
            }

            var updatedResult = await _employeeRepository.UpdateEmployeeAsync(employee, user);
            return updatedResult;
        }
    }
}
