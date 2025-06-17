using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.BusinessLogic.Interfaces.Service.Masters
{
    public interface IQueryService
    {
        Task<int> GetCount();
        public Task<int> GetFilteredCount(string? name, string? phone, int departmentId);

    }
}
