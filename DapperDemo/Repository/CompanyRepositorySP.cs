using Dapper;
using DapperDemo.Data;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DapperDemo.Repository
{
    
    public class CompanyRepositorySP : ICompanyRepository
    {
        
        private readonly IDbConnection _db;
      public CompanyRepositorySP(IConfiguration configuration)
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Add(Company company)
        {
            var paramteres = new DynamicParameters();
            paramteres.Add("@CompanyId", 0, DbType.Int32, direction: ParameterDirection.Output);
            paramteres.Add("@Name", company.Name);
            paramteres.Add("@Address", company.Address);
            paramteres.Add("@City", company.City);
            paramteres.Add("@State", company.State);
            paramteres.Add("@PostalCode", company.PostalCode);
            this._db.Execute("usp_AddCompany",paramteres, commandType: CommandType.StoredProcedure);

            company.CompanyId=paramteres.Get<int>("@CompanyId");

            return company;
        }

        public Company Find(int id)
        {
            
            return _db.Query<Company>("usp_GetCompany",new { CompanyId=id}, commandType: CommandType.StoredProcedure).SingleOrDefault();   
           

        }

        public List<Company> GetAll()
        {
            
            var result = _db.Query<Company>("usp_GetALLCompany",commandType:CommandType.StoredProcedure).ToList();
            return result;
        }

        public void Remove(int id)
        {
            this._db.Execute("usp_RemoveCompany", new { CompanyId=id }, commandType: CommandType.StoredProcedure);

        }

        public Company Update(Company company)
        {

            var paramteres = new DynamicParameters();
            paramteres.Add("@CompanyId", company.CompanyId, DbType.Int32);
            paramteres.Add("@Name", company.Name);
            paramteres.Add("@Address", company.Address);
            paramteres.Add("@City", company.City);
            paramteres.Add("@State", company.State);
            paramteres.Add("@PostalCode", company.PostalCode);
            this._db.Execute("usp_UpdateCompany", paramteres, commandType: CommandType.StoredProcedure);

            company.CompanyId = paramteres.Get<int>("@CompanyId");

            return company;

        }
    }
}
