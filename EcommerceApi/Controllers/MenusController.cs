using EcommerceApi.Services.Interfaces;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using EcommerceApi.DTOs;

namespace EcommerceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenusController : ControllerBase
{
    private readonly IMenuService _service;

    public MenusController(IMenuService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems()
    {
        return Ok(await _service.GetAllMenuItemsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MenuItem>> GetMenuItem(Guid id)
    {
        var menuItem = await _service.GetMenuItemByIdAsync(id);
        if (menuItem == null) return NotFound();
        return Ok(menuItem);
    }

    // [HttpPost]
    // public async Task<ActionResult<MenuItem>> CreateMenuItem(MenuItemDTO menuItemDto)
    // {
    //     var created = await _service.CreateMenuItemAsync(menuItemDto);
    //     return CreatedAtAction(nameof(GetMenuItem), new { id = created.Id }, created);
    // }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMenuItem(Guid id, MenuItemDTO menuItemDto)
    {
        if (id != menuItemDto.Id) return BadRequest();

        var success = await _service.UpdateMenuItemAsync(menuItemDto);
        if (success == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMenuItem(Guid id)
    {
        var deletedMenuItem = await _service.DeleteMenuItemAsync(id);
        if (deletedMenuItem == null) return NotFound();
        return NoContent();
    }
}
