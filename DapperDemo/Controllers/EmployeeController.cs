    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DapperDemo.Data;
using DapperDemo.Models;
using DapperDemo.Repository;

namespace DapperDemo.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IBonusRepository _bonusRepo;
        [BindProperty]
        public Employee Employee { get; set; }
        public EmployeeController(IEmployeeRepository employeeRepo, ICompanyRepository companyRepo, IBonusRepository bonusRepo)
        {
            _employeeRepo = employeeRepo;
            _companyRepo = companyRepo;
            _bonusRepo = bonusRepo;
        }

        // GET: Companies
        public async Task<IActionResult> Index(int companyId = 0)
        {
            //List<Employee> list = _employeeRepo.GetAll();
            //foreach(var item in list)
            //{   
            //    item.Company = _companyRepo.Find(item.CompanyId);
            //}   
            var list = _bonusRepo.GetEmployeWithCompany(companyId);
            return View(list);
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = _employeeRepo.Find(id);


            return View(employee);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> companyList =_companyRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList = companyList;
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost()
        {
            if (ModelState.IsValid)
            {
                _employeeRepo.Add(Employee);
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IEnumerable<SelectListItem> companyList = _companyRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList = companyList;
            Employee employee = _employeeRepo.Find(id);
            _employeeRepo.Update(employee);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPost(int id)
        {
            if (id != Employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _employeeRepo.Update(Employee);
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _employeeRepo.Remove(id);


            return RedirectToAction(nameof(Index));
        }

       

       
    }
}
