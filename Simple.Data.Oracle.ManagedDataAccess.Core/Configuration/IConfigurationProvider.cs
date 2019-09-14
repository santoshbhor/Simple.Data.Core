using Simple.Data.Ado.Schema;

namespace Simple.Data.Oracle.Configuration
{
    internal interface IConfigurationProvider
    {
        string ConnnectionSchemaOverride { get; } 
        ISchemaProvider SchemaProvider { get; }
    }
}