using System.Diagnostics;
using DapperDemo.Models;
using DapperDemo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DapperDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBonusRepository _bonusRepo;
        public HomeController(ILogger<HomeController> logger,IBonusRepository bonusRepo)
        {
            _bonusRepo = bonusRepo;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Company> list = _bonusRepo.GetAllCompanyWithEmployees();
            return View(list);
        }

        public IActionResult AddTestRecords()
        {
            Company company = new Company()
            {
                Name = "Test" + Guid.NewGuid().ToString(),
                Address = "test address",
                City = "test city",
                PostalCode = "test postalCode",
                State = "test state",
                Employees = new List<Employee>()
            };

            company.Employees.Add(new Employee()
            {
                Email = "test Email",
                Name = "Test Name " + Guid.NewGuid().ToString(),
                Phone = " test phone",
                Title = "Test Manager"
            });

            company.Employees.Add(new Employee()
            {
                Email = "test Email 2",
                Name = "Test Name 2" + Guid.NewGuid().ToString(),
                Phone = " test phone 2",
                Title = "Test Manager 2"
            });
            _bonusRepo.AddTestCompanyWithEmployee(company);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveTestRecords()
        {
            int[] comapny = _bonusRepo.FilterAllCompanies("Test").Select(u=>u.CompanyId).ToArray();

            _bonusRepo.RemoveRange(comapny);

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
