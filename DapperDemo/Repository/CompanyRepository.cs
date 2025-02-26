﻿using Dapper;
using DapperDemo.Data;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DapperDemo.Repository
{
    
    public class CompanyRepository : ICompanyRepository
    {
        
        private readonly IDbConnection _db;
      public CompanyRepository(IConfiguration configuration)
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Add(Company company)
        {
            var sql = "INSERT INTO Companies(Name,Address,City,State,PostalCode) VALUES(@Name,@Address,@City,@State,@PostalCode)"
            + "SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = _db.Query<int>(sql,company).Single();
            company.CompanyId = id;
            return company;


        }

        public Company Find(int id)
        {
            var sql = "SELECT * FROM Companies WHERE CompanyId = @Id";
            return _db.Query<Company>(sql, new { @Id = id }).Single();   
           

        }

        public List<Company> GetAll()
        {
            var sql = "SELECT * FROM Companies";
            var result = _db.Query<Company>(sql).ToList();
            return result;
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            _db.Execute(sql, new { id });

        }

        public Company Update(Company company)
        {

            var sql = "UPDATE Companies SET Name = @Name, Address = @Address, City = @City, State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";
            _db.Execute(sql,company);
            
            return company;

        }
    }
}
