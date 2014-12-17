using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThreeTrunks.Data.Repositories;
using ThreeTrunks.Logic.Managers;

namespace ThreeTrunks.UI.Attributes
{
    public class IsPublicAttribute : AuthorizeAttribute
    {
        // Custom property
        private string _notifyUrl = "/Error/Unavailable";
        private string _keyToCheck = "IsPublic";

        public string KeyToCheck
        {
            get { return _keyToCheck; }
            set { _keyToCheck = value; }
        }

        public string NotifyUrl
        {
            get { return _notifyUrl; }
            set { _notifyUrl = value; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (AuthorizeCore(filterContext.HttpContext))
            {
                HttpCachePolicyBase cachePolicy =
                    filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, null);
            }
            else
            {
                if (NotifyUrl != null)
                    filterContext.Result = new RedirectResult(NotifyUrl);
            }

            //HandleUnauthorizedRequest(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (isAuthorized && httpContext.User.IsInRole("Admin"))
            {
                return true;
            }

            else
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var settingsManager = new SettingsManager(unitOfWork);

                    var value = settingsManager.GetValue(KeyToCheck);
                    bool result;
                    if (Boolean.TryParse(value, out result))
                    {
                        return result;
                    }

                    return false;
                }
            }
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

    }
}