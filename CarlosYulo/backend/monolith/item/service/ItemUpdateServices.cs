using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.shop;

namespace CarlosYulo.backend.monolith.item;

public class ItemUpdateServices
{
    private readonly ItemBuy _buy;
    private readonly ItemRestockQuantity _restockQuantity;
    private readonly ItemUpdateDetails _updateDetails;
    private readonly ErrorMessageBox _messageBox;


    public ItemUpdateServices(
        ItemBuy buy,
        ItemRestockQuantity restockQuantity,
        ItemUpdateDetails updateDetails)
    {
        _buy = buy;
        _restockQuantity = restockQuantity;
        _updateDetails = updateDetails;
    }

    public bool BuyItem(List<Item> itemList)
    {
        string message;
        if (!_buy.BuyItem(itemList, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public bool AddQuantity(Item item, int? quantity)
    {
        string message;
        if (!_restockQuantity.AddQuantity(item, quantity, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public bool UpdateDetails(Item? item)
    {
        string message;
        if (!_updateDetails.UpdateDetails(item, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }
}