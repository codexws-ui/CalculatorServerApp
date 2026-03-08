using Microsoft.AspNetCore.Mvc;
using ServerApp.Data;
using ServerApp.Models;
using ServerApp.Services;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ServerApp.API
{
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class CalculatorApiController : ControllerBase
    {
        private readonly CalculatorDB db;
        private readonly CalculatorService calculatorService;
        private readonly SettingsService settingsService;

        public CalculatorApiController(CalculatorDB _db, CalculatorService _calculatorService, SettingsService _settingsService)
        {
            db = _db;
            calculatorService = _calculatorService;
            settingsService = _settingsService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            return Ok(new { operations = await GetOperations(db) });
        }

        [HttpPost("post")]
        public IActionResult Calculate([FromBody] Calculator calculator)
        {
            return Ok(new { result = calculatorService.Calculate(calculator) });
        }

        private async Task<Dictionary<int, string>> GetOperations(CalculatorDB db)
        {
            Settings settings = await settingsService.Get(db);
            IEnumerable<int> selected = settings.SelectedOperations?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse) ?? [];

            return Enum.GetValues(typeof(OperationType))
                .Cast<OperationType>()
                .Where(o => selected.Contains((int)o))
                .ToDictionary(o => (int)o, o => GetDisplayName(o));
        }

        private static string GetDisplayName(Enum value)
        {
            if (value == null) return string.Empty;

            var member = value.GetType().GetMember(value.ToString());

            if (member?.Length > 0)
            {
                var display = member[0].GetCustomAttribute<DisplayAttribute>();

                if (display != null)
                {
                    return display.GetName() ?? display.Name ?? value.ToString();
                }
            }

            return value.ToString();
        }
    }
}
