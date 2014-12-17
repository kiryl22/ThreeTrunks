using ThreeTrunks.Data.Enums;

namespace ThreeTrunks.Data.Models
{
    public class Settings : BaseModel
    {
        public string SettingKey { get; set; }

        public string SettingValue { get; set; }

        public SettingTypes SettingType { get; set; }

        public string SettingCaption { get; set; }
    }
}
