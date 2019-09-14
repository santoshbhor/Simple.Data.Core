namespace Simple.Data.Oracle
{
    internal class OracleSchemaProvider : SchemaProviderBase
    {
        private readonly SqlReflection _sqlReflection;

        public OracleSchemaProvider(OracleConnectionProvider connectionProvider)
        {
            _sqlReflection = new SqlReflection(connectionProvider);
        }

        protected override ISchemaReflector Reflector
        {
            get { return _sqlReflection; }
        }
    }
}