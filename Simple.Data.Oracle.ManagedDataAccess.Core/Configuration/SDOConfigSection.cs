using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal class SdoConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("schemas")]
        public SchemaCollection Schemas
        {
            get { return (SchemaCollection) this["schemas"]; }
            set { this["schemas"] = value; }
        }
    }
}