using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Npgsql;
using Excel = Microsoft.Office.Interop.Excel;

namespace AccountingApp
{
    public partial class FormОтчётПросрочка : Form
    {
        private DataTable текущийРезультат;

        public FormОтчётПросрочка()
        {
            InitializeComponent();
        }

        private void FormОтчётПросрочка_Load(object sender, EventArgs e)
        {
            if (!DbConnectionHelper.TestConnection())
            {
                MessageBox.Show("Нет подключения к БД!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dtpНаДату.Value = DateTime.Today;
            ЗагрузитьКлиентов();
            НастроитьГрид();
        }

        private void ЗагрузитьКлиентов()
        {
            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(
                        "SELECT id, \"Название\" FROM \"Клиенты\" ORDER BY \"Название\"", conn))
                    using (var rd = cmd.ExecuteReader())
                    {
                        clbКлиенты.Items.Clear();
                        while (rd.Read())
                        {
                            clbКлиенты.Items.Add(new КлиентItem
                            {
                                Id = rd.GetInt32(0),
                                Название = rd.GetString(1)
                            }, true); // по умолчанию все отмечены
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void НастроитьГрид()
        {
            dgvРезультат.AutoGenerateColumns = true;
            dgvРезультат.DefaultCellStyle.Format = "";
        }

        private void btnВыбратьВсех_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbКлиенты.Items.Count; i++)
                clbКлиенты.SetItemChecked(i, true);
        }

        private void btnСнять_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbКлиенты.Items.Count; i++)
                clbКлиенты.SetItemChecked(i, false);
        }

