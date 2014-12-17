using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeTrunks.Data.Models;

namespace ThreeTrunks.Data.Repositories
{
    using System.Security.Cryptography.X509Certificates;

    public class SettingsRepository : GenericRepository<Settings>
    {
        public SettingsRepository(ThreeTrunksContext context)
            : base(context)
        {

        }

        public Settings GetBySettingKey(string key)
        {
            return context.Settings.FirstOrDefault(s =>s.SettingKey.ToLower() == key.ToLower());
        }

        public bool IsSettingSettingsExist(string key)
        {
            return context.Settings.Any(s => String.Equals(s.SettingKey, key, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
