using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using AccountingApp.Repositories;

namespace AccountingApp
{
    public partial class FormОплаты : Form
    {
        private ОплатыRepository repo;

        public FormОплаты()
        {
            InitializeComponent();
            repo = new ОплатыRepository();
        }

        private void FormОплаты_Load(object sender, EventArgs e)
        {
            if (!DbConnectionHelper.TestConnection())
            {
                MessageBox.Show("Нет подключения к БД!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            НастройкаDataGridView();
            Загрузить();
        }

        private void НастройкаDataGridView()
        {
            dgvОплаты.AutoGenerateColumns = false;
            dgvОплаты.Columns.Clear();

            dgvОплаты.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Id", HeaderText = "ID", DataPropertyName = "Id", Width = 40, Visible = false });

            dgvОплаты.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Счет", HeaderText = "Продажа", DataPropertyName = "НомерСчета", Width = 200 });

            dgvОплаты.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Клиент", HeaderText = "Клиент", DataPropertyName = "Клиент", Width = 250 });

            dgvОплаты.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Сумма", HeaderText = "Сумма", DataPropertyName = "Сумма",
                Width = 120, DefaultCellStyle = { Format = "N2" }
            });

            dgvОплаты.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Дата", HeaderText = "Дата оплаты", DataPropertyName = "Дата",
                Width = 120, DefaultCellStyle = { Format = "d" }
            });

            dgvОплаты.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Статус", HeaderText = "Статус", DataPropertyName = "Статус", Width = 130 });

            dgvОплаты.SelectionMode      = DataGridViewSelectionMode.FullRowSelect;
            dgvОплаты.AllowUserToAddRows = false;
            dgvОплаты.AllowUserToDeleteRows = false;
            dgvОплаты.MultiSelect = false;
            dgvОплаты.ReadOnly    = true;
        }

        private void Загрузить()
        {
            try
            {
                var список = repo.GetAll();
                dgvОплаты.DataSource = null;
                dgvОплаты.DataSource = список;
                this.Text = $"Оплаты по счетам (записей: {список.Count})";
                ПодсветитьПросроченные();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки оплат:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ПодсветитьПросроченные()
        {
            foreach (DataGridViewRow row in dgvОплаты.Rows)
            {
                var статус = row.Cells["Статус"].Value?.ToString();
                if (статус == "Просрочено")
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose;
                else if (статус == "Оплачено")
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.Honeydew;
                else if (статус == "Частично")
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
            }
        }

        private DataTable LoadTable(string query)
        {
            var dt = new DataTable();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var da = new NpgsqlDataAdapter(cmd))
                    da.Fill(dt);
            }
            return dt;
        }

        private void btnДобавить_Click(object sender, EventArgs e)
        {
            DataTable счета = LoadTable(@"
                SELECT p.id,
                       ('Продажа #' || p.id || ' от ' || to_char(p.data, 'DD.MM.YYYY') ||
                        ' (' || COALESCE(k.""Название"", 'без клиента') || ', ' ||
                        to_char(p.totalsum, 'FM999999990.00') || ' ₽)') AS ""Описание""
                FROM ""prodaja"" p
                LEFT JOIN ""Клиенты"" k ON p.idclient = k.id
                ORDER BY p.data DESC, p.id DESC");

            if (счета.Rows.Count == 0)
            {
                MessageBox.Show("Сначала создайте хотя бы одну продажу!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dlg = new Form
            {
                Text = "Новая оплата",
                Width = 460,
                Height = 280,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblСчет = new Label
            { Text = "Продажа:", Location = new System.Drawing.Point(15, 15), Width = 100 };
            var cbСчет = new ComboBox
            {
                Location = new System.Drawing.Point(15, 35),
                Width = 410,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = счета,
                DisplayMember = "Описание",
                ValueMember = "id"
            };

            var lblСумма = new Label
            { Text = "Сумма оплаты:", Location = new System.Drawing.Point(15, 75), Width = 200 };
            var numСумма = new NumericUpDown
            {
                Location = new System.Drawing.Point(15, 95),
                Width = 200,
                Minimum = 0,
                Maximum = 100000000,
                DecimalPlaces = 2,
                Increment = 100m,
                ThousandsSeparator = true
            };

            var lblДата = new Label
            { Text = "Дата оплаты:", Location = new System.Drawing.Point(230, 75), Width = 200 };
            var dtДата = new DateTimePicker
            {
                Location = new System.Drawing.Point(230, 95),
                Width = 195,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };

            var btnOK = new Button
            {
                Text = "Сохранить",
                Location = new System.Drawing.Point(120, 180),
                Width = 100,
                DialogResult = DialogResult.OK
            };
            var btnCancel = new Button
            {
                Text = "Отмена",
                Location = new System.Drawing.Point(230, 180),
                Width = 100,
                DialogResult = DialogResult.Cancel
            };

            dlg.Controls.AddRange(new Control[] {
                lblСчет, cbСчет,
                lblСумма, numСумма,
                lblДата, dtДата,
                btnOK, btnCancel
            });
            dlg.AcceptButton = btnOK;
            dlg.CancelButton = btnCancel;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (cbСчет.SelectedValue == null)
                {
                    MessageBox.Show("Выберите продажу!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (numСумма.Value <= 0)
                {
                    MessageBox.Show("Сумма оплаты должна быть больше нуля!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    repo.Add(
                        Convert.ToInt32(cbСчет.SelectedValue),
                        numСумма.Value,
                        dtДата.Value.Date
                    );
                    MessageBox.Show("Оплата добавлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Загрузить();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения:\n{ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnУдалить_Click(object sender, EventArgs e)
        {
            if (dgvОплаты.CurrentRow == null)
            {
                MessageBox.Show("Выберите оплату для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvОплаты.CurrentRow.Cells["Id"].Value);

            if (MessageBox.Show("Удалить эту оплату?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                repo.Delete(id);
                MessageBox.Show("Оплата удалена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Загрузить();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnОбновить_Click(object sender, EventArgs e) => Загрузить();
    }
}
