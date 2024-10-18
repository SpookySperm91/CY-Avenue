namespace CarlosYulo.backend.monolith.common;

public class ImageViewer
{
    public Image? ConvertByteArrayToImage(byte[]? imageData)
    {
        // Check for null or empty input
        if (imageData == null || imageData.Length == 0)
        {
            Console.WriteLine("No image data provided.");
            return null; // Return null if there's no data
        }

        try
        {
            using (var ms = new MemoryStream(imageData))
            {
                return Image.FromStream(ms, true, true); // Keep the stream open
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Invalid image data: {ex.Message}");
            return null; // Return null in case of an error
        }
        catch (Exception ex) // Catch any other potential exceptions
        {
            Console.WriteLine($"An error occurred while converting byte array to image: {ex.Message}");
            return null; // Return null for unexpected errors
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

        // JPEG Magic Number (SOI marker)
        byte[] jpegMagicNumber = { 0xFF, 0xD8 };
        // PNG Magic Number
        byte[] pngMagicNumber = { 0x89, 0x50, 0x4E, 0x47 };
   

        // Check if it's a valid JPEG
        if (profilePicture.Length >= jpegMagicNumber.Length &&
            profilePicture.Take(jpegMagicNumber.Length).SequenceEqual(jpegMagicNumber))
        {
            return true; // Valid JPEG
        }

        // Check if it's a valid PNG
        if (profilePicture.Length >= pngMagicNumber.Length &&
            profilePicture.Take(pngMagicNumber.Length).SequenceEqual(pngMagicNumber))
        {
            return true; // Valid PNG
        }

        return false; // Not a valid JPEG, PNG, or GIF
    }

}