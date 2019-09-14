using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal class ColumnCollection : NamedConfigurationElementCollection
    {
        protected override string ElementName
        {
            get
            {
                return "column";
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new ColumnElement();
        }
    }
}