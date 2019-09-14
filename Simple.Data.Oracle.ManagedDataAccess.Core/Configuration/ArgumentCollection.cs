using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal class ArgumentCollection : NamedConfigurationElementCollection
    {
        protected override string ElementName
        {
            get
            {
                return "argument";
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new ArgumentElement();
        }
    }
}