using ScrapWeb.Config;
using ScrapWeb.Entities;
using ScrapWeb.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrapWeb.Services
{
    public class SettingsService
    {
        private TokenService tokenService;

        public SettingsService()
        {
            tokenService = new TokenService();
        }

        public SettingsEntity getExtendedSettings()
        {
            var settingsEntity = this.getSimpleSettings();
            try
            {
                settingsEntity.validToken = tokenService.getValid();
            }
            catch (ServiceException ex)
            {
                settingsEntity.validToken = null;
            }
            settingsEntity.tokenList = tokenService.getAll();
            return settingsEntity;
        }

        public SettingsEntity getSimpleSettings() 
        {
            var scrapSettings = ScrapSettingsConfiguration.Instance;
            return new SettingsEntity
            {
                name = scrapSettings.Name
            };
        }

    }
}