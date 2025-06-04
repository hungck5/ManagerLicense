using EcommerceApi.Db;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Services;

public class MenuService : IMenuService
{
    private readonly ApplicationDbContext _context;

    public MenuService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<MenuItem?> GetMenuItemByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
    {
       return await _context.MenuItems.ToListAsync();
    }

    public async Task<MenuItem?> CreateMenuItemAsync(MenuItem menuItem)
    {
        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync();
        return menuItem;
    }

    public async Task<MenuItem?> UpdateMenuItemAsync(MenuItemDTO menuItemDto)
    {
        var menuItem = await _context.MenuItems.FindAsync(menuItemDto.Id);
        if (menuItem == null) return null;
        
        var newMenuItem = new MenuItem(
            menuItemDto.Id,
            menuItemDto.Name,
            new SeoMeta { Slug = menuItemDto.Url },
            menuItemDto.Order,
            menuItemDto.ParentId
        );
        menuItem.Update(newMenuItem);

        _context.MenuItems.Update(menuItem);
        await _context.SaveChangesAsync();
        return menuItem;
    }

    public Task<bool> DeleteMenuItemAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}