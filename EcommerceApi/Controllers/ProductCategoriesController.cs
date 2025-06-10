using EcommerceApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Services;

namespace EcommerceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductCategoriesController : ControllerBase
{
    private readonly IProductCategoryService _service;

    public ProductCategoriesController(IProductCategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductCategoryDTO>>> GetProductCategories()
    {
        return Ok(await _service.GetAllProductCategoriesAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductCategoryDTO>> GetProductCategory(Guid id)
    {
        var productCategory = await _service.GetProductCategoryByIdAsync(id);
        if (productCategory == null) return NotFound();
        return Ok(productCategory);
    }

    [HttpPost]
    public async Task<ActionResult<ProductCategoryDTO>> CreateProductCategory(ProductCategoryDTO productCategoryDto)
    {
        var created = await _service.CreateProductCategoryAsync(productCategoryDto);
        return CreatedAtAction(nameof(GetProductCategory), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductCategory(Guid id, ProductCategoryDTO productCategoryDto)
    {
        var success = await _service.UpdateProductCategoryAsync(productCategoryDto);
        if (success == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductCategory(Guid id)
    {
        var success = await _service.DeleteProductCategoryAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
