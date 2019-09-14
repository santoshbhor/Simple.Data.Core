﻿using System;

namespace Simple.Data
{
    public partial class Database
    {
        /// <summary>
        /// Opens an instance of <see cref="Database"/> which connects to an ADO.NET data source
        /// specified in the 'Simple.Data.Properties.Settings.ConnectionString' config ConnectionStrings setting.
        /// </summary>
        /// <returns>A <see cref="Database"/> object as a dynamic type.</returns>
        public static dynamic Open(string schemaName = null)
        {
            return DatabaseOpener.OpenDefault(schemaName);
        }

        /// <summary>
        /// Opens an instance of <see cref="Database"/> which connects to an ADO.NET data source
        /// specified in the connectionString parameter.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>A <see cref="Database"/> object as a dynamic type.</returns>
        public static dynamic OpenConnection(string connectionString)
        {
            return DatabaseOpener.OpenConnection(connectionString);
        }

        /// <summary>
        /// Opens an instance of <see cref="Database"/> which connects to the SQL CE database
        /// specified in the filename parameter.
        /// </summary>
        /// <param name="filename">The name of the SQL CE database file.</param>
        /// <returns>A <see cref="Database"/> object as a dynamic type.</returns>
        public static dynamic OpenFile(string filename)
        {
            return DatabaseOpener.OpenFile(filename);
        }
        
        public static dynamic OpenNamedConnection(string connectionName, string schemaName = null)
        {
            return DatabaseOpener.OpenNamedConnection(connectionName, schemaName);
        }

        public static dynamic OpenConnection(string connectionString,string providerName)
        {
            return DatabaseOpener.OpenConnection(connectionString, providerName);
        }

        public static dynamic OpenConnection(string connectionString, string providerName, string schemaName)
        {
            return DatabaseOpener.OpenConnection(connectionString, providerName, schemaName);
        }
    }
}