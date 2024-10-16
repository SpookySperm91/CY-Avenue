using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend;

public class Employee
{
    public string? FullName { get; set; }
    public int? EmployeeTypeId { get; set; }
    public double? Salary { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public int? Age { get; set; }
    public DateTime? BirthDate { get; set; }


    // Non mutable on surface level
    public int? EmployeeId { get; set; }
    public string EmployeeType { get; private set; }
    public byte[]? ProfilePictureByte { get; set; }
    public Image? ProfilePictureImage { get; private set; }

    private ImageViewer _imageViewer;

    public Employee()
    {
        _imageViewer = new ImageViewer();
    }

    public override string ToString()
    {
        return $"Employee ID: {EmployeeId}, " +
               $"Full Name: {FullName}, " +
               $"Employee Type ID: {EmployeeTypeId}, " +
               $"Employee Type: {EmployeeType}, " +
               $"Salary: {Salary}, " + // Formats as currency
               $"Email: {Email}, " +
               $"Phone Number: {PhoneNumber}, " +
               $"Gender: {Gender}, " +
               $"Age: {Age}, " +
               $"Birth Date: {BirthDate?.ToString("MMMM dd, yyyy") ?? "N/A"}, " +
               $"Profile Picture Byte: {(ProfilePictureByte != null ? $"{ProfilePictureByte.Length} bytes" : "N/A")}, " +
               $"Profile Picture: {(ProfilePictureImage != null ? "Image Set" : "N/A")}";
    }


    public bool SetProfilePicture(string profilePicturePath, out string message)
    {
        try
        {
            byte[] formattedProfilePicture = _imageViewer.LoadProfilePicture(profilePicturePath);

            if (_imageViewer.IsValidImageFormat(formattedProfilePicture))
            {
                ProfilePictureImage = _imageViewer.ConvertByteArrayToImage(formattedProfilePicture); // Image 
                ProfilePictureByte = formattedProfilePicture; // byte[] 
                message = "Profile Picture Set";
                return true;
            }

            message = "Profile picture could not be loaded. Invalid image format. Only PNG and JPEG are supported.";
            return false;
        }
        catch (FileNotFoundException ex)
        {
            message = ex.Message;
            return false;
        }
        catch (InvalidDataException ex)
        {
            message = ex.Message;
            return false;
        }
        catch (Exception ex)
        {
            message = ex.Message;
            return false;
        }
    }

    public void SetProfilePicture(string profilePicturePath)
    {
        try
        {
            byte[] formattedProfilePicture = _imageViewer.LoadProfilePicture(profilePicturePath);

            if (_imageViewer.IsValidImageFormat(formattedProfilePicture))
            {
                ProfilePictureImage = _imageViewer.ConvertByteArrayToImage(formattedProfilePicture); // image 
                ProfilePictureByte = formattedProfilePicture;
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

    public void SetProfilePictureImage(Image? profilePictureImage)
    {
        ProfilePictureImage = profilePictureImage;
    }

    public void SetEmployeeType(string employeeType)
    {
        EmployeeType = employeeType;
    }
}



