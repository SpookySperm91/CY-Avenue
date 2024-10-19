namespace CarlosYulo.backend.monolith.common;

public class ErrorMessageBox
{
    public void ShowErrorMessage(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    
    public void ShowSuccessMessage(string message)
    {
        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}