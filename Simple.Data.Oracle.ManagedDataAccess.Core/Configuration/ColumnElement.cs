using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal class ColumnElement : NamedConfigurationElement
    {
        [ConfigurationProperty("dataType", IsRequired = true)]
        public virtual string DataType
        {
            get { return (string)this["dataType"]; }
            set { this["dataType"] = value; }
        }

        [ConfigurationProperty("length", DefaultValue = 0)]
        public virtual int Length
        {
            get { return (int)this["length"]; }
            set { this["length"] = value; }
        }

        [ConfigurationProperty("isPrimaryKey", DefaultValue = false)]
        public virtual bool IsPrimaryKey
        {
            get { return (bool)this["isPrimaryKey"]; }
            set { this["isPrimaryKey"] = value; }
        }

        [ConfigurationProperty("foreignKey", DefaultValue = null)]
        public virtual string ForeignKey
        {
            get { return (string)this["foreignKey"]; }
            set { this["foreignKey"] = value; }
        }
    }
}