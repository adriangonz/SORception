using ScrapWeb.Entities;
using ScrapWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScrapWeb.Controllers
{
    public class SettingsController : ApiController
    {
        private SettingsService settingsService;

        SettingsController()
        {
            settingsService = new SettingsService();
        }

        // GET api/<controller>
        public SettingsEntity Get()
        {
            return settingsService.getExtendedSettings();
        }
    }
}