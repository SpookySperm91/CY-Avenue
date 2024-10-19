using CarlosYulo.backend.monolith.shop;

namespace CarlosYulo.backend.monolith.item;

public class ItemController
{
    private readonly ItemCreateServices _create;
    private readonly ItemSearchServices _search;
    private readonly ItemUpdateServices _update;


    public ItemController(
        ItemCreateServices create, 
        ItemSearchServices search, 
        ItemUpdateServices update)
    {
        _create = create;
        _search = search;
        _update = update;
    }

    // CREATE 
    public bool Create(Item item)
    {
        return _create.Create(item);
    }

    
    // SEARCH
    public List<Item>? SearchAll()
    {
        return _search.SearchAll();
    }

    public List<Item>? SearchByCategory(ItemCategory category)
    {
        return _search.SearchByCategory(category);
    }


    public Item? SearchItemById(int itemId)
    {
        return _search.SearchItemById(itemId);
    }
    
    
    // UPDATE 
    public bool BuyItem(List<Item> itemList)
    {
        return _update.BuyItem(itemList);
    }

    public bool AddQuantity(Item item, int? quantity)
    {
        return _update.AddQuantity(item, quantity);
    }

    public bool UpdateDetails(Item? item)
    {
        return _update.UpdateDetails(item);
    }
}