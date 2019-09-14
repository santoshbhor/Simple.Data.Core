using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProfilingApp
{
    using ProfilingApp.Resources;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    class Program
    {
        static void Main(string[] args)
        {
            //ResetDatabase();

            new CastTask().Run();
            if (Debugger.IsAttached) Console.ReadKey();
        }

        private static void ResetDatabase()
        {
            using (var cn = new SqlConnection(Settings.Default.ConnectionString))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    foreach (
                        var sql in Regex.Split(Resources.Resources.DatabaseResetSql, @"^\s*GO\s*$", RegexOptions.Multiline))
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
