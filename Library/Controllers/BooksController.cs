#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder)
        {
            //var applicationDbContext = _context.Books.Include(b => b.Authors);

            ViewData["BookNameSort"] = String.IsNullOrEmpty(sortOrder) ? "BookName" : "";
            ViewData["PriceSort"] = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewData["CategorySort"] = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewData["AuthorSort"] = sortOrder == "Author" ? "Author_desc" : "Author";

            var Books = (from s in _context.Books
                        select s).Include(b => b.Authors).ToList();

            switch (sortOrder)
            {
                case "BookName":
                    Books = Books.OrderByDescending(s => s.BookName).ToList();
                    break;
                case "Author":
                    Books = Books.OrderBy(s => s.Authors.AuthorName).ToList();
                    break;
                case "Author_desc":
                    Books = Books.OrderByDescending(s => s.Authors.AuthorName).ToList();
                    break;
                case "Price":
                    Books = Books.OrderBy(s => s.Price).ToList();
                    break;
                case "Price_desc":
                    Books = Books.OrderByDescending(s => s.Price).ToList();
                    break;
                case "Category":
                    Books = Books.OrderBy(s => s.Category).ToList();
                    break;
                case "Category_desc":
                    Books = Books.OrderByDescending(s => s.Category).ToList();
                    break;
                default:
                    Books = Books.OrderBy(s => s.BookName).ToList();
                    break;
            }

            return View(Books.ToList());
        }
    
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookName,AuthorId,Color,Size,Price,Material,Category")] Books books)
        {
            ModelState.Remove("Authors");
            if (ModelState.IsValid)
            {
                _context.Add(books);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", books.AuthorId);
            return View(books);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", books.AuthorId);
            return View(books);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,BookName,AuthorId,Color,Size,Price,Material,Category")] Books books)
        {
            if (id != books.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(books);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(books.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", books.AuthorId);
            return View(books);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var books = await _context.Books.FindAsync(id);
            _context.Books.Remove(books);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
