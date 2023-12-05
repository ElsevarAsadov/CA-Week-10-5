using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Business.Exceptions;
using Pustok.Business.Extensions;
using Pustok.Models;
using Pustok.Business.Services.Interfaces;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("manage")]
    public class BookController : Controller
    {
        private readonly PustokContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IBookService _bookService;

        public BookController(PustokContext context, IWebHostEnvironment env, IBookService bookService)
        {
            _context = context;
            _env = env;
            _bookService = bookService;
        }
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllAsync();
            return View(books);
        }

        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _bookService.CreateAsync(book);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidImageContentException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();


            Book existBook = await _bookService.GetByIdAsync(id);

            if (existBook == null) return NotFound();

            foreach (var item in existBook.BookTags)
            {
                existBook.TagIds.Add(item.TagId);
            }

            existBook.TagIds = existBook.BookTags.Select(x => x.TagId).ToList();

            return View(existBook);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _bookService.UpdateAsync(book);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidImageContentException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidImageSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }


            return RedirectToAction("Index");
        }
    }
}
