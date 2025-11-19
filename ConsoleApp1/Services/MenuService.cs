// Services/MenuService.cs
public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    
    public MenuService(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }
    
    public async Task<List<Menu>> GetMenuByCategoriesAsync()
    {
        var categories = await _menuRepository.GetCategoriesWithMenusAsync();
        return categories.SelectMany(c => c.Menus).ToList();
    }
    
    public async Task<bool> UpdateMenuPriceAsync(int menuId, decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Цена должна быть положительной");
            
        return await _menuRepository.UpdateMenuPriceAsync(menuId, newPrice);
    }
}