using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal class SchemaElement : NamedConfigurationElement
    {
        [ConfigurationProperty("isDefault", DefaultValue = false)]
        public virtual bool IsDefault
        {
            get { return (bool)this["isDefault"]; }
            set { this["isDefault"] = value; }
        }

        [ConfigurationProperty("tableViews", DefaultValue = null)]
        public TableViewCollection TableViews
        {
            get { return (TableViewCollection)this["tableViews"]; }
            set { this["tableViews"] = value; }
        }

        [ConfigurationProperty("procedures", DefaultValue = null)]
        public ProcedureCollection Procedures
        {
            get { return (ProcedureCollection)this["procedures"]; }
            set { this["procedures"] = value; }
        }
    }
}