namespace CarlosYulo.backend;

public class Employee
{
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public int EmployeeTypeId { get; set; }
    public double Salary { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public DateTime Birthday { get; set; }
    public string ProfilePicture { get; set; }
    
    
    public Employee(){}
    
    public Employee(int employeeId, string employeeFullName, int employeeTypeId, double salary, string email, string phoneNumber, string gender, int age, DateTime birthday, string profilePicture)
    {
        EmployeeId = employeeId;
        EmployeeFullName = employeeFullName;
        EmployeeTypeId = employeeTypeId;
        Salary = salary;
        Email = email;
        PhoneNumber = phoneNumber;
        Gender = gender;
        Age = age;
        Birthday = birthday;
        ProfilePicture = profilePicture;
    }
}