using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[Route("san-pham")]
public class ProductController : Controller
{
  // private readonly IProductService _productService;

  // public ProductController(IProductService productService)
  // {
  //     _productService = productService;
  // }

  [HttpGet]
  public IActionResult Index()
  {
    // var products = _productService.GetAllProducts();
    return View();
  }

  // [HttpGet("{id}")]
  // public IActionResult Details(int id)
  // {
  //     var product = _productService.GetProductById(id);
  //     if (product == null)
  //     {
  //         return NotFound();
  //     }
  //     return View(product);
  // }
    
    [HttpGet("{id}")]
    public IActionResult Detail(int id)
    {
        // var product = _productService.GetProductById(id);
        // if (product == null)
        // {
        //     return NotFound();
        // }
        return View();
    }
}