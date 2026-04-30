using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using AccountingApp.Models;
using AccountingApp.Repositories;

namespace AccountingApp
{
    public partial class FormОтгрузки : Form
    {
        private ОтгрузкиRepository repo;

        public FormОтгрузки()
        {
            InitializeComponent();
            repo = new ОтгрузкиRepository();
        }

        private void FormОтгрузки_Load(object sender, EventArgs e)
        {
            if (!DbConnectionHelper.TestConnection())
            {
                MessageBox.Show("Нет подключения к БД!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            НастройкаDataGridView();
            Загрузить();
        }

        private void НастройкаDataGridView()
        {
            dgvОтгрузки.AutoGenerateColumns = false;
            dgvОтгрузки.Columns.Clear();

            // ✅ ПРАВИЛЬНЫЙ СПОСОБ: создаём колонки через new
            var colId = new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", DataPropertyName = "Id", Width = 40, Visible = false };
            var colСчет = new DataGridViewTextBoxColumn { Name = "Счет", HeaderText = "Счёт №", DataPropertyName = "НомерСчета", Width = 80 };
            var colТовар = new DataGridViewTextBoxColumn { Name = "Товар", HeaderText = "Товар", DataPropertyName = "Товар", Width = 250 };
            var colСклад = new DataGridViewTextBoxColumn { Name = "Склад", HeaderText = "Склад", DataPropertyName = "Склад", Width = 200 };
            var colКол = new DataGridViewTextBoxColumn { Name = "Кол", HeaderText = "Кол-во", DataPropertyName = "Количество", Width = 80 };
            var colДата = new DataGridViewTextBoxColumn { Name = "Дата", HeaderText = "Дата", DataPropertyName = "Дата", Width = 100 };

            dgvОтгрузки.Columns.Add(colId);
            dgvОтгрузки.Columns.Add(colСчет);
            dgvОтгрузки.Columns.Add(colТовар);
            dgvОтгрузки.Columns.Add(colСклад);
            dgvОтгрузки.Columns.Add(colКол);
            dgvОтгрузки.Columns.Add(colДата);

            dgvОтгрузки.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvОтгрузки.AllowUserToAddRows = false;
            dgvОтгрузки.AllowUserToDeleteRows = false;
        }

        private void Загрузить()
        {
            try
            {
                var список = repo.GetAll();
                dgvОтгрузки.DataSource = null;
                dgvОтгрузки.DataSource = список;
                this.Text = "Журнал отгрузок (записей: " + список.Count + ")";
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка загрузки отгрузок!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable LoadTable(string query)
        {
            DataTable dt = new DataTable();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var da = new NpgsqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        private void btnДобавить_Click(object sender, EventArgs e)
        {
            DataTable счета = LoadTable(
                "SELECT id, ('Продажа #' || id || ' от ' || to_char(data, 'DD.MM.YYYY')) AS \"Номер\" " +
                "FROM \"prodaja\" ORDER BY data DESC, id DESC");
            DataTable товары = LoadTable("SELECT id, \"Название\" FROM \"Товары\" ORDER BY \"Название\"");
            DataTable склады = LoadTable("SELECT id, \"Название\" FROM \"Склады\" ORDER BY \"Название\"");

            if (счета.Rows.Count == 0)
            {
                MessageBox.Show("Сначала создайте хотя бы одну продажу!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form dlg = new Form
            {
                Text = "Новая отгрузка",
                Width = 400,
                Height = 300,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            ComboBox cbСчет = new ComboBox { Location = new System.Drawing.Point(10, 10), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList, DataSource = счета, DisplayMember = "Номер", ValueMember = "id" };
            ComboBox cbТовар = new ComboBox { Location = new System.Drawing.Point(10, 50), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList, DataSource = товары, DisplayMember = "Название", ValueMember = "id" };
            ComboBox cbСклад = new ComboBox { Location = new System.Drawing.Point(10, 90), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList, DataSource = склады, DisplayMember = "Название", ValueMember = "id" };
            NumericUpDown numQty = new NumericUpDown { Location = new System.Drawing.Point(10, 130), Width = 350, Minimum = 1 };
            Button btnOk = new Button { Text = "Сохранить", DialogResult = DialogResult.OK, Location = new System.Drawing.Point(100, 180) };
            Button btnCancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, Location = new System.Drawing.Point(200, 180) };

            dlg.Controls.AddRange(new Control[] {
                new Label { Text = "Продажа:", Location = new System.Drawing.Point(10, 0) }, cbСчет,
                new Label { Text = "Товар:", Location = new System.Drawing.Point(10, 40) }, cbТовар,
                new Label { Text = "Склад:", Location = new System.Drawing.Point(10, 80) }, cbСклад,
                new Label { Text = "Кол-во:", Location = new System.Drawing.Point(10, 120) }, numQty,
                btnOk, btnCancel
            });
            dlg.AcceptButton = btnOk;
            dlg.CancelButton = btnCancel;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    repo.Add(
                        (int)cbСчет.SelectedValue,
                        (int)cbТовар.SelectedValue,
                        (int)cbСклад.SelectedValue,
                        (int)numQty.Value
                    );
                    Загрузить();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnУдалить_Click(object sender, EventArgs e)
        {
            if (dgvОтгрузки.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvОтгрузки.CurrentRow.Cells["Id"].Value);
            if (MessageBox.Show("Удалить эту запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    repo.Delete(id);
                    Загрузить();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnОбновить_Click(object sender, EventArgs e) => Загрузить();
    }
}