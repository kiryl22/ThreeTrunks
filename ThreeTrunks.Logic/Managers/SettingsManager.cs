using System.Collections.Generic;
using System.Linq;

namespace ThreeTrunks.Logic.Managers
{
    using Data.Models;
    using Data.Repositories;

    public class SettingsManager
    {
        private readonly UnitOfWork _unitOfWork;

        public SettingsManager(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string GetValue(string key)
        {
            var setting = _unitOfWork.SettingsRepository.GetBySettingKey(key);
            return setting != null ? setting.SettingValue : null;
        }

        public Settings GetSetting(string key)
        {
            return _unitOfWork.SettingsRepository.GetBySettingKey(key);
        }

        public List<Settings> GetSettings()
        {
            return _unitOfWork.SettingsRepository.Get().ToList();
        }

        public Settings SetValue(string key, string value)
        {
            if (_unitOfWork.SettingsRepository.IsSettingSettingsExist(key))
            {
                var setting = _unitOfWork.SettingsRepository.GetBySettingKey(key);
                setting.SettingValue = value;
                _unitOfWork.Save();
                return setting;
            }

            return null;
        }

        public Settings SetValue(Settings setting)
        {
            _unitOfWork.SettingsRepository.Update(setting);
            _unitOfWork.Save();
            return setting;
        }

        public Settings CreateNewSettings(string key, string value)
        {
            if (!_unitOfWork.SettingsRepository.IsSettingSettingsExist(key))
            {
                _unitOfWork.SettingsRepository.Insert(new Settings
                    {
                                                             SettingKey = key,
                                                             SettingValue = value
                                                         });

                _unitOfWork.Save();

            }

            var setting = _unitOfWork.SettingsRepository.GetBySettingKey(key);
            return setting;
        }
    }
}
