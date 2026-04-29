using System;

namespace AccountingApp.Models
{
    public class Оплата
    {
        public int Id { get; set; }
        public int СчетId { get; set; }
        public string НомерСчета { get; set; }
        public string Клиент { get; set; }
        public decimal Сумма { get; set; }
        public DateTime Дата { get; set; }
        public string Статус { get; set; }
    }
}
