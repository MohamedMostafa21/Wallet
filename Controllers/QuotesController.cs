using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Digital_Wallet.Controllers
{
    [Authorize]
    public class QuotesController : Controller
    {
        private static readonly List<string> Quotes = new List<string>
        {
            "A budget is telling your money where to go instead of wondering where it went.",
            "Do not save what is left after spending, but spend what is left after saving.",
            "The art is not in making money, but in keeping it.",
            "Financial peace isn't the acquisition of stuff. It's learning to live on less than you make.",
            "Beware of little expenses; a small leak will sink a great ship.",
            "The quickest way to double your money is to fold it in half and put it in your back pocket.",
            "It's not about how much money you make, but how much money you keep.",
            "Financial fitness is not a pipe dream or a state of mind. It’s a reality if you are willing to pursue it and embrace it.",
            "Never spend your money before you have it.",
            "Financial freedom is available to those who learn about it and work for it.",
            "The best investment you can make is in yourself.",
            "It’s not your salary that makes you rich, it’s your spending habits.",
            "An investment in knowledge pays the best interest.",
            "The secret to wealth is simple: Find a way to do more for others than anyone else does.",
            "Rich people have small TVs and big libraries, and poor people have small libraries and big TVs.",
            "Don’t let the fear of losing be greater than the excitement of winning.",
            "The goal isn’t more money. The goal is living life on your terms.",
            "Save money, and money will save you.",
            "Wealth consists not in having great possessions, but in having few wants.",
            "The more your money works for you, the less you have to work for money."
        };

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetRandomQuote()
        {
            var random = new Random();
            int index = random.Next(Quotes.Count);
            return Json(Quotes[index]);
        }
    }
}
