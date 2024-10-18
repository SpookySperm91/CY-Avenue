using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend.monolith.shop;

public class Item
{
    public int? ItemId { get; set; }
    public string? ItemName { get; set; }
    public string? ItemDescription { get; set; }
    public string? ItemCategory { get; set; }
    public double? ItemPrice { get; set; }
    public int? ItemQuantity { get; set; }
    public double? ItemLiabilityCost { get; set; }
    public int? QuantityToBuy { get; set; }
    public DateTime? DateRestocked { get; set; }
    
    // inside job 
    private ImageViewer _imageViewer;
    public byte[]? ItemPictureBytes { get; set; }
    public Image? ItemPicture { get; private set; }


    public Item()
    {
        _imageViewer = new ImageViewer();
        
    }
    
    public override string ToString()
    {
        return $"Item ID: {ItemId}, " +
               $"Item Name: {ItemName}, " +
               $"Description: {ItemDescription}, " +
               $"Price: {ItemPrice:C}, " +
               $"Quantity: {ItemQuantity}, " +
               $"Liability Cost: {ItemLiabilityCost:C}, " +
               $"Image Size: {(ItemPictureBytes != null ? ItemPictureBytes.Length.ToString() : "No Image")}";
    }

    
    public bool SetProfilePicture(string itemPicturePath, out string message)
    {
        try
        {
            byte[] formattedItemPicture = _imageViewer.LoadProfilePicture(itemPicturePath);

            if (_imageViewer.IsValidImageFormat(formattedItemPicture))
            {
                ItemPicture = _imageViewer.ConvertByteArrayToImage(formattedItemPicture); // Image 
                ItemPictureBytes = formattedItemPicture; // byte[] 
                message = "Item Picture Set";
                return true;
            }

            message = "Item picture could not be loaded. Invalid image format. Only PNG and JPEG are supported.";
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


    public void SetProfilePicture(string itemPicturePath)
    {
        try
        {
            byte[] formattedProfilePicture = _imageViewer.LoadProfilePicture(itemPicturePath);

            if (_imageViewer.IsValidImageFormat(formattedProfilePicture))
            {
                ItemPicture = _imageViewer.ConvertByteArrayToImage(formattedProfilePicture); // Image 
                ItemPictureBytes = formattedProfilePicture; // byte[] 
                return;
            }

            MessageBox.Show(
                "Item picture could not be loaded. Invalid image format. Only PNG and JPEG are supported.", "Error",
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
    
    public void SetProfilePictureImage(Image? itemPictureImage)
    {
        ItemPicture = itemPictureImage;
    }
}


public enum ItemCategory
{
    MERCHANDISE,
    EQUIPMENT,
    SUPPLEMENT
}



