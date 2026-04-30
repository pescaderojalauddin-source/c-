using System;

namespace AccountingApp.Models
{
    /// <summary>
    /// Оплата (oplata). Поле СчетId хранит idprodaji из новой схемы.
    /// </summary>
    public class Оплата
    {
        public int Id { get; set; }
        public int СчетId { get; set; }            // в БД: oplata.idprodaji
        public string НомерСчета { get; set; }     // вычисляется: "Продажа #N от dd.MM.yyyy"
        public string Клиент { get; set; }
        public decimal Сумма { get; set; }         // в БД: oplata.sum
        public DateTime Дата { get; set; }         // в БД: oplata.data
        public string Статус { get; set; }         // вычисляется на лету по prodaja.oplacheno/totalsum
    }
}
