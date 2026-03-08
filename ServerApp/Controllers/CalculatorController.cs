using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Data;
using ServerApp.Models;
using ServerApp.Services;

namespace ServerApp.Controllers
{
    //[Authorize]
    public class CalculatorController : Controller
    {
        private readonly CalculatorDB db;
        private readonly CalculatorService calculatorService;
        private readonly SettingsService settingsService;

        public CalculatorController(CalculatorDB _db, CalculatorService _calculatorService, SettingsService _settingsService)
        {
            db = _db;
            calculatorService = _calculatorService;
            settingsService = _settingsService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Settings = await settingsService.Get(db);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(Calculator calculator)
        {
            if (ModelState.IsValid)
            {
                string result = calculatorService.Calculate(calculator);

                if (result.StartsWith("ERROR"))
                {
                    ModelState.AddModelError(string.Empty, result);
                }
                else
                {
                    calculator.Result = result;
                }
            }

            ViewBag.Settings = await settingsService.Get(db);
            ModelState.Remove("Result");

            return View("Index", calculator);
        }
    }
}
