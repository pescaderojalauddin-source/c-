using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Npgsql;
using Excel = Microsoft.Office.Interop.Excel;

namespace AccountingApp
{
    public partial class FormСчета : Form
    {
        private int текущийСчетId = -1;

        public FormСчета()
        {
            InitializeComponent();
        }

        private void FormСчета_Load(object sender, EventArgs e)
        {
            if (!DbConnectionHelper.TestConnection())
            {
                MessageBox.Show("Нет подключения к БД!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            НастроитьГриды();
            ЗагрузитьСчета();
        }

        private void НастроитьГриды()
        {
            // Грид продаж
            dgvСчета.AutoGenerateColumns = false;
            dgvСчета.Columns.Clear();
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", DataPropertyName = "id", Width = 40, Visible = false });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Номер", HeaderText = "№ продажи", DataPropertyName = "Номер", Width = 100 });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Дата", HeaderText = "Дата", DataPropertyName = "Дата", Width = 100, DefaultCellStyle = { Format = "d" } });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Клиент", HeaderText = "Клиент", DataPropertyName = "Клиент", Width = 200 });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Сумма", HeaderText = "Сумма", DataPropertyName = "Сумма", Width = 100, DefaultCellStyle = { Format = "N2" } });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Оплачено", HeaderText = "Оплачено", DataPropertyName = "Оплачено", Width = 100, DefaultCellStyle = { Format = "N2" } });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Долг", HeaderText = "Долг", DataPropertyName = "Долг", Width = 100, DefaultCellStyle = { Format = "N2" } });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Статус", HeaderText = "Статус", DataPropertyName = "Статус", Width = 100 });
            dgvСчета.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Грид позиций
            dgvПозиции.AutoGenerateColumns = false;
            dgvПозиции.Columns.Clear();
            dgvПозиции.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", DataPropertyName = "id", Width = 40, Visible = false });
            dgvПозиции.Columns.Add(new DataGridViewTextBoxColumn { Name = "Товар", HeaderText = "Товар", DataPropertyName = "Товар", Width = 250 });
            dgvПозиции.Columns.Add(new DataGridViewTextBoxColumn { Name = "Количество", HeaderText = "Кол-во", DataPropertyName = "Количество", Width = 80 });
            dgvПозиции.Columns.Add(new DataGridViewTextBoxColumn { Name = "Цена", HeaderText = "Цена", DataPropertyName = "Цена", Width = 100, DefaultCellStyle = { Format = "N2" } });
            dgvПозиции.Columns.Add(new DataGridViewTextBoxColumn { Name = "Сумма", HeaderText = "Сумма", DataPropertyName = "Сумма", Width = 100, DefaultCellStyle = { Format = "N2" } });
            dgvПозиции.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void ЗагрузитьСчета()
        {
            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT p.id,
                               p.id::text                                AS ""Номер"",
                               p.data                                    AS ""Дата"",
                               COALESCE(k.""Название"", '')              AS ""Клиент"",
                               p.totalsum                                AS ""Сумма"",
                               p.oplacheno                               AS ""Оплачено"",
                               (p.totalsum - p.oplacheno)                AS ""Долг"",
                               CASE
                                   WHEN p.totalsum > 0 AND p.oplacheno >= p.totalsum THEN 'Оплачен'
                                   WHEN (CURRENT_DATE - p.data) > 20 AND p.oplacheno < p.totalsum THEN 'Просрочен'
                                   WHEN p.oplacheno > 0 THEN 'Частично'
                                   ELSE 'Не оплачен'
                               END                                       AS ""Статус""
                        FROM ""prodaja"" p
                        LEFT JOIN ""Клиенты"" k ON p.idclient = k.id
                        ORDER BY p.data DESC, p.id DESC";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        dgvСчета.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продаж: {ex.Message}\n\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvСчета_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvСчета.CurrentRow == null) return;

            текущийСчетId = Convert.ToInt32(dgvСчета.CurrentRow.Cells["Id"].Value);
            ЗагрузитьПозиции(текущийСчетId);
        }

        private void ЗагрузитьПозиции(int счетId)
        {
            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT pi.id,
                               COALESCE(t.""Название"", '')      AS ""Товар"",
                               pi.quantity                       AS ""Количество"",
                               pi.price                          AS ""Цена"",
                               (pi.quantity * pi.price)          AS ""Сумма""
                        FROM ""prodaja_info"" pi
                        LEFT JOIN ""Товары"" t ON pi.idproduct = t.id
                        WHERE pi.idprodaji = @id
                        ORDER BY pi.id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", счетId);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            var dt = new DataTable();
                            adapter.Fill(dt);
                            dgvПозиции.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки позиций: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnДобавитьСчет_Click(object sender, EventArgs e)
        {
            DataTable клиенты = new DataTable();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, \"Название\" FROM \"Клиенты\" ORDER BY \"Название\"", conn))
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(клиенты);
                }
            }

            var форма = new Form { Text = "Новая продажа", Width = 400, Height = 240, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };

            var lblКлиент = new Label { Text = "Клиент:", Location = new System.Drawing.Point(10, 10), Width = 350 };
            var cmbКлиент = new ComboBox { Location = new System.Drawing.Point(10, 30), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbКлиент.DataSource = клиенты;
            cmbКлиент.DisplayMember = "Название";
            cmbКлиент.ValueMember = "id";

            var lblДата = new Label { Text = "Дата:", Location = new System.Drawing.Point(10, 60), Width = 350 };
            var dtpДата = new DateTimePicker { Location = new System.Drawing.Point(10, 80), Width = 350, Format = DateTimePickerFormat.Short, Value = DateTime.Today };

            var btnOK = new Button { Text = "Создать", Location = new System.Drawing.Point(100, 130), Width = 100, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(210, 130), Width = 100, DialogResult = DialogResult.Cancel };

            форма.Controls.AddRange(new Control[] { lblКлиент, cmbКлиент, lblДата, dtpДата, btnOK, btnCancel });
            форма.AcceptButton = btnOK;
            форма.CancelButton = btnCancel;

            if (форма.ShowDialog() == DialogResult.OK)
            {
                if (cmbКлиент.SelectedValue == null)
                {
                    MessageBox.Show("Выберите клиента!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (var conn = DbConnectionHelper.GetConnection())
                    {
                        conn.Open();
                        string sql = @"INSERT INTO ""prodaja"" (idclient, data, totalsum, oplacheno)
                                       VALUES (@k, @d, 0, 0) RETURNING id";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@k", cmbКлиент.SelectedValue);
                            cmd.Parameters.AddWithValue("@d", dtpДата.Value.Date);
                            int newId = Convert.ToInt32(cmd.ExecuteScalar());

                            MessageBox.Show($"Продажа #{newId} создана!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ЗагрузитьСчета();

                            foreach (DataGridViewRow row in dgvСчета.Rows)
                            {
                                if (Convert.ToInt32(row.Cells["Id"].Value) == newId)
                                {
                                    dgvСчета.CurrentCell = row.Cells[1];
                                    row.Selected = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка создания продажи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnУдалитьСчет_Click(object sender, EventArgs e)
        {
            if (текущийСчетId == -1)
            {
                MessageBox.Show("Выберите продажу для удаления!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранную продажу со всеми позициями и оплатами?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (var conn = DbConnectionHelper.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand(@"DELETE FROM ""prodaja"" WHERE id = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", текущийСчетId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    текущийСчетId = -1;
                    dgvПозиции.DataSource = null;
                    MessageBox.Show("Продажа удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьСчета();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnДобавитьПозицию_Click(object sender, EventArgs e)
        {
            if (текущийСчетId == -1)
            {
                MessageBox.Show("Сначала выберите или создайте продажу!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable товары = new DataTable();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id, \"Название\" FROM \"Товары\" ORDER BY \"Название\"", conn))
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(товары);
                }
            }

            var форма = new Form { Text = "Добавить позицию", Width = 400, Height = 260, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };

            var lblТовар = new Label { Text = "Товар:", Location = new System.Drawing.Point(10, 10), Width = 350 };
            var cmbТовар = new ComboBox { Location = new System.Drawing.Point(10, 30), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbТовар.DataSource = товары;
            cmbТовар.DisplayMember = "Название";
            cmbТовар.ValueMember = "id";

            var lblКол = new Label { Text = "Количество:", Location = new System.Drawing.Point(10, 60), Width = 350 };
            var numКол = new NumericUpDown { Location = new System.Drawing.Point(10, 80), Width = 350, Minimum = 1, Maximum = 1000000, Value = 1 };

            var lblЦена = new Label { Text = "Цена за ед.:", Location = new System.Drawing.Point(10, 110), Width = 350 };
            var numЦена = new NumericUpDown { Location = new System.Drawing.Point(10, 130), Width = 350, Minimum = 0, Maximum = 100000000, DecimalPlaces = 2, Value = 0 };

            var btnOK = new Button { Text = "Добавить", Location = new System.Drawing.Point(100, 170), Width = 100, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(210, 170), Width = 100, DialogResult = DialogResult.Cancel };

            форма.Controls.AddRange(new Control[] { lblТовар, cmbТовар, lblКол, numКол, lblЦена, numЦена, btnOK, btnCancel });
            форма.AcceptButton = btnOK;
            форма.CancelButton = btnCancel;

            if (форма.ShowDialog() == DialogResult.OK)
            {
                if (cmbТовар.SelectedValue == null)
                {
                    MessageBox.Show("Выберите товар!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (var conn = DbConnectionHelper.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand(
                            @"INSERT INTO ""prodaja_info"" (idprodaji, idproduct, quantity, price)
                              VALUES (@p, @t, @k, @c)", conn))
                        {
                            cmd.Parameters.AddWithValue("@p", текущийСчетId);
                            cmd.Parameters.AddWithValue("@t", cmbТовар.SelectedValue);
                            cmd.Parameters.AddWithValue("@k", (int)numКол.Value);
                            cmd.Parameters.AddWithValue("@c", numЦена.Value);
                            cmd.ExecuteNonQuery();
                        }

                        ПересчитатьСумму(conn, текущийСчетId);
                    }

                    MessageBox.Show("Позиция добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьПозиции(текущийСчетId);
                    ЗагрузитьСчета();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления позиции: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnУдалитьПозицию_Click(object sender, EventArgs e)
        {
            if (dgvПозиции.CurrentRow == null || текущийСчетId == -1)
            {
                MessageBox.Show("Выберите позицию для удаления!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int позицияId = Convert.ToInt32(dgvПозиции.CurrentRow.Cells["Id"].Value);
            if (MessageBox.Show("Удалить эту позицию?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (var conn = DbConnectionHelper.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand(@"DELETE FROM ""prodaja_info"" WHERE id = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", позицияId);
                            cmd.ExecuteNonQuery();
                        }

                        ПересчитатьСумму(conn, текущийСчетId);
                    }

                    MessageBox.Show("Позиция удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьПозиции(текущийСчетId);
                    ЗагрузитьСчета();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static void ПересчитатьСумму(NpgsqlConnection conn, int idProdaji)
        {
            using (var cmd = new NpgsqlCommand(
                @"UPDATE ""prodaja"" SET totalsum = COALESCE(
                      (SELECT SUM(quantity * price) FROM ""prodaja_info"" WHERE idprodaji = @id), 0)
                  WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", idProdaji);
                cmd.ExecuteNonQuery();
            }
        }

        private void btnЭкспортСчета_Click(object sender, EventArgs e)
        {
            if (текущийСчетId <= 0 || dgvСчета.CurrentRow == null)
            {
                MessageBox.Show("Выберите продажу для экспорта!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int номерId = текущийСчетId;
            DateTime дата = DateTime.Today;
            string клиент = "";
            decimal суммаСчёта = 0m;
            decimal оплачено = 0m;
            string статус = "";

            DataTable позиции = new DataTable();

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(@"
                        SELECT p.data, COALESCE(k.""Название"",'') AS ""Клиент"",
                               p.totalsum, p.oplacheno,
                               CASE
                                   WHEN p.totalsum > 0 AND p.oplacheno >= p.totalsum THEN 'Оплачен'
                                   WHEN (CURRENT_DATE - p.data) > 20 AND p.oplacheno < p.totalsum THEN 'Просрочен'
                                   WHEN p.oplacheno > 0 THEN 'Частично'
                                   ELSE 'Не оплачен'
                               END
                        FROM ""prodaja"" p
                        LEFT JOIN ""Клиенты"" k ON p.idclient = k.id
                        WHERE p.id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", текущийСчетId);
                        using (var rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                дата       = rd.GetDateTime(0);
                                клиент     = rd.GetString(1);
                                суммаСчёта = rd.IsDBNull(2) ? 0m : rd.GetDecimal(2);
                                оплачено   = rd.IsDBNull(3) ? 0m : rd.GetDecimal(3);
                                статус     = rd.IsDBNull(4) ? "" : rd.GetString(4);
                            }
                        }
                    }

                    using (var cmd = new NpgsqlCommand(@"
                        SELECT t.""Название"" AS ""Товар"",
                               t.""СтавкаНДС"" AS ""НДС%"",
                               pi.quantity   AS ""Количество"",
                               pi.price      AS ""Цена"",
                               (pi.quantity * pi.price) AS ""Сумма""
                        FROM ""prodaja_info"" pi
                        JOIN ""Товары"" t ON pi.idproduct = t.id
                        WHERE pi.idprodaji = @id
                        ORDER BY pi.id", conn))
                    using (var da = new NpgsqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@id", текущийСчетId);
                        da.Fill(позиции);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения данных продажи:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var sfd = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                FileName = $"Счёт-фактура_№{номерId}_{дата:yyyyMMdd}.xlsx"
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
                    ws.Name = "Счёт-фактура";

                    // Шапка документа
                    ws.Cells[1, 1] = $"СЧЁТ-ФАКТУРА № {номерId}";
                    ((Excel.Range)ws.Cells[1, 1]).Font.Bold = true;
                    ((Excel.Range)ws.Cells[1, 1]).Font.Size = 16;
                    ws.Range[ws.Cells[1, 1], ws.Cells[1, 6]].Merge();
                    ((Excel.Range)ws.Cells[1, 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    ws.Cells[3, 1] = "Дата:";
                    ws.Cells[3, 2] = дата.ToString("dd.MM.yyyy");
                    ws.Cells[4, 1] = "Покупатель:";
                    ws.Cells[4, 2] = клиент;
                    ws.Cells[5, 1] = "Статус:";
                    ws.Cells[5, 2] = статус;
                    ws.Cells[6, 1] = "Оплачено:";
                    ws.Cells[6, 2] = оплачено;
                    try { ((Excel.Range)ws.Cells[6, 2]).NumberFormatLocal = "# ##0,00"; } catch { }

                    ((Excel.Range)ws.Cells[3, 1]).Font.Bold = true;
                    ((Excel.Range)ws.Cells[4, 1]).Font.Bold = true;
                    ((Excel.Range)ws.Cells[5, 1]).Font.Bold = true;
                    ((Excel.Range)ws.Cells[6, 1]).Font.Bold = true;

                    // Заголовок таблицы товаров
                    int hdrRow = 8;
                    string[] headers = { "№", "Товар", "Ставка НДС, %", "Кол-во", "Цена", "Сумма" };
                    for (int c = 0; c < headers.Length; c++)
                    {
                        ws.Cells[hdrRow, c + 1] = headers[c];
                        var cell = (Excel.Range)ws.Cells[hdrRow, c + 1];
                        cell.Font.Bold = true;
                        cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        cell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    }

                    // Позиции
                    int row = hdrRow + 1;
                    for (int r = 0; r < позиции.Rows.Count; r++)
                    {
                        var dr = позиции.Rows[r];
                        ws.Cells[row, 1] = r + 1;
                        ws.Cells[row, 2] = dr["Товар"]?.ToString();
                        ws.Cells[row, 3] = dr["НДС%"];
                        ws.Cells[row, 4] = dr["Количество"];
                        ws.Cells[row, 5] = dr["Цена"];
                        ws.Cells[row, 6] = dr["Сумма"];

                        for (int c = 1; c <= 6; c++)
                            ((Excel.Range)ws.Cells[row, c]).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                        row++;
                    }

                    // Числовые форматы
                    if (позиции.Rows.Count > 0)
                    {
                        int dataLast = row - 1;
                        try
                        {
                            ws.Range[ws.Cells[hdrRow + 1, 5], ws.Cells[dataLast, 6]].NumberFormatLocal = "# ##0,00";
                        }
                        catch { /* локаль может отличаться — пропустим */ }
                    }

                    // Итог
                    ws.Cells[row, 5] = "ИТОГО:";
                    var totalLbl = (Excel.Range)ws.Cells[row, 5];
                    totalLbl.Font.Bold = true;
                    totalLbl.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    ws.Cells[row, 6] = суммаСчёта;
                    var totalVal = (Excel.Range)ws.Cells[row, 6];
                    totalVal.Font.Bold = true;
                    try { totalVal.NumberFormatLocal = "# ##0,00"; } catch { }
                    totalVal.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightYellow);

                    ws.Columns.AutoFit();
                    wb.SaveAs(sfd.FileName);
                    wb.Close(false);
                    app.Quit();

                    MessageBox.Show($"Счёт-фактура сохранён:\n{sfd.FileName}", "Готово",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    try { System.Diagnostics.Process.Start(sfd.FileName); } catch { }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта:\n{ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (ws != null) Marshal.ReleaseComObject(ws);
                    if (wb != null) Marshal.ReleaseComObject(wb);
                    if (app != null) Marshal.ReleaseComObject(app);
                }
            }
        }
    }
}
