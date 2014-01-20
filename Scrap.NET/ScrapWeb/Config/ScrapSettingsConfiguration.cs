using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ScrapWeb.Config
{
    public class ScrapSettingsConfiguration : ConfigurationSection
    {
        private static ScrapSettingsConfiguration instance = null;
        public static ScrapSettingsConfiguration Instance
        {
            get
            {
                if (instance == null) 
                {
                    instance = (ScrapSettingsConfiguration)WebConfigurationManager
                        .GetSection("scrapSettings");
                }
                return instance;
            }
        }

        [ConfigurationProperty("name", IsRequired=true)]
        public NameElement Name
        {
            get { return (NameElement)base["name"]; }
            set { base["name"] = value; }
        }

        public class NameElement : ConfigurationElement
        {
            [ConfigurationProperty("value", IsRequired=true)]
            public string Name
            {
                get { return (string)base["value"]; }
                set { base["value"] = value; }
            }
        }
    }
}