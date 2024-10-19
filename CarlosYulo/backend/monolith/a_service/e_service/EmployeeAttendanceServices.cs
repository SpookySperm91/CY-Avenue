using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.employee.attendance;

namespace CarlosYulo.backend.monolith.employee;

public class EmployeeAttendanceServices
{
    private readonly EmployeeAttendanceCreate _attendanceCreate;
    private readonly EmployeeAttendanceSearchAll _attendanceSearchAll;
    private readonly EmployeeAttendanceSearchDaily _attendanceSearchDaily;
    private readonly EmployeeAttendanceSearchMonthly _attendanceSearchMonthly;
    private readonly ErrorMessageBox _errorMessageBox;

    public EmployeeAttendanceServices(
        EmployeeAttendanceCreate attendanceCreate,
        EmployeeAttendanceSearchAll attendanceSearchAll,
        EmployeeAttendanceSearchDaily attendanceSearchDaily,
        EmployeeAttendanceSearchMonthly attendanceSearchMonthly)
    {
        _attendanceCreate = attendanceCreate;
        _attendanceSearchAll = attendanceSearchAll;
        _attendanceSearchDaily = attendanceSearchDaily;
        _attendanceSearchMonthly = attendanceSearchMonthly;
        _errorMessageBox = new ErrorMessageBox();
    }


    public bool CreateEmployeeAttendance(int? employeeId, AttendanceStatus attendanceStatus)
    {
        string message;
        if (!_attendanceCreate.CreateEmployeeAttendance(employeeId, attendanceStatus, out message))
        {
            _errorMessageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public EmployeeAttendance? SearchEmployeeDailyAttendance(int employeeId, DateTime attendanceDate)
    {
        string message;
        var attendance = _attendanceSearchDaily.SearchEmployeeDailyAttendance(employeeId, attendanceDate, out message);
        if (attendance is null)
        {
            _errorMessageBox.ShowErrorMessage(message);
            return null;
        }

        return attendance;
    }

    public List<EmployeeAttendance>? SearchEmployeeMonthlyAttendance(int employeeId, DateTime attendanceDate)
    {
        string message;
        var monthlyAttendance =
            _attendanceSearchMonthly.SearchEmployeeMonthlyAttendance(employeeId, attendanceDate, out message);
        if (monthlyAttendance is null || !monthlyAttendance.Any())
        {
            _errorMessageBox.ShowErrorMessage(message);
            return null;
        }

        return monthlyAttendance;
    }

    public List<EmployeeAttendance> SearchAll(DateTime checkDate, AttendanceType? procedure)
    {
        string message;
        var attendance = _attendanceSearchAll.SearchAll(checkDate, procedure, out message);
        if (attendance is null || !attendance.Any())
        {
            _errorMessageBox.ShowErrorMessage(message);
            return null;
        }

        return attendance;
    }
}