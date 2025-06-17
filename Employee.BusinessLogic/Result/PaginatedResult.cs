using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.BusinessLogic.Result
{
    public class PaginatedResult<T>
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<T> Items { get; set; }
    }

}
