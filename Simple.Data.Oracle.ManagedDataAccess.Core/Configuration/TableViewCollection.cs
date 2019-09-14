using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal class TableViewCollection : NamedConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "tableView"; }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new TableViewElement();
        }
    }
}