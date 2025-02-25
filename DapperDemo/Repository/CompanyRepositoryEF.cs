using DapperDemo.Data;
using DapperDemo.Models;

namespace DapperDemo.Repository
{
    
    public class CompanyRepositoryEF : ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

      public CompanyRepositoryEF(ApplicationDbContext context)
        {
            _db = context;
        }

        public Company Add(Company company)
        {
            _db.Companies.Add(company);
            _db.SaveChanges();
            return company;
        }

        public Company Find(int id)
        {
               Company company = _db.Companies.FirstOrDefault(u=>u.CompanyId==id);
            return company;

        }

        public List<Company> GetAll()
        {
            List<Company> list = new List<Company>();
            list = _db.Companies.ToList();
            return list;
        }

        public void Remove(int id)
        {
            Company company = _db.Companies.FirstOrDefault(u => u.CompanyId == id);
            _db.Companies.Remove(company);
            _db.SaveChanges();
        }

        public Company Update(Company company)
        {
            
            _db.Companies.Update(company);
            _db.SaveChanges();
            return company;
        }
    }
}
