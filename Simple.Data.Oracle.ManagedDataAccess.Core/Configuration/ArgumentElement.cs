using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal class ArgumentElement : NamedConfigurationElement
    {
        [ConfigurationProperty("dataType", IsRequired = true)]
        public virtual string DataType
        {
            get { return (string)this["dataType"]; }
            set { this["dataType"] = value; }
        }

        [ConfigurationProperty("direction", IsRequired = true)]
        public virtual string Direction
        {
            get { return (string)this["direction"]; }
            set { this["direction"] = value; }
        }
    }
}