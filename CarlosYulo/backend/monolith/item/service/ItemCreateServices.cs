using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.shop;

namespace CarlosYulo.backend.monolith.item;

public class ItemCreateServices
{
    private readonly ItemCreate _create;
    private readonly ErrorMessageBox _messageBox;

    public ItemCreateServices(ItemCreate create)
    {
        _create = create;
        _messageBox = new ErrorMessageBox();
    }

    public bool Create(Item item)
    {
        string message;
        if (!_create.Create(item, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }
}