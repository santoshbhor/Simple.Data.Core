using System;
using System.Collections.Generic;
using System.Data;
using Simple.Data.Ado.Schema;

namespace Simple.Data.Oracle
{
    internal interface ISchemaReflector
    {
        List<Tuple<string, string>> PrimaryKeys { get; }
        IList<Table> Tables { get; }
        IList<Tuple<string, string, DbType, int>> Columns { get; }
        IList<ForeignKey> ForeignKeys { get; }
        IList<Procedure> Procedures { get; }
        List<Tuple<string, string, Type, ParameterDirection, string>> ProcedureArguments { get; }
        string Schema { get; }
    }
}