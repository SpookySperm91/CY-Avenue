using System.ComponentModel;
using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend;

public class Client
{
    public int? MembershipId { get; set; }
    public byte[]? ProfilePictureByte { get; set; }
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

    public Client(int membershipId, byte[] profilePictureByte, string fullName, int membershipTypeId,
        string email, string phoneNumber, string gender, int age, DateTime birthDate,
        DateTime membershipStart, DateTime membershipEnd, string membershipStatus)
    {
        MembershipId = membershipId;
        ProfilePictureByte = profilePictureByte;
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
               $"Profile Picture Byte: {(ProfilePictureByte != null ? $"{ProfilePictureByte.Length} bytes" : "N/A")}, " +
               $"Profile Picture: {ProfilePictureImage}";
    }

    // Set string picture path into byte and save to ProfilePicture
    public bool SetProfilePicture(string profilePicturePath, out string message)
    {
        try
        {
            byte[] formattedProfilePicture = _imageViewer.LoadProfilePicture(profilePicturePath);

            if (_imageViewer.IsValidImageFormat(formattedProfilePicture))
            {
                ProfilePictureImage = _imageViewer.ConvertByteArrayToImage(formattedProfilePicture);
                ProfilePictureByte = formattedProfilePicture;
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

    public void SetProfilePicture(Image? profilePictureImage)
    {
        ProfilePictureImage = profilePictureImage;
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