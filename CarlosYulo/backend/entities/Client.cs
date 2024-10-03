using System.ComponentModel;

namespace CarlosYulo.backend;

public class Client
{
    public  int MembershipId { get;  set; }
    public  string ProfilePicture { get;  set; }
    public  string FullName { get;  set; }
    public  int MembershipType { get;  set; }
    public  string Email { get;  set; }
    public  string PhoneNumber { get;  set; }
    public  string Gender { get;  set; }
    public  int Age { get; set; }
    public  DateTime BirthDate { get;  set; }
    public  DateTime MembershipStart { get;  set; }
    public  DateTime MembershipEnd { get;  set; }
    public string MembershipStatus { get;  set; }

    public Client() { }
    
    public Client(int membershipId, string profilePicture, string fullName, int membershipType,
        string email, string phoneNumber, string gender, int age, DateTime birthDate,
        DateTime membershipStart, DateTime membershipEnd, string membershipStatus)
    {
        MembershipId = membershipId;
        ProfilePicture = profilePicture;
        FullName = fullName;
        MembershipType = membershipType;
        Email = email;
        PhoneNumber = phoneNumber;
        Gender = gender;
        Age = age;
        BirthDate = birthDate;
        MembershipStart = membershipStart;
        MembershipEnd = membershipEnd;
        MembershipStatus = membershipStatus;
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


