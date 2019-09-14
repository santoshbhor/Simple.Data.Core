﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.SqlServer
{
    using System.ComponentModel.Composition;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Ado;

    [Export(typeof(IObservableQueryRunner))]
    public class SqlObservableQueryRunner : IObservableQueryRunner
    {
        public IObservable<IDictionary<string, object>> Run(IDbCommand command, IDbConnection connection, IDictionary<string, int> index)
        {
            return new SqlObservable(connection as SqlConnection, command as SqlCommand, index);
        }

        class SqlObservable : IObservable<IDictionary<string,object>>
        {
            private readonly SqlConnection _connection;
            private readonly SqlCommand _command;
            private IDictionary<string, int> _index;

            public SqlObservable(SqlConnection connection, SqlCommand command, IDictionary<string,int> index)
            {
                if (connection == null) throw new ArgumentNullException("connection");
                if (command == null) throw new ArgumentNullException("command");
                _connection = connection;
                _command = command;
                _index = index;
            }

            public async Task<IDisposable> SubscribeAsync(IObserver<IDictionary<string, object>> observer)
            {
                try
                {
                    if (_connection.State == ConnectionState.Closed)
                    {
                        await _connection.OpenAsync();
                    }

                    using (var reader = await _command.ExecuteReaderAsync())
                    {
                        if (_index == null) _index = reader.CreateDictionaryIndex();
                        while (await reader.ReadAsync())
                        {
                            observer.OnNext(reader.ToDictionary(_index));
                        }
                    }

                    observer.OnCompleted();
                    
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }

                return new ActionDisposable(() =>
                {
                    using (_connection) using (_command) { }
                });

            }

            public IDisposable Subscribe(IObserver<IDictionary<string, object>> observer)
            {
                return SubscribeAsync(observer);
            }

            //Older methods that support BeginExecuteReader migrated to use newr ExecuteReaderAsync
            //public IDisposable Subscribe(IObserver<IDictionary<string, object>> observer)
            //{
            //    if (_connection.State == ConnectionState.Closed)
            //    {
            //        _connection.Open();
            //    }

            //    _command.BeginExecuteReader(ExecuteReaderCompleted, observer);

            //    var reader = _command.ExecuteReaderAsync();

            //    return new ActionDisposable(() =>
            //                                    {
            //                                        using (_connection) using (_command) { }
            //                                    });
            //}

            //private void ExecuteReaderCompleted(IAsyncResult ar)
            //{
            //    var observer = ar.AsyncState as IObserver<IDictionary<string, object>>;
            //    if (observer == null) throw new InvalidOperationException();
            //    try
            //    {
            //        using (var reader = _command.EndExecuteReader(ar))
            //        {
            //            if (_index == null) _index = reader.CreateDictionaryIndex();
            //            while (reader.Read())
            //            {
            //                observer.OnNext(reader.ToDictionary(_index));
            //            }
            //        }
            //        observer.OnCompleted();
            //    }
            //    catch (Exception ex)
            //    {
            //        observer.OnError(ex);
            //    }
            //}
        }
    }
}
