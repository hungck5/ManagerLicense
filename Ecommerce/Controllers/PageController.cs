using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

public class PageController : Controller
{
    // private readonly IPageService _pageService;

    // public PageController(IPageService pageService)
    // {
    //     _pageService = pageService;
    // }

    [HttpGet]
    public IActionResult RenderPage()
    {
        // var pages = _pageService.GetAllPages();
        return View();
    }

    // [HttpGet("{id}")]
    // public IActionResult Details(int id)
    // {
    //     var page = _pageService.GetPageById(id);
    //     if (page == null)
    //     {
    //         return NotFound();
    //     }
    //     return View(page);
    // }
}