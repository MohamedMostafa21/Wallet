using Digital_Wallet.Data;
using Digital_Wallet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace Digital_Wallet.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            // Last 7 Days Transactions
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;
            List<Transaction> WantedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate && y.UserId == userId)
                .ToListAsync();
            // Total Income
            int TotalIncome = WantedTransactions
                .Where(i => i.Category.Type == "Income" && i.UserId == userId)
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

            // Total Expenses
            int TotalExpense = WantedTransactions
                .Where(i => i.Category.Type == "Expense" && i.UserId == userId)
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            // Balance
            int Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;

            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);


            //Donut Chart Expense By Category
            ViewBag.DountData = WantedTransactions.Where(i => i.Category.Type == "Expense" && i.UserId == userId)
            .GroupBy(j => j.Category.CategoryId)
            .Select(k => new
            {
                categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                amount = k.Sum(j => j.Amount),
                formattedAmount = k.Sum(j => j.Amount).ToString("CO"),
            })
            .OrderByDescending(l => l.amount).ToList();

            // Chart Data Income And Xxpenses

            // Income Data
            List<ChartData> IncomeData = WantedTransactions
                .Where(c => c.Category.Type == "Income")
                .GroupBy(d => d.Date)
                .Select(k => new ChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            // Expense Data
            List<ChartData> ExpenseData = WantedTransactions
                .Where(c => c.Category.Type == "Expense")
                .GroupBy(d => d.Date)
                .Select(k => new ChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            //AllData
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.ChartData = from day in Last7Days
                                join income in IncomeData on day equals income.day into dayIncomeJoined
                                from income in dayIncomeJoined.DefaultIfEmpty()
                                join expense in ExpenseData on day equals expense.day into expenseJoined
                                from expense in expenseJoined.DefaultIfEmpty()
                                select new
                                {
                                    day = day,
                                    income = income == null ? 0 : income.income,
                                    expense = expense == null ? 0 : expense.expense,
                                };
            return View();
        }
    }
    public class ChartData
    {
        public string day { get; set; }
        public int income { get; set; }
        public int expense { get; set; }
    }
}
