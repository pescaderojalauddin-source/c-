using System;
using Npgsql;

namespace AccountingApp
{
    public static class DbConnectionHelper
    {
        // Строка подключения к твоему PostgreSQL в Docker
        private static string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres";

        // Возвращает новое открытое соединение (не забудь закрыть!)
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        // Проверка, работает ли подключение
        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Можно показать ex.Message для отладки
                return false;
            }
        }
    }
}