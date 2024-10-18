namespace CarlosYulo.backend.entities;

public class EmployeeSalary
{
    public int? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? EmployeeType { get; set; }
    public double? Salary { get; set; }
    public DateTime? PostedSalaryDate { get; set; }
    public bool IsPayed { get; set; }
    
    public override string ToString()
    {
        return $"Employee ID: {EmployeeId ?? -1}\n" + 
               $"Employee Name: {EmployeeName ?? "N/A"}\n" + 
               $"Employee Type: {EmployeeType ?? "N/A"}\n" + 
               $"Salary: {(Salary.HasValue ? Salary.Value.ToString("C2") : "N/A")}\n" + 
               $"Posted Salary Date: {(PostedSalaryDate.HasValue ? PostedSalaryDate.Value.ToShortDateString() : "N/A")}\n" + 
               $"Is Paid: {IsPayed}";
    }

}