using System.ComponentModel;
using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend;

public class Client
{
    public int? MembershipId { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public Image? ProfilePictureImage { get; private set; }
    public string? FullName { get; set; }
    public int? MembershipTypeId { get; set; }
    public string? Membership { get; private set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public int? Age { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? MembershipStart { get; set; }
    public DateTime? MembershipEnd { get; set; }
    public string? MembershipStatus { get; set; }

    private ImageViewer _imageViewer;

    public Client()
    {
        _imageViewer = new ImageViewer();
    }

    public Client(int membershipId, byte[] profilePicture, string fullName, int membershipTypeId,
        string email, string phoneNumber, string gender, int age, DateTime birthDate,
        DateTime membershipStart, DateTime membershipEnd, string membershipStatus)
    {
        MembershipId = membershipId;
        ProfilePicture = profilePicture;
        FullName = fullName;
        MembershipTypeId = membershipTypeId;
        Email = email;
        PhoneNumber = phoneNumber;
        Gender = gender;
        Age = age;
        BirthDate = birthDate;
        MembershipStart = membershipStart;
        MembershipEnd = membershipEnd;
        MembershipStatus = membershipStatus;
        _imageViewer = new ImageViewer();
    }

    public override string ToString()
    {
        return $"Full Name: {FullName}, " +
               $"Membership ID: {MembershipId}, " +
               $"Membership Type ID: {MembershipTypeId}, " +
               $"Membership: {Membership}, " +
               $"Email: {Email}, " +
               $"Phone Number: {PhoneNumber}, " +
               $"Birth Date: {BirthDate?.ToString("MMMM dd, yyyy") ?? "N/A"}, " +
               $"Membership Start: {MembershipStart?.ToString("MMMM dd, yyyy") ?? "N/A"}, " +
               $"Membership End: {MembershipEnd?.ToString("MMMM dd, yyyy") ?? "N/A"}, " +
               $"Membership Status: {MembershipStatus}, " +
               $"Age: {Age}, " +
               $"Gender: {Gender}, " +
               $"Profile Picture: {(ProfilePicture != null ? $"{ProfilePicture.Length} bytes" : "N/A")}";
    }

    // Set string picture path into byte and save to ProfilePicture
    public void SetProfilePicture(string profilePicturePath)
    {
        try
        {
            byte[] formattedProfilePicture = _imageViewer.LoadProfilePicture(profilePicturePath);

            if (_imageViewer.IsValidImageFormat(formattedProfilePicture))
            {
                ProfilePictureImage = _imageViewer.ConvertByteArrayToImage(formattedProfilePicture);
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

    public void SetMembership(string membershipType)
    {
        Membership = membershipType;
        
    }
}

public enum MembershipType
{
    [Description("Accessibility")] ACCESSIBILITY,
    [Description("Premium")] PREMIUM,
    [Description("Walk-in")] WALK_IN,
    [Description("Walk-in(Treadmill)")] WALK_IN_TREADMILL
}

public enum MembershipStatus
{
    [Description("Inactive")] INACTIVE,
    [Description("Active")] ACTIVE
}