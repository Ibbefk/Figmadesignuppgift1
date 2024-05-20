using Microsoft.AspNetCore.Mvc;
using Figmadesign.Data;
using Figmadesign.Models;
using System.Threading.Tasks;

namespace Figmadesign.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsletterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(NewsletterSubscription subscription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscription);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(subscription);
        }
    }
}
