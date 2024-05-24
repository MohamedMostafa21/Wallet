using Microsoft.AspNetCore.Authorization;
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

        public IActionResult CreateOrEdit(int id =0)
        {
            FillCategories();
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == null)
            {
                return Unauthorized();
            }

            if (id == 0)
            {
                return View(new Transaction { UserId = userId });
            }
            else
            {
                var transaction = _context.Transactions.FirstOrDefault(c => c.TransactionId == id && c.UserId == userId);
                if (transaction == null)
                {
                    return NotFound();
                }
                return View(transaction);
            }
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
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var categories = _context.Categories.Where(t => t.UserId == userId).ToList();
            ViewBag.Categories = categories;
        }
    }
}
