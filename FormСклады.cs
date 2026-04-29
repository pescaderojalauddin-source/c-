using System;
using System.Windows.Forms;
using AccountingApp.Models;
using AccountingApp.Repositories;

namespace AccountingApp
{
    public partial class FormСклады : Form
    {
        private СкладыRepository складыRepo;

        public FormСклады()
        {
            InitializeComponent();
            складыRepo = new СкладыRepository();
        }

        private void FormСклады_Load(object sender, EventArgs e)
        {
            if (!DbConnectionHelper.TestConnection())
            {
                MessageBox.Show("❌ Не удалось подключиться к базе данных!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            НастройкаDataGridView();
            ЗагрузитьСклады();
        }

        private void НастройкаDataGridView()
        {
            dgvСклады.AutoGenerateColumns = false;
            dgvСклады.Columns.Clear();

            dgvСклады.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdColumn",
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 60,
                ReadOnly = true,
                Visible = false
            });

            dgvСклады.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NameColumn",
                HeaderText = "Название склада",
                DataPropertyName = "Название",
                Width = 500
            });

            dgvСклады.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "FabColumn",
                HeaderText = "Фабричный",
                DataPropertyName = "Фабричный",
                Width = 120,
                TrueValue = true,
                FalseValue = false,
                IndeterminateValue = false
            });

            dgvСклады.AllowUserToAddRows = false;
            dgvСклады.AllowUserToDeleteRows = false;
            dgvСклады.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvСклады.MultiSelect = false;
        }

        private void ЗагрузитьСклады()
        {
            try
            {
                var склады = складыRepo.GetAll();
                dgvСклады.DataSource = null;
                dgvСклады.DataSource = склады;
                this.Text = $"Справочник складов (записей: {склады.Count})";
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при загрузке складов!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnДобавить_Click(object sender, EventArgs e)
        {
            var форма = new Form
            {
                Text = "Добавление склада",
                Width = 450,
                Height = 220,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblName = new Label { Text = "Название склада:", Location = new System.Drawing.Point(15, 15), Width = 400 };
            var txtName = new TextBox { Location = new System.Drawing.Point(15, 40), Width = 400 };

            var chkFab = new CheckBox { Text = "Это фабричный склад?", Location = new System.Drawing.Point(15, 80), AutoSize = true };

            var btnOK = new Button { Text = "Добавить", Location = new System.Drawing.Point(130, 120), Width = 100, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(240, 120), Width = 100, DialogResult = DialogResult.Cancel };

            форма.Controls.AddRange(new Control[] { lblName, txtName, chkFab, btnOK, btnCancel });
            форма.AcceptButton = btnOK;
            форма.CancelButton = btnCancel;

            if (форма.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Название склада не может быть пустым!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    складыRepo.Add(txtName.Text.Trim(), chkFab.Checked);
                    MessageBox.Show("✅ Склад добавлен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьСклады();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при добавлении!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnИзменить_Click(object sender, EventArgs e)
        {
            if (dgvСклады.CurrentRow == null)
            {
                MessageBox.Show("Выберите склад для изменения!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvСклады.CurrentRow.Cells["IdColumn"].Value);
            string название = dgvСклады.CurrentRow.Cells["NameColumn"].Value.ToString();
            bool фабричный = Convert.ToBoolean(dgvСклады.CurrentRow.Cells["FabColumn"].Value);

            var форма = new Form
            {
                Text = "Изменение склада",
                Width = 450,
                Height = 220,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblName = new Label { Text = "Название склада:", Location = new System.Drawing.Point(15, 15), Width = 400 };
            var txtName = new TextBox { Location = new System.Drawing.Point(15, 40), Width = 400, Text = название };

            var chkFab = new CheckBox { Text = "Это фабричный склад?", Location = new System.Drawing.Point(15, 80), AutoSize = true, Checked = фабричный };

            var btnOK = new Button { Text = "Сохранить", Location = new System.Drawing.Point(130, 120), Width = 100, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(240, 120), Width = 100, DialogResult = DialogResult.Cancel };

            форма.Controls.AddRange(new Control[] { lblName, txtName, chkFab, btnOK, btnCancel });
            форма.AcceptButton = btnOK;
            форма.CancelButton = btnCancel;

            if (форма.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Название не может быть пустым!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    складыRepo.Update(id, txtName.Text.Trim(), chkFab.Checked);
                    MessageBox.Show("✅ Склад изменён!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьСклады();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при изменении!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnУдалить_Click(object sender, EventArgs e)
        {
            if (dgvСклады.CurrentRow == null)
            {
                MessageBox.Show("Выберите склад для удаления!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvСклады.CurrentRow.Cells["IdColumn"].Value);
            string название = dgvСклады.CurrentRow.Cells["NameColumn"].Value.ToString();

            if (MessageBox.Show($"Удалить склад \"{название}\"?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    складыRepo.Delete(id);
                    MessageBox.Show("✅ Склад удалён!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьСклады();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при удалении!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnОбновить_Click(object sender, EventArgs e) => ЗагрузитьСклады();
    }
}