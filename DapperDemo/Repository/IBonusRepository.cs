using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public interface IBonusRepository
    {
        List<Employee> GetEmployeWithCompany(int id);
        Company GetCompanyWithEployees(int id);

        List<Company> GetAllCompanyWithEmployees();

        void AddTestCompanyWithEmployee(Company company);

        void RemoveRange(int[] companyId);

        List<Company> FilterAllCompanies(string name);
    }
}
