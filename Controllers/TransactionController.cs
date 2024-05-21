﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Wallet.Data;
using Digital_Wallet.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Digital_Wallet.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var transactions = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .ToList();
            return View(transactions);
        }

        public IActionResult CreateOrEdit()
        {
            FillCategories();
            return View(new Transaction());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            transaction.UserId = userId;

            if (ModelState.IsValid)
            {
                if (transaction.TransactionId == 0)
                {
                    _context.Add(transaction);
                }
                else
                {
                    _context.Update(transaction);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            FillCategories();
            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public void FillCategories()
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
        }
    }
}
