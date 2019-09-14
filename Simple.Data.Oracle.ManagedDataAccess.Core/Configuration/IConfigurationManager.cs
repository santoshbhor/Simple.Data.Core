using System.Collections.Specialized;

namespace Simple.Data.Oracle.Configuration
{
    internal interface IConfigurationManager
    {
        NameValueCollection AppSettings { get; }
        object GetSection(string sectionName);
    }
}