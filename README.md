# Бухгалтерия

Учебный проект (пара 15, вариант Муратов). Магазин ведёт учёт клиентов, товаров, складов, счетов-фактур, отгрузок и оплат, контролирует просрочку 20 дней, экспортирует счёт-фактуру и отчёт о просрочках в Excel, строит круговую диаграмму по ставкам НДС.

## Стек

- C# WinForms (.NET Framework 4.7.2)
- PostgreSQL 18 (Docker, контейнер `my_postgres`)
- Npgsql 4.1.10 — клиент PostgreSQL
- Microsoft.Office.Interop.Excel — экспорт в Excel (требует установленный MS Office)
- System.Windows.Forms.DataVisualization — круговая диаграмма

## База данных

Семь таблиц:

| Таблица        | Поля                                                          |
|----------------|---------------------------------------------------------------|
| Клиенты        | id, Название                                                  |
| Товары         | id, Название, СтавкаНДС                                       |
| Склады         | id, Название, Фабричный                                       |
| prodaja        | id, idclient, data, totalsum, oplacheno                       |
| prodaja_info   | id, idprodaji, idproduct, quantity, price                     |
| oplata         | id, idprodaji, data, sum                                      |
| Отгрузки       | id, СчетId, ТоварId, СкладId, Количество, Дата                |

На `oplata` висит триггер `tg_oplata_recalc` — при любом INSERT/UPDATE/DELETE он пересчитывает `prodaja.oplacheno = SUM(oplata.sum)` для соответствующей продажи. Статус продажи и оплаты не хранится в БД, вычисляется SQL-выражением (`Оплачен`/`Частично`/`Просрочен` по правилу 20 дней).

Скрипт создания и миграция данных со старой схемы — `sql/migration_prodaja.sql`.

Строка подключения по умолчанию (см. `DbConnectionHelper.cs`):
```
Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres
```

## Запуск

1. Поднять PostgreSQL в Docker:
   ```
   docker run -d --name my_postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 postgres
   ```

2. Создать таблицы (DDL для всех 7 таблиц — см. `sql/`).

3. Открыть `AccountingApp.csproj` в Visual Studio 2022 / 2026 Insider.

4. В `References` должны быть:
   - `Npgsql` (через NuGet, прописан в `packages.config`)
   - `System.Windows.Forms.DataVisualization` (Framework)
   - `Microsoft.Office.Interop.Excel` (COM → Microsoft Excel xx.x Object Library)

5. Build → Rebuild Solution (Ctrl+Shift+B).

6. F5 — запуск.

## Возможности

- CRUD для всех справочников: Клиенты, Товары (с НДС), Склады.
- Счета-накладные: верхний грид со счетами и нижний с позициями, связанные через КлиентId/ТоварId. Экспорт счёта-фактуры в Excel.
- Журнал отгрузок с привязкой к счёту, товару и складу.
- Журнал оплат. Статус вычисляется на лету:
  - `oplacheno >= totalsum` → «Оплачено»;
  - `(CURRENT_DATE - data) > 20` и долг есть → «Просрочено»;
  - `oplacheno > 0` → «Частично»;
  - иначе → «Не оплачено».
  Реальный пересчёт `oplacheno` — в триггере БД, приложение его не трогает.
- «Оплаты клиента» (drill-down): выбрал клиента → видны все его продажи; выбрал продажу → видны её товары и оплаты.
- Отчёты:
  - «Просроченные платежи» — на дату, мульти-выбор клиентов, экспорт в Excel.
  - «Диаграмма НДС за период» — круговая диаграмма распределения сумм по ставкам НДС.

## Структура проекта

```
.
├── AccountingApp.csproj
├── App.config
├── DbConnectionHelper.cs
├── Program.cs
├── Form1.cs / .Designer.cs               главное окно
├── FormТовары.cs                         справочник товаров
├── FormСклады.cs                         справочник складов
├── FormСчета.cs                          счета + позиции + экспорт
├── FormОтгрузки.cs                       отгрузки
├── FormОплаты.cs                         оплаты + логика просрочки
├── FormОтчёты.cs                         хаб для отчётов
├── FormОтчётПросрочка.cs                 отчёт «Просроченные платежи»
├── FormДиаграммаНДС.cs                   круговая диаграмма
├── Models/                               POCO-классы
└── Repositories/                         доступ к данным через Npgsql
```
