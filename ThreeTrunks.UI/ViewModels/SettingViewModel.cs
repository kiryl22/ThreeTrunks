using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ThreeTrunks.Data.Enums;

namespace ThreeTrunks.UI.ViewModels
{
    using Newtonsoft.Json;

    using ThreeTrunks.Data.Models;

    public class SettingViewModel
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        public static Settings Map(SettingViewModel viewModel)
        {
            if(viewModel == null)
                return new Settings();

            SettingTypes settingType;
            if (!Enum.TryParse(viewModel.Type.ToString(), out settingType))
            {
                settingType = SettingTypes.Other;
            }

            return new Settings()
                       {
                           SettingKey = viewModel.Key,
                           SettingValue = viewModel.Value,
                           Id = viewModel.Id,
                           SettingCaption = viewModel.Caption,
                           SettingType = settingType
                       };
        }

        public static SettingViewModel Map(Settings model)
        {
            if (model == null)
                return new SettingViewModel();

            return new SettingViewModel()
            {
                Key = model.SettingKey,
                Value = model.SettingValue,
                Id = model.Id,
                Caption = model.SettingCaption,
                Type = (int)model.SettingType
            };
        }

        public static List<SettingViewModel> MapList(List<Settings> model)
        {
            if (model == null)
                return new List<SettingViewModel>();

            return new List<SettingViewModel>(model.Select(Map));
        }

        public static List<Settings> MapList(List<SettingViewModel> model)
        {
            if (model == null)
                return new List<Settings>();

            return new List<Settings>(model.Select(Map));
        }
    }
}