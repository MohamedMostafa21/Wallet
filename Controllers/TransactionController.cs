using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Digital_Wallet.Data;
using Digital_Wallet.Models;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Digital_Wallet.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var transactions = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .ToList();

            return View(transactions);
        }


        // GET: Transaction/Create
        public IActionResult CreateOrEdit()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            FillCategories(userId.Value);
            return View(new Transaction());
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            var userId = HttpContext.Session.GetInt32("Id");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            transaction.UserId = userId.Value;
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If model state is not valid, refill categories
            FillCategories(transaction.UserId);
            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public void FillCategories(int? userId)
        {
            var categories = _context.Categories
                .Where(c => c.UserId == userId)
                .ToList();

            // Add a default category for selection
            Category DefaultCategory = new Category() { CategoryId = 0, Title = "Choose a Category" };
            categories.Insert(0, DefaultCategory);

            ViewBag.Categories = categories;
        }
    }
}
