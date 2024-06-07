namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public ReportingStructure(string employee, int numberOfReports)
        {
            EmployeeId = employee;
            NumberOfReports = numberOfReports;
        }

        public string EmployeeId { get; set; }
        public int NumberOfReports { get; set; }
    }
}
