using System.Configuration;

namespace Simple.Data.Oracle.Configuration
{
    internal abstract class NamedConfigurationElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrWhiteSpace(elementName) && elementName == ElementName;
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((NamedConfigurationElement) element).Name;
        }
    }
}