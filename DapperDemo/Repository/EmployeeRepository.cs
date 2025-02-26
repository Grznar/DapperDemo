﻿using Dapper;
using DapperDemo.Data;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DapperDemo.Repository
{
    
    public class EmployeeRepository : IEmployeeRepository
    {
        
        private readonly IDbConnection _db;
      public EmployeeRepository(IConfiguration configuration)
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Employee Add(Employee employee)
        {
            var sql = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId)"
            +"SELECT CAST(SCOPE_IDENTITY() as int); ";
            var id = _db.Query<int>(sql,employee).Single();
            employee.CompanyId = id;
            return employee;


        }

        public Employee Find(int id)
        {
            var sql = "SELECT * FROM Employees WHERE EmployeeId = @Id";
            return _db.Query<Employee>(sql, new { @Id = id }).Single();   
           

        }

        public List<Employee> GetAll()
        {
            var sql = "SELECT * FROM Employees";
            var result = _db.Query<Employee>(sql).ToList();
            return result;
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Employees WHERE EmployeeId = @Id";
            _db.Execute(sql, new { id });

        }

        public Employee Update(Employee employee)
        {

            var sql = "UPDATE Employees SET Name = @Name, Title = @Title, Email = @Email, Phone = @Phone, CompanyId = @CompanyId WHERE EmployeeId = @EmployeeId";
            _db.Execute(sql,employee);
            
            return employee;

        }
    }
}
