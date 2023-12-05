using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.ViewModels;

namespace Pustok.Controllers;

public class HomeController : Controller
{
    private readonly PustokContext _context;

    public HomeController(PustokContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        HomeViewModel homeVm = new HomeViewModel()
        {
            Sliders = _context.Sliders.ToList(),
            FeaturedBooks = _context.Books.Include(x=>x.Author).Include(x=>x.BookImages).Where(x=>x.IsFeatured).ToList(),
            NewBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x=>x.IsNew).ToList(),
            BestsellerBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x=>x.IsBestseller).ToList()
        };
        return View(homeVm);
    }
}