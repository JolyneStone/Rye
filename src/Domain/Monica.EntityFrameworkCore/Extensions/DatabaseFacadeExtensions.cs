using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Monica.DataAccess;
using System.Threading.Tasks;

namespace Monica.EntityFrameworkCore
{
    public static class DatabaseFacadeExtensions
    {
        public static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection connection, params object[] parameters)
        {
            var conn = facade.GetDbConnection();
            connection = conn;
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddRange(parameters);
            return cmd;
        }

        public static DataTable SqlQuery(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            var reader = command.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }

        public static List<T> SqlQuery<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class, new()
        {
            var dt = SqlQuery(facade, sql, parameters);
            return dt.ToList<T>();
        }

        public static IQueryable<T> SqlQueryToQueryable<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class, new()
        {
            var dt = SqlQuery(facade, sql, parameters);
            return dt.ToList<T>().AsQueryable();
        }

        public static async Task<DataTable> SqlQueryAsync(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            var reader = await command.ExecuteReaderAsync();
            var dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }

        public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class, new()
        {
            var dt = await SqlQueryAsync(facade, sql, parameters);
            return dt.ToList<T>();
        }
        public static async Task<IQueryable<T>> SqlQueryToQueryableAsync<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class, new()
        {
            var dt = await SqlQueryAsync(facade, sql, parameters);
            return dt.ToList<T>().AsQueryable();
        }
    }
}
