using System;
using System.Collections.Generic;
using System.Data;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

namespace Simple.Data.Oracle.Configuration
{
    internal class ConfigReflector : ISchemaReflector
    {
        public ConfigReflector(SdoConfigSection configSection)
        {
            Tables = new List<Table>();
            Columns = new List<Tuple<string, string, DbType, int>>();
            PrimaryKeys = new List<Tuple<string, string>>();
            ForeignKeys = new List<ForeignKey>();
            Procedures = new List<Procedure>();
            ProcedureArguments = new List<Tuple<string, string, Type, ParameterDirection, string>>();
            LoadConfigData(configSection);
        }

        private void LoadConfigData(SdoConfigSection configSection)
        {
            if (configSection == null)
                return;

            foreach(var schema in configSection.Schemas)
            {
                LoadSchema(schema as SchemaElement);
            }
        }

        private void LoadSchema(SchemaElement schemaElement)
        {
            if (schemaElement == null)
                return; 

            if(Schema == null || schemaElement.IsDefault)
            {
                Schema = schemaElement.Name;
            }
            foreach(var tableView in schemaElement.TableViews)
            {
                LoadTable(tableView as TableViewElement, schemaElement.Name);
            }
            Tables.Add(new Table("DUAL", null, TableType.Table));
            foreach(var procedure in schemaElement.Procedures)
            {
                LoadProcedure(procedure as ProcedureElement, schemaElement.Name);
            }
        }

        private void LoadProcedure(ProcedureElement procedureElement, string schemaName)
        {
            if (procedureElement == null)
                return;

            Procedures.Add(new Procedure(procedureElement.GetProcedureName(), procedureElement.GetProcedureName(), schemaName));
            foreach(var argumentElement in procedureElement.Arguments)
            {
                LoadArgument(argumentElement as ArgumentElement, procedureElement.GetProcedureName(), schemaName);
            }
        }

        private void LoadArgument(ArgumentElement argumentElement, string procedure, string schema)
        {
            if (argumentElement == null || argumentElement.DataType == null || argumentElement.Direction == null)
                return;

            ProcedureArguments.Add(new Tuple<string, string, Type, ParameterDirection, string>(procedure, argumentElement.Name, argumentElement.DataType.ToClrType(), argumentElement.Direction.ToParameterDirection(argumentElement.Name == "__ReturnValue"), schema));
        }

        private void LoadTable(TableViewElement tableViewElement, string schemaName)
        {
            if (tableViewElement == null)
                return;
            
            var table = new Table(tableViewElement.Name, schemaName, tableViewElement.IsView ? TableType.View : TableType.Table); 
            Tables.Add(table);
            foreach(var column in tableViewElement.Columns)
            {
                LoadColumn(column as ColumnElement, table);
            }
        }

        private void LoadColumn(ColumnElement columnElement, Table table)
        {
            if (columnElement == null)
                return;

            Columns.Add(new Tuple<string, string, DbType, int>(table.GetNameWithSchema(), columnElement.Name,DbTypeConverter.FromDataType(columnElement.DataType), columnElement.Length));
            if(columnElement.IsPrimaryKey)
            {
                PrimaryKeys.Add(new Tuple<string, string>(table.GetNameWithSchema(), columnElement.Name));
            }

            if(!string.IsNullOrWhiteSpace(columnElement.ForeignKey))
            {
                var parts = columnElement.ForeignKey.Split('.');
                var schemaName = parts.Length > 2 ? parts[0] : null;
                var objectName = parts.Length > 2 ? parts[1] : parts[0];
                var columnName = parts.Length > 2 ? parts[2] : parts[1];
                ForeignKeys.Add(new ForeignKey(new ObjectName(table.Schema, table.ActualName), new[] { columnElement.Name }, new ObjectName(schemaName, objectName), new[] { columnName }));
            }
        }
            


        public List<Tuple<string, string>> PrimaryKeys { get; private set; }
        public IList<Table> Tables { get; private set; }
        public IList<Tuple<string, string, DbType, int>> Columns { get; private set; }
        public IList<ForeignKey> ForeignKeys { get; private set; }
        public IList<Procedure> Procedures { get; private set; }
        public List<Tuple<string, string, Type, ParameterDirection, string>> ProcedureArguments { get; private set; }
        public string Schema { get; private set; }
    }
}