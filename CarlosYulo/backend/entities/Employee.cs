using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend;

public class Employee
{
    public int? EmployeeId { get; set; }
    public string? EmployeeFullName { get; set; }
    public int? EmployeeTypeId { get; set; }
    public double? Salary { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public int? Age { get; set; }
    public DateTime? Birthday { get; set; }
    public byte[] ProfilePicture { get; set; }

    private ImageViewer _imageViewer;

    public Employee()
    {
        _imageViewer = new ImageViewer();
    }

    public Employee(int employeeId, string employeeFullName, int employeeTypeId, double salary, string email,
        string phoneNumber, string gender, int age, DateTime birthday, byte[] profilePicture)
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
        _imageViewer = new ImageViewer();
    }


    public void SetProfilePictureToByte(string profilePicturePath)
    {
        try
        {
            byte[] formattedProfilePicture = _imageViewer.LoadProfilePicture(profilePicturePath);

            if (_imageViewer.IsValidImageFormat(formattedProfilePicture))
            {
                ProfilePicture = formattedProfilePicture;
            }
            else
            {
                throw new InvalidDataException("Invalid image format. Only PNG and JPEG are supported.");
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (InvalidDataException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}

public enum EmployeeRole
{
    STAFF,
    SUPERVISOR,
    TRAINER,
    MANAGER
}