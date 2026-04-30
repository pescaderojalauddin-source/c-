using System;
using System.Collections.Generic;
using AccountingApp.Models;
using Npgsql;

namespace AccountingApp.Repositories
{
    /// <summary>
    /// Доступ к таблице oplata. Триггер БД tg_oplata_recalc сам
    /// пересчитывает prodaja.oplacheno при INSERT/UPDATE/DELETE.
    /// Статус оплаты вычисляется на лету.
    /// </summary>
    public class ОплатыRepository
    {
        public List<Оплата> GetAll()
        {
            var list = new List<Оплата>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT o.id,
                           o.idprodaji,
                           ('Продажа #' || p.id || ' от ' || to_char(p.data, 'DD.MM.YYYY')) AS ""НомерСчета"",
                           COALESCE(k.""Название"", '')                                     AS ""Клиент"",
                           o.sum,
                           o.data,
                           CASE
                               WHEN p.oplacheno >= p.totalsum THEN 'Оплачено'
                               WHEN (CURRENT_DATE - p.data) > 20 THEN 'Просрочено'
                               WHEN p.oplacheno > 0 THEN 'Частично'
                               ELSE 'Не оплачено'
                           END AS ""Статус""
                    FROM ""oplata"" o
                    LEFT JOIN ""prodaja"" p ON o.idprodaji = p.id
                    LEFT JOIN ""Клиенты"" k ON p.idclient = k.id
                    ORDER BY o.data DESC, o.id DESC";

                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Оплата
                        {
                            Id          = reader.GetInt32(0),
                            СчетId      = reader.GetInt32(1),
                            НомерСчета  = reader.IsDBNull(2) ? "" : reader.GetString(2),
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

        public List<Оплата> GetByProdaja(int idProdaji)
        {
            var list = new List<Оплата>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT o.id, o.idprodaji, o.sum, o.data
                    FROM ""oplata"" o
                    WHERE o.idprodaji = @id
                    ORDER BY o.data, o.id";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idProdaji);
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new Оплата
                            {
                                Id      = rd.GetInt32(0),
                                СчетId  = rd.GetInt32(1),
                                Сумма   = rd.GetDecimal(2),
                                Дата    = rd.GetDateTime(3)
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void Add(int idProdaji, decimal сумма, DateTime дата)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    @"INSERT INTO ""oplata"" (idprodaji, sum, data) VALUES (@p, @s, @d)", conn))
                {
                    cmd.Parameters.AddWithValue("@p", idProdaji);
                    cmd.Parameters.AddWithValue("@s", сумма);
                    cmd.Parameters.AddWithValue("@d", дата);
                    cmd.ExecuteNonQuery();
                }
                // prodaja.oplacheno пересчитается триггером
            }
        }

        public void Delete(int id)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    @"DELETE FROM ""oplata"" WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                // prodaja.oplacheno пересчитается триггером
            }
        }
    }
}
