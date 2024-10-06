namespace CarlosYulo.backend.monolith.common;

public class ImageViewer
{
    public Image ConvertByteArrayToImage(byte[] imageData)
    {
        try
        {
            using (var ms = new MemoryStream(imageData))
            {
                return Image.FromStream(ms);
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Invalid image data: {ex.Message}");
            throw;
        }
    }

    public byte[] LoadProfilePicture(string imagePath)
    {
        try
        {
            if (File.Exists(imagePath))
            {
                return File.ReadAllBytes(imagePath);
            }
            else
            {
                throw new FileNotFoundException("Image file does not exist.", imagePath);
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw; // Optionally rethrow after logging
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading the image: {ex.Message}");
            throw; // Optionally rethrow or handle as needed
        }
    }


    public bool IsValidImageFormat(byte[] profilePicture)
    {
        // Check if the byte array is not null or empty
        if (profilePicture == null || profilePicture.Length == 0)
        {
            return false;
        }

        byte[] jpegMagicNumber = { 0xFF, 0xD8, 0xFF };
        byte[] pngMagicNumber = { 0x89, 0x50, 0x4E, 0x47 };

        if (profilePicture.Length >= jpegMagicNumber.Length &&
            profilePicture.Take(jpegMagicNumber.Length).SequenceEqual(jpegMagicNumber))
        {
            return true; // Valid JPEG
        }

        if (profilePicture.Length >= pngMagicNumber.Length &&
            profilePicture.Take(pngMagicNumber.Length).SequenceEqual(pngMagicNumber))
        {
            return true; // Valid PNG
        }

        return false; // Not a valid PNG or JPEG
    }
}