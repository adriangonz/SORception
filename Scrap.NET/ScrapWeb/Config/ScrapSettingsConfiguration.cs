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

        public string Name
        {
            get
            {
                return _name.Text;
            }
        }

        public string WebserviceURL
        {
            get
            {
                return _webserviceURL.Text;
            }
        }

        public string ActiveMQURL
        {
            get
            {
                return _activeMQURL.Text;
            }
        }

        [ConfigurationProperty("name", IsRequired=true)]
        public TextElement _name
        {
            get { return (TextElement)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("webservice", IsRequired = true)]
        public TextElement _webserviceURL
        {
            get { return (TextElement)base["webservice"]; }
            set { base["webservice"] = value; }
        }

        [ConfigurationProperty("activemq", IsRequired = true)]
        public TextElement _activeMQURL
        {
            get { return (TextElement)base["activemq"]; }
            set { base["activemq"] = value; }
        }

        public class TextElement : ConfigurationElement
        {
            [ConfigurationProperty("value", IsRequired=true)]
            public string Text
            {
                get { return (string)base["value"]; }
                set { base["value"] = value; }
            }
        }
    }
}