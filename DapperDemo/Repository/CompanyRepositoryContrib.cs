using Dapper;
using Dapper.Contrib.Extensions;
using DapperDemo.Data;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DapperDemo.Repository
{
    
    public class CompanyRepositoryContrib : ICompanyRepository
    {
        
        private readonly IDbConnection _db;
      public CompanyRepositoryContrib(IConfiguration configuration)
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Add(Company company)
        {
           var id = _db.Insert<Company>(company);
            company.CompanyId = (int)id;
            return company;
        }

        public Company Find(int id)
        {
            
            return _db.Get<Company>(id);


        }

        public List<Company> GetAll()
        {
            
            
            return _db.GetAll<Company>().ToList();
        }

        public void Remove(int id)
        {
            var toDelete = _db.Get<Company>(id);
            _db.Delete<Company>(toDelete);

        }

        public Company Update(Company company)
        {

            _db.Update<Company>(company);
            
            return company;

        }
    }
}
