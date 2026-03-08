using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Data;
using ServerApp.Models;
using ServerApp.Services;

namespace ServerApp.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly CalculatorDB db;
        private readonly SettingsService settingsService;

        public SettingsController(SettingsService _settingsService, CalculatorDB _db)
        {
            db = _db;
            settingsService = _settingsService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await settingsService.Get(db));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(List<int> operations)
        {
            Settings settings = await settingsService.Get(db);

            settings.SelectedOperations = operations.Count > 0 ? string.Join(",", operations) : null;

            try
            {
                await settingsService.Set(db, settings);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Index", settings);
        }
    }
}