        private void btnСформировать_Click(object sender, EventArgs e)
        {
            var выбранные = new List<int>();
            foreach (var item in clbКлиенты.CheckedItems)
            {
                if (item is КлиентItem ki) выбранные.Add(ki.Id);
            }

            if (выбранные.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одного клиента!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime наДату = dtpНаДату.Value.Date;

            try
            {
                текущийРезультат = ПолучитьПросроченные(выбранные, наДату);
                dgvРезультат.DataSource = текущийРезультат;

                ОформитьКолонки();

                decimal итогоДолг = 0m;
                foreach (DataRow r in текущийРезультат.Rows)
                {
                    if (r["Долг"] != DBNull.Value)
                        итогоДолг += Convert.ToDecimal(r["Долг"]);
                }
                lblИтого.Text = $"Итого долг: {итогоДолг:N2} ₽   |   Записей: {текущийРезультат.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка построения отчёта:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ОформитьКолонки()
        {
            if (dgvРезультат.Columns.Contains("Сумма счёта"))
                dgvРезультат.Columns["Сумма счёта"].DefaultCellStyle.Format = "N2";
            if (dgvРезультат.Columns.Contains("Оплачено"))
                dgvРезультат.Columns["Оплачено"].DefaultCellStyle.Format = "N2";
            if (dgvРезультат.Columns.Contains("Долг"))
                dgvРезультат.Columns["Долг"].DefaultCellStyle.Format = "N2";
            if (dgvРезультат.Columns.Contains("Дата"))
                dgvРезультат.Columns["Дата"].DefaultCellStyle.Format = "d";

            // Подсветка по числу дней просрочки
            foreach (DataGridViewRow row in dgvРезультат.Rows)
            {
                if (row.Cells["Дней просрочки"].Value is int дней)
                {
                    if (дней > 60) row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                    else if (дней > 30) row.DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose;
                    else row.DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                }
            }
        }

        private DataTable ПолучитьПросроченные(List<int> клиентыIds, DateTime наДату)
        {
            var dt = new DataTable();
            dt.Columns.Add("Счёт №",        typeof(string));
            dt.Columns.Add("Дата",          typeof(DateTime));
            dt.Columns.Add("Клиент",        typeof(string));
            dt.Columns.Add("Сумма счёта",   typeof(decimal));
            dt.Columns.Add("Оплачено",      typeof(decimal));
            dt.Columns.Add("Долг",          typeof(decimal));
            dt.Columns.Add("Дней просрочки",typeof(int));

            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT p.id,
                           p.id::text                                                AS ""Номер"",
                           p.data                                                    AS ""Дата"",
                           k.""Название""                                            AS ""Клиент"",
                           p.totalsum                                                AS ""СуммаСчета"",
                           COALESCE((SELECT SUM(o.sum) FROM ""oplata"" o
                                     WHERE o.idprodaji = p.id AND o.data <= @date), 0) AS ""Оплачено""
                    FROM ""prodaja"" p
                    JOIN ""Клиенты"" k ON p.idclient = k.id
                    WHERE p.idclient = ANY(@ids)
                      AND p.data + INTERVAL '20 days' < @date
                    ORDER BY p.data";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ids",  клиентыIds.ToArray());
                    cmd.Parameters.AddWithValue("@date", наДату);

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            decimal сумма = rd.IsDBNull(4) ? 0m : rd.GetDecimal(4);
                            decimal оплачено = rd.IsDBNull(5) ? 0m : rd.GetDecimal(5);
                            decimal долг = сумма - оплачено;
                            if (долг <= 0) continue; // полностью оплачено — не просрочка

                            DateTime датаСчета = rd.GetDateTime(2);
                            int дней = (int)(наДату - датаСчета).TotalDays - 20;

                            dt.Rows.Add(
                                "#" + rd.GetValue(1).ToString(),
                                датаСчета,
                                rd.GetString(3),
                                сумма,
                                оплачено,
                                долг,
                                дней
                            );
                        }
                    }
                }
            }
            return dt;
        }

        private void btnЭкспорт_Click(object sender, EventArgs e)
        {
            if (текущийРезультат == null || текущийРезультат.Rows.Count == 0)
            {
                MessageBox.Show("Сначала сформируйте отчёт!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var sfd = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                FileName = $"Просроченные_платежи_{DateTime.Now:yyyyMMdd}.xlsx"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                Excel.Application app = null;
                Excel.Workbook wb = null;
                Excel.Worksheet ws = null;
                try
                {
                    app = new Excel.Application { Visible = false, DisplayAlerts = false };
                    wb = app.Workbooks.Add();
                    ws = (Excel.Worksheet)wb.ActiveSheet;
                    ws.Name = "Просрочка";

                    // Заголовок
                    ws.Cells[1, 1] = "Отчёт: Просроченные платежи";
                    ((Excel.Range)ws.Cells[1, 1]).Font.Bold = true;
                    ((Excel.Range)ws.Cells[1, 1]).Font.Size = 14;
                    ws.Range[ws.Cells[1, 1], ws.Cells[1, текущийРезультат.Columns.Count]].Merge();

                    ws.Cells[2, 1] = $"На дату: {dtpНаДату.Value:dd.MM.yyyy}";
                    ((Excel.Range)ws.Cells[2, 1]).Font.Italic = true;

                    int headerRow = 4;

                    // Заголовки колонок
                    for (int c = 0; c < текущийРезультат.Columns.Count; c++)
                    {
                        ws.Cells[headerRow, c + 1] = текущийРезультат.Columns[c].ColumnName;
                        var cell = (Excel.Range)ws.Cells[headerRow, c + 1];
                        cell.Font.Bold = true;
                        cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        cell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    }

                    // Данные
                    decimal итого = 0m;
                    for (int r = 0; r < текущийРезультат.Rows.Count; r++)
                    {
                        for (int c = 0; c < текущийРезультат.Columns.Count; c++)
                        {
                            var val = текущийРезультат.Rows[r][c];
                            ws.Cells[headerRow + 1 + r, c + 1] = val == DBNull.Value ? "" : val;
                            ((Excel.Range)ws.Cells[headerRow + 1 + r, c + 1])
                                .Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                        итого += Convert.ToDecimal(текущийРезультат.Rows[r]["Долг"]);
                    }

                    // Форматирование числовых колонок
                    int dataLastRow = headerRow + текущийРезультат.Rows.Count;
                    int[] numericCols = { 4, 5, 6 }; // Сумма счёта, Оплачено, Долг
                    foreach (var col in numericCols)
                    {
                        var range = ws.Range[ws.Cells[headerRow + 1, col], ws.Cells[dataLastRow, col]];
                        range.NumberFormat = "#,##0.00";
                    }
                    ws.Range[ws.Cells[headerRow + 1, 2], ws.Cells[dataLastRow, 2]].NumberFormat = "dd.mm.yyyy";

                    // Итоговая строка
                    int итогоRow = dataLastRow + 1;
                    ws.Cells[итогоRow, 5] = "ИТОГО ДОЛГ:";
                    ((Excel.Range)ws.Cells[итогоRow, 5]).Font.Bold = true;
                    ((Excel.Range)ws.Cells[итогоRow, 5]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    ws.Cells[итогоRow, 6] = итого;
                    var итогоCell = (Excel.Range)ws.Cells[итогоRow, 6];
                    итогоCell.Font.Bold = true;
                    итогоCell.NumberFormat = "#,##0.00";
                    итогоCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightYellow);

                    ws.Columns.AutoFit();
                    wb.SaveAs(sfd.FileName);
                    wb.Close(false);
                    app.Quit();

                    MessageBox.Show($"Отчёт сохранён:\n{sfd.FileName}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта в Excel:\n{ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    try { wb?.Close(false); app?.Quit(); } catch { }
                }
                finally
                {
                    if (ws != null)  Marshal.ReleaseComObject(ws);
                    if (wb != null)  Marshal.ReleaseComObject(wb);
                    if (app != null) Marshal.ReleaseComObject(app);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }

        private class КлиентItem
        {
            public int Id { get; set; }
            public string Название { get; set; }
            public override string ToString() => Название;
        }
    }
}
