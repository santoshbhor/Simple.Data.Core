﻿using System.Data;
using System.Data.Common;

namespace Simple.Data.Ado
{
    /// <summary>
    /// Provides a base class for implementing <see cref="IDbConnection"/> which delegates all the interface methods
    /// to a wrapped instance of <see cref="IDbConnection"/>.
    /// Also implements the <see cref="DbConnection.GetSchema(string,string[])"/> method which is on <see cref="DbConnection"/> but not <see cref="IDbConnection"/>.
    /// </summary>
    public abstract class DelegatingConnectionBase : IDbConnection, ISchemaGetter
    {
        private readonly IDbConnection _delegatedConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegatingConnectionBase"/> class.
        /// </summary>
        /// <param name="delegatedConnection">The connection to which method calls are delegated by default.</param>
        protected DelegatingConnectionBase(IDbConnection delegatedConnection)
        {
            _delegatedConnection = delegatedConnection;
        }

        protected IDbConnection DelegatedConnection
        {
            get { return _delegatedConnection; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public virtual void Dispose()
        {
            _delegatedConnection.Dispose();
        }

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        /// <returns>
        /// An object representing the new transaction.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public virtual IDbTransaction BeginTransaction()
        {
            return _delegatedConnection.BeginTransaction();
        }

        /// <summary>
        /// Begins a database transaction with the specified <see cref="T:System.Data.IsolationLevel"/> value.
        /// </summary>
        /// <returns>
        /// An object representing the new transaction.
        /// </returns>
        /// <param name="il">One of the <see cref="T:System.Data.IsolationLevel"/> values. </param><filterpriority>2</filterpriority>
        public virtual IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _delegatedConnection.BeginTransaction(il);
        }

        /// <summary>
        /// Closes the connection to the database.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public virtual void Close()
        {
            _delegatedConnection.Close();
        }

        /// <summary>
        /// Changes the current database for an open Connection object.
        /// </summary>
        /// <param name="databaseName">The name of the database to use in place of the current database. </param><filterpriority>2</filterpriority>
        public virtual void ChangeDatabase(string databaseName)
        {
            _delegatedConnection.ChangeDatabase(databaseName);
        }

        /// <summary>
        /// Creates and returns a Command object associated with the connection.
        /// </summary>
        /// <returns>
        /// A Command object associated with the connection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public virtual IDbCommand CreateCommand()
        {
            return _delegatedConnection.CreateCommand();
        }

        /// <summary>
        /// Opens a database connection with the settings specified by the ConnectionString property of the provider-specific Connection object.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public virtual void Open()
        {
            _delegatedConnection.OpenIfClosed();
        }

        /// <summary>
        /// Gets or sets the string used to open a database.
        /// </summary>
        /// <returns>
        /// A string containing connection settings.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public virtual string ConnectionString
        {
            get { return _delegatedConnection.ConnectionString; }
            set { _delegatedConnection.ConnectionString = value; }
        }

        /// <summary>
        /// Gets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        /// <returns>
        /// The time (in seconds) to wait for a connection to open. The default value is 15 seconds.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public virtual int ConnectionTimeout
        {
            get { return _delegatedConnection.ConnectionTimeout; }
        }

        /// <summary>
        /// Gets the name of the current database or the database to be used after a connection is opened.
        /// </summary>
        /// <returns>
        /// The name of the current database or the name of the database to be used once a connection is open. The default value is an empty string.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public virtual string Database
        {
            get { return _delegatedConnection.Database; }
        }

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Data.ConnectionState"/> values.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public virtual ConnectionState State
        {
            get { return _delegatedConnection.State; }
        }

        public virtual DataTable GetSchema(string collectionName, params string[] constraints)
        {
            return _delegatedConnection.GetSchema(collectionName, constraints);
        }
    }
}
