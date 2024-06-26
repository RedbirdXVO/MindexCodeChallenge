﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Compensation CreateCompensation(Compensation compensation)
        {
            if(compensation != null)
            {
                _employeeRepository.AddCompensation(compensation);
                _employeeRepository.SaveAsync().Wait();
            }

            return compensation;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public Compensation GetCompensationById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetCompensationById(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        public ReportingStructure GetReportingStructureById(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var employee = _employeeRepository.GetEmployeeWithDirectReportsById(id);
                if (employee != null)
                {
                    return new ReportingStructure(id, GetNumberOfReports(employee));
                }
            }

            return null;
        }

        private int GetNumberOfReports(Employee employee) 
        { 
            if(employee.DirectReports == null)
            {
                return 0;
            }

            int directReportsCount = employee.DirectReports.Count();
            foreach (Employee directReport in employee.DirectReports)
            {
                directReportsCount += GetNumberOfReports(_employeeRepository.GetEmployeeWithDirectReportsById(directReport.EmployeeId));
            }

            return directReportsCount;
        }
    }
}
