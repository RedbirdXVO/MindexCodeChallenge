using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee GetEmployeeWithDirectReportsById(String id);
        Compensation GetCompensationById(String id);
        Employee Add(Employee employee);
        Compensation AddCompensation(Compensation compensation);
        Employee Remove(Employee employee);
        Task SaveAsync();
    }
}