using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public static class SqliteUtilities
    {
        public static bool ExecuteCommand(string connectionString , string strCommand)
        {
            return ExecuteCommandAsync(connectionString, strCommand).GetAwaiter().GetResult();
        }

        public static async Task<bool> ExecuteCommandAsync(string connectionString, string strCommand)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqliteCommand command = new SqliteCommand(strCommand, connection);
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception e)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return true;
        }
    }
}
