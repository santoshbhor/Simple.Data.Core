using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal class ProcedureCollection : NamedConfigurationElementCollection
    {
        protected override string ElementName
        {
            get
            {
                return "procedure";
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProcedureElement();
        }
    }
}