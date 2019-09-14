using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Simple.Data.Ado.Schema;

namespace Simple.Data.Oracle.Configuration
{
    internal class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfigurationManager _configurationManager;

        public ConfigurationProvider(IConfigurationManager configurationManager = null)
        {
            _configurationManager = configurationManager ?? new AppConfigConfigurationManager();
        }
        public string ConnnectionSchemaOverride
        {
            get { return GetAppSettingsValue("Simple.Data.Oracle.Schema"); }
        }

        private string GetAppSettingsValue(string name)
        {
            if(!_configurationManager.AppSettings.AllKeys.Contains(name) || string.IsNullOrWhiteSpace(_configurationManager.AppSettings[name]))
            {
                return null;
            }
            return _configurationManager.AppSettings[name];
        }

        public ISchemaProvider SchemaProvider
        {
            get 
            { 
                var sectionName = GetAppSettingsValue("Simple.Data.Oracle.ConfigSectionName") ?? "SimpleDataOracleConfig";
                var section = _configurationManager.GetSection(sectionName) as SdoConfigSection;
                return section == null ? null : new ConfigSchemaProvider(section);
            }
        }

        private class AppConfigConfigurationManager : IConfigurationManager
        {
            public NameValueCollection AppSettings { get { return ConfigurationManager.AppSettings; } }
            public object GetSection(string sectionName)
            {
                return ConfigurationManager.GetSection(sectionName);
            }
        }
    }

}