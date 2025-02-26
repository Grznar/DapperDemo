    using Dapper;
    using Dapper.Contrib.Extensions;
    using DapperDemo.Models;
    using Microsoft.Data.SqlClient;
    using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Data;

    namespace DapperDemo.Repository
    {
        public class BonusRepository : IBonusRepository
        {
            private readonly IDbConnection _db;
            public BonusRepository(IConfiguration configuration)
            {
                this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            }

        public void AddTestCompanyWithEmployee(Company company)
        {
            var sql = "INSERT INTO Companies(Name,Address,City,State,PostalCode) VALUES(@Name,@Address,@City,@State,@PostalCode)"
           + "SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = _db.Query<int>(sql, company).Single();
            company.CompanyId = id;

            // foreach(var employee in company.Employees)
            // {
            //     employee.CompanyId=company.CompanyId;
            //     var sql1 = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId)"
            //+ "SELECT CAST(SCOPE_IDENTITY() as int); ";
            //     _db.Query<int>(sql1, employee).Single();

            // }
            company.Employees.Select(u =>
            {
                u.CompanyId = id; ;
                return u;
            }).ToList();
            var sqlmn = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId)"
            + "SELECT CAST(SCOPE_IDENTITY() as int); ";
            _db.Execute(sqlmn, company.Employees);
        }



        public List<Company> GetAllCompanyWithEmployees()
            {
                var sql = "SELECT C.*, E.* FROM Companies AS C INNER JOIN Employees AS E ON E.CompanyId = C.CompanyId";

            var companyDic = new Dictionary<int, Company>();
                var company = _db.Query<Company, Employee, Company>(sql, (comp, empl) => { 

                    if(!companyDic.TryGetValue(comp.CompanyId, out Company company))
                    {
                        company = comp;
                    
                        companyDic.Add(company.CompanyId, company);
                    }
                    company.Employees.Add(empl);
                    return company;
                },splitOn:"EmployeeId");
                return company.Distinct().ToList();
            }

            public Company GetCompanyWithEployees(int id)
            {
                var p = new
                {
                    CompanyId = id
                };
                var sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId "
                    + "SELECT * FROM Employees WHERE CompanyId = @CompanyId";

                Company company;
                using(var lists = _db.QueryMultiple(sql, p))
                {
                    company = lists.Read<Company>().ToList().FirstOrDefault();
                    company.Employees = lists.Read<Employee>().ToList();
                }
                return company;
            }

            public List<Employee> GetEmployeWithCompany(int id)
            {
                var sql = "SELECT E.*,C.* FROM Employees AS E INNER JOIN Companies AS C ON E.CompanyId=C.CompanyId";
                if(id!=0)
                {
                    sql+= " WHERE E.CompanyId=@Id";
                }
                var employee= _db.Query<Employee,Company,Employee>(sql,(empl,comp)=>
                {
                    empl.Company=comp;
                    return empl;
                },new {id}, splitOn: "CompanyId").ToList();
                return employee;
            }

        public void RemoveRange(int[] companyId)
        {
           _db.Query("DELETE FROM Companies WHERE CompanyId IN @companyId", new { companyId });
        }
        public List<Company> FilterAllCompanies(string name)
        {
           var company =  _db.Query<Company>("SELECT * FROM Companies WHERE name LIKE '%' + @name + '%'", new {name});
            return company.ToList();
        }
    }
    }
