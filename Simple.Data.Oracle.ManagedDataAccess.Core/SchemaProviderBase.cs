using System;
using System.Collections.Generic;
using System.Linq;
using Simple.Data.Ado.Schema;

namespace Simple.Data.Oracle
{
    internal abstract class SchemaProviderBase : ISchemaProvider
    {
        protected abstract ISchemaReflector Reflector { get; }

        public virtual IEnumerable<Table> GetTables()
        {
            return Reflector.Tables;
        }

        public virtual IEnumerable<Column> GetColumns(Table table)
        {
            return Reflector.Columns
                .Where(c => table.ActualName.InvariantEquals(c.Item1))
                .Select(c => new Column(c.Item2, table, false, c.Item3, c.Item4));
        }

        public virtual Key GetPrimaryKey(Table table)
        {
            return new Key(Reflector.PrimaryKeys.Where(t => t.Item1.InvariantEquals(table.ActualName)).Select(t => t.Item2));
        }

        public virtual IEnumerable<ForeignKey> GetForeignKeys(Table table)
        {
            return Reflector.ForeignKeys.Where(fk => fk.DetailTable.Name.InvariantEquals(table.ActualName));
        }

        public virtual IEnumerable<Procedure> GetStoredProcedures()
        {
            return Reflector.Procedures;
        }

        public virtual IEnumerable<Parameter> GetParameters(Procedure storedProcedure)
        {
            var parameters = Reflector.ProcedureArguments
                .Where(p => p.Item1.InvariantEquals(storedProcedure.Name) && p.Item5.InvariantEquals(storedProcedure.Schema))
                .Select(p => new Parameter(p.Item2, p.Item3, p.Item4));
            return parameters;
        }

        public string QuoteObjectName(string unquotedName)
        {
            return string.Format("\"{0}\"", unquotedName);
        }

        public string NameParameter(string baseName)
        {
            if (string.IsNullOrWhiteSpace(baseName))
                throw new ArgumentException("baseName is not set.");
            return (baseName.StartsWith(":")) ? baseName : ":" + baseName;
        }

        public string GetDefaultSchema()
        {
            return Reflector.Schema;
        }
    }
}