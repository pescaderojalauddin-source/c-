using System;
using System.Collections.Generic;
using AccountingApp.Models;
using Npgsql;

namespace AccountingApp.Repositories
{
    public class ОплатыRepository
    {
        public List<Оплата> GetAll()
        {
            var list = new List<Оплата>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT о.id,
                           о.""СчетId"",
                           с.""Номер"" AS ""НомерСчета"",
                           к.""Название"" AS ""Клиент"",
                           о.""Сумма"",
                           о.""Дата"",
                           о.""Статус""
                    FROM ""Оплаты"" о
                    LEFT JOIN ""Счета""    с ON о.""СчетId""  = с.id
                    LEFT JOIN ""Клиенты""  к ON с.""КлиентId"" = к.id
                    ORDER BY о.""Дата"" DESC, о.id DESC";

                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Оплата
                        {
                            Id          = reader.GetInt32(0),
                            СчетId      = reader.GetInt32(1),
                            НомерСчета  = reader.IsDBNull(2) ? "" : reader.GetValue(2).ToString(),
                            Клиент      = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            Сумма       = reader.GetDecimal(4),
                            Дата        = reader.GetDateTime(5),
                            Статус      = reader.IsDBNull(6) ? "" : reader.GetString(6)
                        });
                    }
                }
            }
            return list;
        }

        public void Add(int счетId, decimal сумма, DateTime дата)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();

                // 1) Получаем данные счёта
                decimal суммаСчета;
                DateTime датаСчета;
                ПолучитьСчет(conn, счетId, out суммаСчета, out датаСчета);

                // 2) Сумма уже оплаченного по этому счёту
                decimal ужеОплачено = ПолучитьСуммуОплат(conn, счетId);

                // 3) Статус новой оплаты
                decimal итого = ужеОплачено + сумма;
                string статус;
                if (итого >= суммаСчета)
                    статус = "Оплачено";
                else if ((дата - датаСчета).TotalDays > 20)
                    статус = "Просрочено";
                else
                    статус = "Частично";

                // 4) Вставка
                using (var cmd = new NpgsqlCommand(
                    @"INSERT INTO ""Оплаты"" (""СчетId"", ""Сумма"", ""Дата"", ""Статус"")
                      VALUES (@s, @sum, @d, @st)", conn))
                {
                    cmd.Parameters.AddWithValue("@s",   счетId);
                    cmd.Parameters.AddWithValue("@sum", сумма);
                    cmd.Parameters.AddWithValue("@d",   дата);
                    cmd.Parameters.AddWithValue("@st",  статус);
                    cmd.ExecuteNonQuery();
                }

                // 5) Пересчёт статуса самого счёта
                ОбновитьСтатусСчета(conn, счетId);
            }
        }

        public void Delete(int id)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();

                int счетId = 0;
                using (var cmd = new NpgsqlCommand(
                    @"SELECT ""СчетId"" FROM ""Оплаты"" WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    var obj = cmd.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value)
                        счетId = Convert.ToInt32(obj);
                }

                using (var cmd = new NpgsqlCommand(
                    @"DELETE FROM ""Оплаты"" WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                if (счетId > 0)
                    ОбновитьСтатусСчета(conn, счетId);
            }
        }

        // ==== вспомогательные методы ====

        private static void ПолучитьСчет(NpgsqlConnection conn, int счетId,
                                         out decimal сумма, out DateTime дата)
        {
            сумма = 0m;
            дата  = DateTime.Today;

            using (var cmd = new NpgsqlCommand(
                @"SELECT ""Сумма"", ""Дата"" FROM ""Счета"" WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", счетId);
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        сумма = rd.GetDecimal(0);
                        дата  = rd.GetDateTime(1);
                    }
                }
            }
        }

        private static decimal ПолучитьСуммуОплат(NpgsqlConnection conn, int счетId)
        {
            using (var cmd = new NpgsqlCommand(
                @"SELECT COALESCE(SUM(""Сумма""), 0) FROM ""Оплаты"" WHERE ""СчетId"" = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", счетId);
                var obj = cmd.ExecuteScalar();
                return (obj == null || obj == DBNull.Value) ? 0m : Convert.ToDecimal(obj);
            }
        }

        private static void ОбновитьСтатусСчета(NpgsqlConnection conn, int счетId)
        {
            decimal суммаСчета;
            DateTime датаСчета;
            ПолучитьСчет(conn, счетId, out суммаСчета, out датаСчета);

            decimal оплачено = ПолучитьСуммуОплат(conn, счетId);

            string статус;
            if (суммаСчета > 0 && оплачено >= суммаСчета)
                статус = "Оплачен";
            else if (оплачено > 0 && (DateTime.Today - датаСчета).TotalDays > 20)
                статус = "Просрочен";
            else if (оплачено > 0)
                статус = "Частично";
            else if ((DateTime.Today - датаСчета).TotalDays > 20)
                статус = "Просрочен";
            else
                статус = "Черновик";

            using (var cmd = new NpgsqlCommand(
                @"UPDATE ""Счета"" SET ""Статус"" = @st WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@st", статус);
                cmd.Parameters.AddWithValue("@id", счетId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
