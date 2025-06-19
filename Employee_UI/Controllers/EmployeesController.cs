using Employee.BusinessLogic.Interfaces.Service.Domain;
using Employee.BusinessLogic.Interfaces.Service.Masters;
using Employee.DataAccess.Persistence.DbContexts;
using Employee.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace Employee.UI.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentMaster _departmentMaster;

        public EmployeesController(IEmployeeService employeeService, IDepartmentMaster departmentMaster)
        {
            _employeeService = employeeService;
            _departmentMaster = departmentMaster;
        }


        // GET: Employees
        public async Task<IActionResult> Index(int departmentId)
        {
            if(!ModelState.IsValid)
                return View("Index");
            
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.DepartmentId = new SelectList(await _departmentMaster.GetAll(), "DepartmentId", "DepartmentName", departmentId);

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LoadEmployees()
        {
            var (draw, start, length, name, phone, departmentId) = GetDataTableParameters();
          
            var paginatedResult = await _employeeService.GetAllEmployees(start, length, name, phone, departmentId);

            return Json(new
            {
                draw,
                recordsTotal = paginatedResult.TotalCount,
                recordsFiltered = paginatedResult.FilteredCount,
                data = paginatedResult.Items
            });
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var employees = await _employeeService.GetEmployeeById(id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.DepartmentId = new SelectList(await _departmentMaster.GetAll(), "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employees employees)
        {
            var user = HttpContext.Session.GetString("userName");

            try
             {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, validationErrors = 
                        ModelState.
                            ToDictionary(k => k.Key, v => v.Value?.Errors.Select(e => e.ErrorMessage).ToArray()) });
                }

                var employeeAdded = await _employeeService.HireEmployee(employees, user);
                if (!employeeAdded)
                    return BadRequest();

                return Json(
                    new { success = true, redirectUrl = Url.Action("Index", "Employees") }
                );
            }
            catch (Exception ex)
            {
                return Json(
                    new { success = false, error = ex.InnerException?.Message ?? ex.Message }
                );
            }
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var employees = await _employeeService.GetEmployeeById(id);
            if (employees == null)
            {
                return NotFound();
            }

            ViewBag.DepartmentId = new SelectList(await _departmentMaster.GetAll(), "DepartmentId", "DepartmentName", employees.DepartmentId);
            return View(employees);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // JSON.stringify
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employees employees)
        {
            var user = HttpContext.Session.GetString("userName");

            if (!ModelState.IsValid)
                return Json(new { success = false, redirectUrl = Url.Action("Index", "Employees") });
            try
            {
                var (editedEmployee, editFlag) = await _employeeService.UpdateEmployee(employees, user);

                if (editFlag == -1)
                    return NotFound();

                if(!editedEmployee)
                    return Json(new { success = false, message = "Error updating Employee"});
            }
            catch (DbUpdateConcurrencyException)
            {
                /*if (!EmployeesExists(employees.EmployeeId))
                {
                    return NotFound();
                }*/
            }

            return Json(new { success = false, redirectUrl = Url.Action("Index", "Employees") });
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var employees = await _employeeService.GetEmployeeById(id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var (deletedEmployee, deleteFlag) = await _employeeService.TerminateEmployee(id);
            if(deleteFlag == -1)
                 return Json(new { success = false, redirectUrl = Url.Action("Delete", "Employees") });

            return Json(new { redirectUrl = Url.Action("Index", "Employees") });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        

        /*private bool EmployeesExists(Guid id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }*/


        /*[HttpPost]
        public async Task<IActionResult> DownloadCsv()
        {
            // Fetch the list again, or use session/cache if already available
            var employees = await _context.Employees.Include(a => a.Department)
                .AsNoTracking()
                .ToListAsync(); 

            var builder = new StringBuilder();
            builder.AppendLine("EmployeeId,FirstName,LastName,Email,PhoneNumber,Department,Designation");

            foreach (var emp in employees)
            {
                builder.AppendLine($"{emp.EmployeeId},{emp.FirstName},{emp.LastName},{emp.Email},{emp.PhoneNumber},{emp.Department},{emp.Designation}");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(builder.ToString());
            return File(buffer, "text/csv", "employees.csv");
        }*/




        // Helper functions here

        private ValueTuple<string, int, int, string, string, int> GetDataTableParameters()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Convert.ToInt32(Request.Form["start"]);
            var length = Convert.ToInt32(Request.Form["length"]);
           

            var name = Request.Form["name"].FirstOrDefault();
            var phone = Request.Form["phone"].FirstOrDefault();
            var isValidDepartment = int.TryParse(Request.Form["DepartmentId"], out var departmentId);
            
            if (!isValidDepartment)
                departmentId = 0;
                
            return new ValueTuple<string, int, int, string, string, int>(draw, start, length, name, phone, departmentId);
        }
    }
}