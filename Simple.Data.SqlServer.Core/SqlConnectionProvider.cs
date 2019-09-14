﻿using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

namespace Simple.Data.SqlServer
{
    [Export(typeof(IConnectionProvider))]
    [Export("System.Data.SqlClient", typeof(IConnectionProvider))]
    public class SqlConnectionProvider : IConnectionProvider
    {
        private string _connectionString;

        public SqlConnectionProvider()
        {
            
        }

        public SqlConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public ISchemaProvider GetSchemaProvider()
        {
            return new SqlSchemaProvider(this);
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public string GetIdentityFunction()
        {
            return "SCOPE_IDENTITY()";
        }

        public bool TryGetNewRowSelect(Table table, ref string insertSql, out string selectSql)
        {
            var identityColumn = table.Columns.FirstOrDefault(col => col.IsIdentity);

            if (identityColumn == null)
            {
                selectSql = null;
                return false;
            }

            selectSql = "select * from " + table.QualifiedName + " where " + identityColumn.QuotedName +
                        " = SCOPE_IDENTITY()";
            return true;
        }

        public bool SupportsCompoundStatements
        {
            get { return true; }
        }

        public bool SupportsStoredProcedures
        {
            get { return true; }
        }

        public IProcedureExecutor GetProcedureExecutor(AdoAdapter adapter, ObjectName procedureName)
        {
            return new ProcedureExecutor(adapter, procedureName);
        }
    }
}
