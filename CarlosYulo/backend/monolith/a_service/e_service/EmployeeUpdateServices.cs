using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.employee.update;

namespace CarlosYulo.backend.monolith.employee;

public class EmployeeUpdateServices
{
    private readonly EmployeeUpdateDetails _updateDetails;
    private readonly EmployeeUpdateProfilePicture _updateProfilePicture;
    private readonly ErrorMessageBox _errorMessageBox;

    public EmployeeUpdateServices(
        EmployeeUpdateDetails updateDetails,
        EmployeeUpdateProfilePicture updateProfilePicture)
    {
        _updateDetails = updateDetails;
        _updateProfilePicture = updateProfilePicture;
        _errorMessageBox = new ErrorMessageBox();
    }

    public bool UpdateDetails(Employee employee)
    {
        string message;
        if (!_updateDetails.UpdateDetails(employee, out message))
        {
            _errorMessageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }


    public bool UpdateProfilePicture(Employee employee, string picturePath)
    {
        string message;
        if (!_updateProfilePicture.UpdateProfilePicture(employee, picturePath, out message))
        {
            _errorMessageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }
}