namespace Simple.Data.Ado
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    class DataReaderMultipleEnumerator : IEnumerator<IEnumerable<IDictionary<string, object>>>
    {
        private readonly IDisposable _connectionDisposable;
        private readonly IDbConnection _connection;
        private IDictionary<string, int> _index;
        private readonly IDbCommand _command;
        private IDataReader _reader;
        private bool _lastRead;

        public DataReaderMultipleEnumerator(IDbCommand command, IDbConnection connection) : this(command, connection, null)
        {
        }

        public DataReaderMultipleEnumerator(IDbCommand command, IDbConnection connection, IDictionary<string, int> index)
        {
            _command = command;
            _connection = connection;
            _connectionDisposable = _connection.MaybeDisposable();
            _index = index;
        }

        public void Dispose()
        {
            using (_connectionDisposable)
            using (_command)
            using (_reader)
            {
                /* NO-OP */
            }
        }

        public bool MoveNext()
        {
            if (_reader == null)
            {
                ExecuteReader();
                _lastRead = true;
                return true;
            }
            _lastRead = _reader.NextResult();
            return _lastRead;
        }

        private void ExecuteReader()
        {
            _connection.OpenIfClosed();
            _reader = _command.TryExecuteReader();
            _index = _index ?? _reader.CreateDictionaryIndex();
        }

        public void Reset()
        {
            if (_reader != null) _reader.Dispose();
            ExecuteReader();
        }

        public IEnumerable<IDictionary<string, object>> Current
        {
            get
            {
                if (!_lastRead) throw new InvalidOperationException();
                var index = _reader.CreateDictionaryIndex();
                while (_reader.Read())
                {
                    yield return _reader.ToDictionary(index);
                }
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}