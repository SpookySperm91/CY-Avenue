using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.shop;

namespace CarlosYulo.backend.monolith.item;

public class ItemSearchServices
{
    private readonly ItemSearchAll _searchAll;
    private readonly ItemSearchByCategory _searchByCategory;
    private readonly ItemSearchById _searchById;
    private readonly ErrorMessageBox _messageBox;


    public ItemSearchServices(
        ItemSearchAll searchAll,
        ItemSearchByCategory searchByCategory,
        ItemSearchById searchById)
    {
        _searchAll = searchAll;
        _searchByCategory = searchByCategory;
        _searchById = searchById;
        _messageBox = new ErrorMessageBox();
    }


    public List<Item>? SearchAll()
    {
        string message;
        var result = _searchAll.SearchAll(out message);

        if (result is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return result;
    }

    public List<Item>? SearchByCategory(ItemCategory category)
    {
        string message;
        var result = _searchByCategory.SearchByCategory(category, out message);

        if (result is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return result;
    }


    public Item? SearchItemById(int itemId)
    {
        string message;
        var result = _searchById.SearchItemById(itemId, out message);
        if (result is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return result;
    }
}