using System;

namespace AccountingApp.Models
{
    /// <summary>
    /// Продажа (prodaja). totalsum/oplacheno — снимок текущего состояния
    /// (триггер БД пересчитывает oplacheno при изменениях oplata).
    /// </summary>
    public class Прода
    {
        public int Id { get; set; }
        public int? КлиентId { get; set; }     // idclient
        public string Клиент { get; set; }
        public DateTime Дата { get; set; }     // data
        public decimal Сумма { get; set; }     // totalsum
        public decimal Оплачено { get; set; }  // oplacheno
        public decimal Долг => Сумма - Оплачено;
        public string Статус { get; set; }     // вычислимое поле
    }

    /// <summary>
    /// Позиция продажи (prodaja_info).
    /// </summary>
    public class ПозицияПродажи
    {
        public int Id { get; set; }
        public int IdProdaji { get; set; }
        public int IdProduct { get; set; }
        public string Товар { get; set; }
        public int Количество { get; set; }
        public decimal Цена { get; set; }
        public decimal Сумма => Количество * Цена;
    }
}
