using Employee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.BusinessLogic.Interfaces.Service.Masters
{
    public interface IDepartmentMaster
    {
        Task<List<Department>> GetAll();
    }
}
