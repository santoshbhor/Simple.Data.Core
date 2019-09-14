using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    public class NamedConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public virtual string Name
        {
            get { return (string) this["name"];  }
            set { this["name"] = value; }
        }
    }
}