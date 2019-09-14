using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal class SchemaCollection : NamedConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "schema"; }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new SchemaElement();
        }
    }
}