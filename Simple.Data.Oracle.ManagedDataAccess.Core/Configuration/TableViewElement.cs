using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal class TableViewElement : NamedConfigurationElement
    {
        [ConfigurationProperty("isView", DefaultValue = false)]
        public virtual bool IsView
        {
            get { return (bool)this["isView"]; }
            set { this["isView"] = value; }
        }

        [ConfigurationProperty("columns")]
        public ColumnCollection Columns
        {
            get { return (ColumnCollection)this["columns"]; }
            set { this["columns"] = value; }
        }
    }
}