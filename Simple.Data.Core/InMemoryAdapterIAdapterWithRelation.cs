﻿namespace Simple.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public partial class InMemoryAdapter : IAdapterWithRelation
    {
        public bool IsValidRelation(string tableName, string relatedTableName)
        {
            return _joins.Any(
                ji =>
                _nameComparer.Equals(tableName, ji.MasterTableName) &&
                _nameComparer.Equals(relatedTableName, ji.MasterPropertyName))
                   ||
                   _joins.Any(
                       ji =>
                       _nameComparer.Equals(tableName, ji.DetailTableName) &&
                       _nameComparer.Equals(relatedTableName, ji.DetailPropertyName));
        }

        public object FindRelated(string tableName, IDictionary<string, object> row, string relatedTableName)
        {
            return FindMaster(tableName, row, relatedTableName) ?? FindDetail(tableName, row, relatedTableName);
        }

        private object FindMaster(string tableName, IDictionary<string, object> row, string relatedTableName)
        {
            var master = _joins.FirstOrDefault(ji =>
                                               _nameComparer.Equals(tableName, ji.MasterTableName) &&
                                               _nameComparer.Equals(relatedTableName, ji.MasterPropertyName));
            if (master != null)
            {
                object result;
                row.TryGetValue(master.DetailPropertyName, out result);
                return result;
            }
            return null;
        }

        private object FindDetail(string tableName, IDictionary<string, object> row, string relatedTableName)
        {
            var detail = _joins.FirstOrDefault(ji =>
                                               _nameComparer.Equals(tableName, ji.DetailTableName) &&
                                               _nameComparer.Equals(relatedTableName, ji.DetailPropertyName));
            if (detail != null)
            {
                object result;
                row.TryGetValue(detail.MasterPropertyName, out result);
                return result;
            }
            return null;
        }
    }
}