using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using ThreeTrunks.Data.Models;
using ThreeTrunks.Data.Repositories;
using ThreeTrunks.Logic.Managers;
using ThreeTrunks.UI.Helpers;
using ThreeTrunks.UI.ViewModels;

namespace ThreeTrunks.UI.ApiControllers
{
    [Authorize]
    public class SettingsController : ApiController
    {
        readonly UnitOfWork _unitOfWork = new UnitOfWork();

        private readonly SettingsManager _settingsManager;

        public SettingsController()
        {
            _settingsManager = new SettingsManager(_unitOfWork);
        }

        [HttpGet]
        [ActionName("GetSiteSettings")]
        public HttpResponseMessage GetSiteSettings()
        {
            var settings = _settingsManager.GetSettings();

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(SettingViewModel.MapList(settings)), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpPost] 
        [ActionName("SaveSiteSettings")]
        public HttpResponseMessage SaveSiteSettings([FromBody]List<SettingViewModel> settings)
        {
            var result = new List<Settings>();

            foreach (var settingViewModel in settings)
            {
                result.Add(_settingsManager.SetValue(SettingViewModel.Map(settingViewModel)));
            }
           
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(SettingViewModel.MapList(result)), Encoding.UTF8, "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("GetSettingValue")]
        public HttpResponseMessage GetSettingValue([FromBody]string key)
        {
            var setting = _settingsManager.GetSetting(key);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(SettingViewModel.Map(setting)), Encoding.UTF8, "application/json");
            return response;
        }

        [HttpPost]
        [ActionName("SaveSetting")]
        public HttpResponseMessage SaveImage([FromBody]SettingViewModel setting)
        {
            var result = _settingsManager.SetValue(SettingViewModel.Map(setting));

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(SettingViewModel.Map(result)), Encoding.UTF8, "application/json");

            return response;
        }

    }
}