using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal class ProcedureElement : NamedConfigurationElement
    {
        [ConfigurationProperty("packageName", DefaultValue = null)]
        public virtual string PackageName
        {
            get { return (string)this["packageName"]; }
            set { this["packageName"] = value; }
        }

        [ConfigurationProperty("arguments")]
        public ArgumentCollection Arguments
        {
            get { return (ArgumentCollection)this["arguments"]; }
            set { this["arguments"] = value; }
        }
    }
}