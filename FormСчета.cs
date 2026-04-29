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
            ЗагрузитьСчета();
            НастроитьГриды();
        }

        private void НастроитьГриды()
        {
            // Настройка грида счетов
            dgvСчета.AutoGenerateColumns = false;
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", DataPropertyName = "id", Width = 40, Visible = false });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Номер", HeaderText = "Номер счёта", DataPropertyName = "Номер", Width = 100 });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Дата", HeaderText = "Дата", DataPropertyName = "Дата", Width = 100 });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Клиент", HeaderText = "Клиент", DataPropertyName = "Клиент", Width = 200 });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Сумма", HeaderText = "Сумма", DataPropertyName = "Сумма", Width = 100, DefaultCellStyle = { Format = "N2" } });
            dgvСчета.Columns.Add(new DataGridViewTextBoxColumn { Name = "Статус", HeaderText = "Статус", DataPropertyName = "Статус", Width = 100 });
            dgvСчета.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Настройка грида позиций
            dgvПозиции.AutoGenerateColumns = false;
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
                    // ИСПРАВЛЕНО: к.id вместо к."Id"
                    string query = @"
                        SELECT с.id, с.""Номер"", с.""Дата"", к.""Название"" AS ""Клиент"", с.""Сумма"", с.""Статус""
                        FROM ""Счета"" с
                        LEFT JOIN ""Клиенты"" к ON с.""КлиентId"" = к.id
                        ORDER BY с.""Дата"" DESC";
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
                MessageBox.Show($"Ошибка загрузки счетов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    // ИСПРАВЛЕНО: т.id вместо т."Id"
                    string query = @"
                        SELECT п.id, т.""Название"" AS ""Товар"", п.""Количество"", п.""Цена"", п.""Сумма""
                        FROM ""ПозицииСчета"" п
                        LEFT JOIN ""Товары"" т ON п.""ТоварId"" = т.id
                        WHERE п.""СчетId"" = @СчетId";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@СчетId", счетId);
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

            var форма = new Form { Text = "Новый счёт", Width = 400, Height = 280, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };

            var lblНомер = new Label { Text = "Номер счёта:", Location = new System.Drawing.Point(10, 10), Width = 350 };
            var txtНомер = new TextBox { Location = new System.Drawing.Point(10, 30), Width = 350 };

            var lblКлиент = new Label { Text = "Клиент:", Location = new System.Drawing.Point(10, 60), Width = 350 };
            var cmbКлиент = new ComboBox { Location = new System.Drawing.Point(10, 80), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbКлиент.DataSource = клиенты;
            cmbКлиент.DisplayMember = "Название";
            cmbКлиент.ValueMember = "id";

            var lblСтатус = new Label { Text = "Статус:", Location = new System.Drawing.Point(10, 110), Width = 350 };
            var cmbСтатус = new ComboBox { Location = new System.Drawing.Point(10, 130), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbСтатус.Items.AddRange(new[] { "Черновик", "Отправлен", "Оплачен", "Закрыт" });
            cmbСтатус.SelectedIndex = 0;

            var btnOK = new Button { Text = "Создать", Location = new System.Drawing.Point(100, 170), Width = 100, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(210, 170), Width = 100, DialogResult = DialogResult.Cancel };

            форма.Controls.AddRange(new Control[] { lblНомер, txtНомер, lblКлиент, cmbКлиент, lblСтатус, cmbСтатус, btnOK, btnCancel });
            форма.AcceptButton = btnOK;
            форма.CancelButton = btnCancel;

            if (форма.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txtНомер.Text) || cmbКлиент.SelectedValue == null)
                {
                    MessageBox.Show("Заполните номер и выберите клиента!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (var conn = DbConnectionHelper.GetConnection())
                    {
                        conn.Open();
                        string sql = "INSERT INTO \"Счета\" (\"Номер\", \"КлиентId\", \"Статус\") VALUES (@Номер, @КлиентId, @Статус) RETURNING id";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@Номер", txtНомер.Text.Trim());
                            cmd.Parameters.AddWithValue("@КлиентId", cmbКлиент.SelectedValue);
                            cmd.Parameters.AddWithValue("@Статус", cmbСтатус.Text);
                            int newId = Convert.ToInt32(cmd.ExecuteScalar());

                            MessageBox.Show("✅ Счёт создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show($"Ошибка создания счёта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnУдалитьСчет_Click(object sender, EventArgs e)
        {
            if (текущийСчетId == -1)
            {
                MessageBox.Show("Выберите счёт для удаления!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранный счёт и все его позиции?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (var conn = DbConnectionHelper.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand("DELETE FROM \"Счета\" WHERE id = @Id", conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", текущийСчетId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    текущийСчетId = -1;
                    dgvПозиции.DataSource = null;
                    MessageBox.Show("✅ Счёт удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Сначала выберите или создайте счёт!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            var numКол = new NumericUpDown { Location = new System.Drawing.Point(10, 80), Width = 350, Minimum = 1, Value = 1 };

            var lblЦена = new Label { Text = "Цена за ед.:", Location = new System.Drawing.Point(10, 110), Width = 350 };
            var numЦена = new NumericUpDown { Location = new System.Drawing.Point(10, 130), Width = 350, Minimum = 0, DecimalPlaces = 2, Value = 0 };

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
                            "INSERT INTO \"ПозицииСчета\" (\"СчетId\", \"ТоварId\", \"Количество\", \"Цена\") VALUES (@СчетId, @ТоварId, @Кол, @Цена)", conn))
                        {
                            cmd.Parameters.AddWithValue("@СчетId", текущийСчетId);
                            cmd.Parameters.AddWithValue("@ТоварId", cmbТовар.SelectedValue);
                            cmd.Parameters.AddWithValue("@Кол", numКол.Value);
                            cmd.Parameters.AddWithValue("@Цена", numЦена.Value);
                            cmd.ExecuteNonQuery();
                        }

                        using (var cmdSum = new NpgsqlCommand(
                            "UPDATE \"Счета\" SET \"Сумма\" = (SELECT COALESCE(SUM(\"Сумма\"), 0) FROM \"ПозицииСчета\" WHERE \"СчетId\" = @СчетId) WHERE id = @СчетId", conn))
                        {
                            cmdSum.Parameters.AddWithValue("@СчетId", текущийСчетId);
                            cmdSum.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("✅ Позиция добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        using (var cmd = new NpgsqlCommand("DELETE FROM \"ПозицииСчета\" WHERE id = @Id", conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", позицияId);
                            cmd.ExecuteNonQuery();
                        }

                        using (var cmdSum = new NpgsqlCommand(
                            "UPDATE \"Счета\" SET \"Сумма\" = (SELECT COALESCE(SUM(\"Сумма\"), 0) FROM \"ПозицииСчета\" WHERE \"СчетId\" = @СчетId) WHERE id = @СчетId", conn))
                        {
                            cmdSum.Parameters.AddWithValue("@СчетId", текущийСчетId);
                            cmdSum.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("✅ Позиция удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьПозиции(текущийСчетId);
                    ЗагрузитьСчета();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnЭкспортСчета_Click(object sender, EventArgs e)
        {
            if (текущийСчетId <= 0 || dgvСчета.CurrentRow == null)
            {
                MessageBox.Show("Выберите счёт для экспорта!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Берём данные счёта из БД
            string номер = "";
            DateTime дата = DateTime.Today;
            string клиент = "";
            decimal суммаСчёта = 0m;
            string статус = "";

            DataTable позиции = new DataTable();

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(@"
                        SELECT с.""Номер"", с.""Дата"", COALESCE(к.""Название"",'') AS ""Клиент"",
                               с.""Сумма"", с.""Статус""
                        FROM ""Счета"" с
                        LEFT JOIN ""Клиенты"" к ON с.""КлиентId"" = к.id
                        WHERE с.id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", текущийСчетId);
                        using (var rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                номер     = rd.GetValue(0).ToString();
                                дата      = rd.GetDateTime(1);
                                клиент    = rd.GetString(2);
                                суммаСчёта = rd.IsDBNull(3) ? 0m : rd.GetDecimal(3);
                                статус    = rd.IsDBNull(4) ? "" : rd.GetString(4);
                            }
                        }
                    }

                    using (var cmd = new NpgsqlCommand(@"
                        SELECT т.""Название"" AS ""Товар"",
                               т.""СтавкаНДС"" AS ""НДС%"",
                               п.""Количество"",
                               п.""Цена"",
                               п.""Сумма""
                        FROM ""ПозицииСчета"" п
                        JOIN ""Товары"" т ON п.""ТоварId"" = т.id
                        WHERE п.""СчетId"" = @id
                        ORDER BY п.id", conn))
                    using (var da = new NpgsqlDataAdapter(cmd))
                    {
                        cmd.Parameters.AddWithValue("@id", текущийСчетId);
                        da.Fill(позиции);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения данных счёта:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var sfd = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                FileName = $"Счёт-фактура_№{номер}_{дата:yyyyMMdd}.xlsx"
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
                    ws.Cells[1, 1] = $"СЧЁТ-ФАКТУРА № {номер}";
                    ((Excel.Range)ws.Cells[1, 1]).Font.Bold = true;
                    ((Excel.Range)ws.Cells[1, 1]).Font.Size = 16;
                    ws.Range[ws.Cells[1, 1], ws.Cells[1, 5]].Merge();
                    ((Excel.Range)ws.Cells[1, 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    ws.Cells[3, 1] = "Дата:";
                    ws.Cells[3, 2] = дата.ToString("dd.MM.yyyy");
                    ws.Cells[4, 1] = "Покупатель:";
                    ws.Cells[4, 2] = клиент;
                    ws.Cells[5, 1] = "Статус:";
                    ws.Cells[5, 2] = статус;

                    ((Excel.Range)ws.Cells[3, 1]).Font.Bold = true;
                    ((Excel.Range)ws.Cells[4, 1]).Font.Bold = true;
                    ((Excel.Range)ws.Cells[5, 1]).Font.Bold = true;

                    // Заголовок таблицы товаров
                    int hdrRow = 7;
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
                        ws.Range[ws.Cells[hdrRow + 1, 5], ws.Cells[dataLast, 6]].NumberFormat = "#,##0.00";
                    }

                    // Итог
                    ws.Cells[row, 5] = "ИТОГО:";
                    var totalLbl = (Excel.Range)ws.Cells[row, 5];
                    totalLbl.Font.Bold = true;
                    totalLbl.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    ws.Cells[row, 6] = суммаСчёта;
                    var totalVal = (Excel.Range)ws.Cells[row, 6];
                    totalVal.Font.Bold = true;
                    totalVal.NumberFormat = "#,##0.00";
                    totalVal.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightYellow);

                    ws.Columns.AutoFit();
                    wb.SaveAs(sfd.FileName);
                    wb.Close(false);
                    app.Quit();

                    MessageBox.Show($"Счёт-фактура сохранён:\n{sfd.FileName}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта:\n{ex.Message}", "Ошибка",
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
    }
}