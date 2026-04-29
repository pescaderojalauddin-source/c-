using System;
using System.Windows.Forms;
using AccountingApp.Models;
using AccountingApp.Repositories;

namespace AccountingApp
{
    public partial class FormТовары : Form
    {
        private ТоварыRepository товарыRepo;

        public FormТовары()
        {
            InitializeComponent();
            товарыRepo = new ТоварыRepository();
        }

        private void FormТовары_Load(object sender, EventArgs e)
        {
            if (!DbConnectionHelper.TestConnection())
            {
                MessageBox.Show("❌ Не удалось подключиться к базе данных!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            НастройкаDataGridView();
            ЗагрузитьТовары();
        }

        private void НастройкаDataGridView()
        {
            dgvТовары.AutoGenerateColumns = false;
            dgvТовары.Columns.Clear();

            dgvТовары.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdColumn",
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 60,
                ReadOnly = true
            });

            dgvТовары.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NameColumn",
                HeaderText = "Название товара",
                DataPropertyName = "Название",
                Width = 400
            });

            dgvТовары.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NdsColumn",
                HeaderText = "Ставка НДС, %",
                DataPropertyName = "СтавкаНДС",
                Width = 120,
                DefaultCellStyle = { Format = "0.00" }
            });

            dgvТовары.AllowUserToAddRows = false;
            dgvТовары.AllowUserToDeleteRows = false;
            dgvТовары.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvТовары.MultiSelect = false;
        }

        private void ЗагрузитьТовары()
        {
            try
            {
                var товары = товарыRepo.GetAll();
                dgvТовары.DataSource = null;
                dgvТовары.DataSource = товары;
                this.Text = $"Справочник товаров (записей: {товары.Count})";
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при загрузке товаров!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnДобавить_Click(object sender, EventArgs e)
        {
            var форма = new Form
            {
                Text = "Добавление товара",
                Width = 450,
                Height = 220,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblName = new Label { Text = "Название товара:", Location = new System.Drawing.Point(15, 15), Width = 400 };
            var txtName = new TextBox { Location = new System.Drawing.Point(15, 40), Width = 400 };

            var lblNds = new Label { Text = "Ставка НДС (%):", Location = new System.Drawing.Point(15, 75), Width = 400 };
            var txtNds = new TextBox { Location = new System.Drawing.Point(15, 100), Width = 400, Text = "20.00" };

            var btnOK = new Button { Text = "Добавить", Location = new System.Drawing.Point(130, 140), Width = 100, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(240, 140), Width = 100, DialogResult = DialogResult.Cancel };

            форма.Controls.AddRange(new Control[] { lblName, txtName, lblNds, txtNds, btnOK, btnCancel });
            форма.AcceptButton = btnOK;
            форма.CancelButton = btnCancel;

            if (форма.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Название товара не может быть пустым!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!decimal.TryParse(txtNds.Text, out decimal nds) || nds < 0)
                {
                    MessageBox.Show("Введите корректную ставку НДС (число)!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    товарыRepo.Add(txtName.Text.Trim(), nds);
                    MessageBox.Show("✅ Товар добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьТовары();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при добавлении!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnИзменить_Click(object sender, EventArgs e)
        {
            if (dgvТовары.CurrentRow == null)
            {
                MessageBox.Show("Выберите товар для изменения!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvТовары.CurrentRow.Cells["IdColumn"].Value);
            string название = dgvТовары.CurrentRow.Cells["NameColumn"].Value.ToString();
            decimal ставка = Convert.ToDecimal(dgvТовары.CurrentRow.Cells["NdsColumn"].Value);

            var форма = new Form
            {
                Text = "Изменение товара",
                Width = 450,
                Height = 220,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblName = new Label { Text = "Название товара:", Location = new System.Drawing.Point(15, 15), Width = 400 };
            var txtName = new TextBox { Location = new System.Drawing.Point(15, 40), Width = 400, Text = название };

            var lblNds = new Label { Text = "Ставка НДС (%):", Location = new System.Drawing.Point(15, 75), Width = 400 };
            var txtNds = new TextBox { Location = new System.Drawing.Point(15, 100), Width = 400, Text = ставка.ToString("0.00") };

            var btnOK = new Button { Text = "Сохранить", Location = new System.Drawing.Point(130, 140), Width = 100, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(240, 140), Width = 100, DialogResult = DialogResult.Cancel };

            форма.Controls.AddRange(new Control[] { lblName, txtName, lblNds, txtNds, btnOK, btnCancel });
            форма.AcceptButton = btnOK;
            форма.CancelButton = btnCancel;

            if (форма.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Название не может быть пустым!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!decimal.TryParse(txtNds.Text, out decimal nds) || nds < 0)
                {
                    MessageBox.Show("Введите корректную ставку НДС!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    товарыRepo.Update(id, txtName.Text.Trim(), nds);
                    MessageBox.Show("✅ Товар изменён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьТовары();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при изменении!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnУдалить_Click(object sender, EventArgs e)
        {
            if (dgvТовары.CurrentRow == null)
            {
                MessageBox.Show("Выберите товар для удаления!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvТовары.CurrentRow.Cells["IdColumn"].Value);
            string название = dgvТовары.CurrentRow.Cells["NameColumn"].Value.ToString();

            if (MessageBox.Show($"Удалить товар \"{название}\"?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    товарыRepo.Delete(id);
                    MessageBox.Show("✅ Товар удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьТовары();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при удалении!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnОбновить_Click(object sender, EventArgs e) => ЗагрузитьТовары();
    }
}