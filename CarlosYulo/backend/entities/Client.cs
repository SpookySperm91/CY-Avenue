using System.ComponentModel;
using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend;

public class Client
{
    // Only variables to(should) be accessed in the surface level
    public string? FullName { get; set; }
    public int? MembershipTypeId { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public int? Age { get; set; }
    public DateTime? BirthDate { get; set; }
    // bool SetProfilePicture(string profilePicturePath, out string message) { }   // method to convert image-path to byte then image. Auto save in their respective variable   


    // Non mutable in surface level
    public int MembershipId { get; set; }
    public string? Membership { get; private set; }
    public byte[]? ProfilePictureByte { get; set; }
    public Image? ProfilePictureImage { get; private set; }
    public DateTime? MembershipStart { get; set; }
    public DateTime? MembershipEnd { get; set; }
    public string? MembershipStatus { get; set; }

    private ImageViewer _imageViewer;

    public Client()
    {
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
               $"Profile Picture: {(ProfilePictureImage != null ? "Image Set" : "N/A")}";
    }

    // Set string picture path into byte and save to ProfilePicture
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
                ProfilePictureImage = _imageViewer.ConvertByteArrayToImage(formattedProfilePicture); // Image 
                ProfilePictureByte = formattedProfilePicture; // byte[] 
                return;
            }

            MessageBox.Show(
                "Profile picture could not be loaded. Invalid image format. Only PNG and JPEG are supported.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (FileNotFoundException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (InvalidDataException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }

    public void SetProfilePictureImage(Image? profilePictureImage)
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
    [Description("Basic")] BASIC,
    [Description("VIP")] VIP,
    [Description("Walk-in")] WALK_IN,
}

public enum MembershipStatus
{
    [Description("Inactive")] INACTIVE,
    [Description("Active")] ACTIVE
}