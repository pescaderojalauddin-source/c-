using System;
using System.Collections.Generic;
using AccountingApp.Models;
using Npgsql;

namespace AccountingApp.Repositories
{
    /// <summary>
    /// Чтение продаж (prodaja) и их позиций (prodaja_info) для UI.
    /// Запись и редактирование выполняются прямо в FormСчета через ad-hoc SQL,
    /// этот класс используется главным образом в FormОплатыКлиента.
    /// </summary>
    public class ПродажиRepository
    {
        public List<Прода> GetByКлиент(int idClient)
        {
            var list = new List<Прода>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT p.id, p.idclient, k.""Название"" AS ""Клиент"",
                           p.data, p.totalsum, p.oplacheno,
                           CASE
                               WHEN p.oplacheno >= p.totalsum AND p.totalsum > 0 THEN 'Оплачен'
                               WHEN (CURRENT_DATE - p.data) > 20 AND p.oplacheno < p.totalsum THEN 'Просрочен'
                               WHEN p.oplacheno > 0 THEN 'Частично'
                               ELSE 'Не оплачен'
                           END AS ""Статус""
                    FROM ""prodaja"" p
                    LEFT JOIN ""Клиенты"" k ON k.id = p.idclient
                    WHERE p.idclient = @id
                    ORDER BY p.data DESC, p.id DESC";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idClient);
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new Прода
                            {
                                Id         = rd.GetInt32(0),
                                КлиентId   = rd.IsDBNull(1) ? (int?)null : rd.GetInt32(1),
                                Клиент     = rd.IsDBNull(2) ? "" : rd.GetString(2),
                                Дата       = rd.GetDateTime(3),
                                Сумма      = rd.GetDecimal(4),
                                Оплачено   = rd.GetDecimal(5),
                                Статус     = rd.IsDBNull(6) ? "" : rd.GetString(6)
                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<ПозицияПродажи> GetПозиции(int idProdaji)
        {
            var list = new List<ПозицияПродажи>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT pi.id, pi.idprodaji, pi.idproduct,
                           t.""Название"" AS ""Товар"",
                           pi.quantity, pi.price
                    FROM ""prodaja_info"" pi
                    JOIN ""Товары"" t ON t.id = pi.idproduct
                    WHERE pi.idprodaji = @id
                    ORDER BY pi.id";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idProdaji);
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new ПозицияПродажи
                            {
                                Id         = rd.GetInt32(0),
                                IdProdaji  = rd.GetInt32(1),
                                IdProduct  = rd.GetInt32(2),
                                Товар      = rd.IsDBNull(3) ? "" : rd.GetString(3),
                                Количество = rd.GetInt32(4),
                                Цена       = rd.GetDecimal(5)
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
