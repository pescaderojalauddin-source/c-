-- ============================================================
--  Миграция со старой схемы (Счета / ПозицииСчета / Оплаты)
--  на новую (prodaja / prodaja_info / oplata) по требованиям ТЗ.
--
--  Идентификаторы записей сохраняются — старый Счета.id становится
--  prodaja.id, и т.д. Это безопасно для FK Отгрузки.СчетId.
--
--  Запускать ОДИН РАЗ. Если уже запускался — упадёт на CREATE TABLE.
-- ============================================================

BEGIN;

-- ------------------------------------------------------------
-- 1. Новые таблицы
-- ------------------------------------------------------------
CREATE TABLE "prodaja" (
    id        SERIAL PRIMARY KEY,
    idclient  INTEGER REFERENCES "Клиенты"(id),
    data      DATE NOT NULL DEFAULT CURRENT_DATE,
    totalsum  NUMERIC(15,2) NOT NULL DEFAULT 0,
    oplacheno NUMERIC(15,2) NOT NULL DEFAULT 0
);

CREATE TABLE "prodaja_info" (
    id        SERIAL PRIMARY KEY,
    idprodaji INTEGER NOT NULL REFERENCES "prodaja"(id) ON DELETE CASCADE,
    idproduct INTEGER NOT NULL REFERENCES "Товары"(id),
    quantity  INTEGER NOT NULL CHECK (quantity > 0),
    price     NUMERIC(15,2) NOT NULL
);

CREATE TABLE "oplata" (
    id        SERIAL PRIMARY KEY,
    idprodaji INTEGER NOT NULL REFERENCES "prodaja"(id) ON DELETE CASCADE,
    data      DATE NOT NULL DEFAULT CURRENT_DATE,
    sum       NUMERIC(15,2) NOT NULL CHECK (sum > 0)
);

CREATE INDEX idx_prodaja_info_idprodaji ON "prodaja_info"(idprodaji);
CREATE INDEX idx_oplata_idprodaji      ON "oplata"(idprodaji);
CREATE INDEX idx_prodaja_data          ON "prodaja"(data);

-- ------------------------------------------------------------
-- 2. Триггер: при добавлении/изменении/удалении оплаты
--    пересчитывает prodaja.oplacheno
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION recalc_oplacheno()
RETURNS TRIGGER AS $$
DECLARE
    target_id INTEGER;
BEGIN
    IF TG_OP = 'DELETE' THEN
        target_id := OLD.idprodaji;
    ELSE
        target_id := NEW.idprodaji;
    END IF;

    UPDATE "prodaja"
       SET oplacheno = COALESCE((SELECT SUM(sum)
                                 FROM "oplata"
                                 WHERE idprodaji = target_id), 0)
     WHERE id = target_id;

    -- Если оплата перенесена с одной продажи на другую — пересчитать обе
    IF TG_OP = 'UPDATE' AND OLD.idprodaji <> NEW.idprodaji THEN
        UPDATE "prodaja"
           SET oplacheno = COALESCE((SELECT SUM(sum)
                                     FROM "oplata"
                                     WHERE idprodaji = OLD.idprodaji), 0)
         WHERE id = OLD.idprodaji;
    END IF;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER tg_oplata_recalc
AFTER INSERT OR UPDATE OR DELETE ON "oplata"
FOR EACH ROW EXECUTE FUNCTION recalc_oplacheno();

-- ------------------------------------------------------------
-- 3. Перенос данных из старых таблиц
-- ------------------------------------------------------------

-- Счета -> prodaja (с сохранением id)
INSERT INTO "prodaja" (id, idclient, data, totalsum, oplacheno)
SELECT id,
       "КлиентId",
       COALESCE("Дата", CURRENT_DATE),
       COALESCE("Сумма", 0),
       0
FROM "Счета";

SELECT setval(pg_get_serial_sequence('"prodaja"', 'id'),
              COALESCE((SELECT MAX(id) FROM "prodaja"), 1));

-- ПозицииСчета -> prodaja_info
INSERT INTO "prodaja_info" (id, idprodaji, idproduct, quantity, price)
SELECT id,
       "СчетId",
       "ТоварId",
       "Количество",
       "Цена"
FROM "ПозицииСчета";

SELECT setval(pg_get_serial_sequence('"prodaja_info"', 'id'),
              COALESCE((SELECT MAX(id) FROM "prodaja_info"), 1));

-- Оплаты -> oplata. Триггер пересчитает oplacheno автоматически
INSERT INTO "oplata" (id, idprodaji, data, sum)
SELECT id,
       "СчетId",
       "Дата",
       "Сумма"
FROM "Оплаты"
WHERE "Сумма" > 0;

SELECT setval(pg_get_serial_sequence('"oplata"', 'id'),
              COALESCE((SELECT MAX(id) FROM "oplata"), 1));

-- ------------------------------------------------------------
-- 4. Перевести FK у Отгрузки на новую таблицу prodaja
-- ------------------------------------------------------------
ALTER TABLE "Отгрузки" DROP CONSTRAINT IF EXISTS "Отгрузки_СчетId_fkey";

ALTER TABLE "Отгрузки"
    ADD CONSTRAINT "Отгрузки_СчетId_fkey"
    FOREIGN KEY ("СчетId") REFERENCES "prodaja"(id) ON DELETE CASCADE;

-- ------------------------------------------------------------
-- 5. Удалить старые таблицы
-- ------------------------------------------------------------
DROP TABLE "Оплаты"        CASCADE;
DROP TABLE "ПозицииСчета"  CASCADE;
DROP TABLE "Счета"         CASCADE;

COMMIT;

-- ------------------------------------------------------------
-- Проверка
-- ------------------------------------------------------------
-- SELECT id, idclient, data, totalsum, oplacheno FROM "prodaja" ORDER BY id;
-- SELECT id, idprodaji, idproduct, quantity, price FROM "prodaja_info" ORDER BY id;
-- SELECT id, idprodaji, data, sum FROM "oplata" ORDER BY id;
