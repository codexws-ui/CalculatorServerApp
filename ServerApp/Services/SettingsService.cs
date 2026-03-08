using Microsoft.EntityFrameworkCore;
using ServerApp.Data;
using ServerApp.Models;

namespace ServerApp.Services
{
    public class SettingsService
    {
        public async Task<Settings> Get(CalculatorDB db)
        {
            Settings? settings = await db.Settings.FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new Settings { Id = Guid.NewGuid().ToString() };

                await Set(db, settings, true);
            }

            return settings;
        }

        public async Task Set(CalculatorDB db, Settings settings, bool isNew = false)
        {
            if (isNew)
            {
                db.Settings.Add(settings);
            }
            else
            {
                db.Settings.Update(settings);
            }

            await db.SaveChangesAsync();
        }
    }
}
