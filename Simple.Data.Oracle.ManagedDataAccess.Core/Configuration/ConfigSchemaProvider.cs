using System.Collections.Generic;
using System.Linq;
using Simple.Data.Ado.Schema;

namespace Simple.Data.Oracle.Configuration
{
    internal class ConfigSchemaProvider : SchemaProviderBase
    {
        private readonly ConfigReflector _reflector;
        public ConfigSchemaProvider(SdoConfigSection configSection)
        {
            _reflector = new ConfigReflector(configSection);
        }

        protected override ISchemaReflector Reflector
        {
            get { return _reflector; }
        }

        public override IEnumerable<Column> GetColumns(Table table)
        {
            return Reflector.Columns
                .Where(c => table.GetNameWithSchema().InvariantEquals(c.Item1))
                .Select(c => new Column(c.Item2, table, false, c.Item3, c.Item4));
        }

        public override Key GetPrimaryKey(Table table)
        {
            return new Key(Reflector.PrimaryKeys.Where(t => t.Item1.InvariantEquals(table.GetNameWithSchema())).Select(t => t.Item2));
        }

        public override IEnumerable<ForeignKey> GetForeignKeys(Table table)
        {
            return Reflector.ForeignKeys.Where(fk => fk.DetailTable.ToString().InvariantEquals(table.GetNameWithSchema()));
        }

    }
}