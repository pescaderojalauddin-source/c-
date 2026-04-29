-- Создание таблицы "Оплаты"
-- Выполнить в DBeaver / pgAdmin / psql, подключившись к БД postgres в контейнере my_postgres.

CREATE TABLE IF NOT EXISTS "Оплаты" (
    id        SERIAL PRIMARY KEY,
    "СчетId"  INTEGER NOT NULL REFERENCES "Счета"(id) ON DELETE CASCADE,
    "Сумма"   NUMERIC(14,2) NOT NULL CHECK ("Сумма" > 0),
    "Дата"    DATE NOT NULL DEFAULT CURRENT_DATE,
    "Статус"  VARCHAR(50) NOT NULL DEFAULT 'Частично'
);

CREATE INDEX IF NOT EXISTS idx_Оплаты_СчетId ON "Оплаты" ("СчетId");
CREATE INDEX IF NOT EXISTS idx_Оплаты_Дата   ON "Оплаты" ("Дата");

-- Проверка
SELECT * FROM "Оплаты";
