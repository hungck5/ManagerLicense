using EcommerceApi.DTOs;
using EcommerceApi.Models;

namespace EcommerceApi.Services.Interfaces;

public interface IMenuService
{ 
    Task<MenuItem?> GetMenuItemByIdAsync(Guid id);
    Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();
    Task<MenuItem?> CreateMenuItemAsync(MenuItem menuItem);
    Task<MenuItem?> UpdateMenuItemAsync(MenuItemDTO menuItemDto);
    Task<bool> DeleteMenuItemAsync(Guid id);
}