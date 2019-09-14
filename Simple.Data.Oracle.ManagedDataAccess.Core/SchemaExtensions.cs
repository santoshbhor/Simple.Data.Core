using Simple.Data.Ado.Schema;
using Simple.Data.Oracle.Configuration;

namespace Simple.Data.Oracle
{
    internal static class SchemaExtensions
    {
        public static string GetNameWithSchema(this Table table)
        {
            return (string.IsNullOrWhiteSpace(table.Schema) ? string.Empty : table.Schema + ".") + table.ActualName; 
        }

        public static string GetProcedureName(this ProcedureElement element)
        {
            return (string.IsNullOrWhiteSpace(element.PackageName) ? string.Empty : element.PackageName + "__") + element.Name;
        }
    }
}