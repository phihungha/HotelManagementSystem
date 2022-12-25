using HotelManagementSoftware.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelManagementSoftware.Business
{
    public class ConfigurationBusiness
    {
        public async Task<int?> GetConfig(string name)
        {
            using (var db = new Database())
            {
                Configuration? config = await db.Configurations.FirstOrDefaultAsync(i => i.Name == name);
                if (config == null)
                    return null;
                return config.Value;
            }
        }

        public async Task SetConfig(string name, int value)
        {
            using (var db = new Database())
            {
                Configuration? config = await db.Configurations.FirstOrDefaultAsync(i => i.Name == name);
                if (config == null)
                {
                    config = new Configuration() { Name = name, Value = value };
                    db.Add(config);
                }
                else
                    config.Value = value;
                await db.SaveChangesAsync();
            }
        }
    }
}
