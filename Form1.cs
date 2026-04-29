using System;
using System.Windows.Forms;
using AccountingApp.Models;
using AccountingApp.Repositories;

namespace AccountingApp
{

    public partial class Form1 : Form
    {


        private КлиентыRepository клиентыRepo;

        public Form1()
        {
            InitializeComponent();
            клиентыRepo = new КлиентыRepository();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Проверяем подключение
            if (!DbConnectionHelper.TestConnection())
            {
                MessageBox.Show("❌ Не удалось подключиться к базе данных!\n\nПроверьте:\n- Запущен ли Docker\n- Работает ли контейнер my_postgres",
                    "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Настраиваем таблицу
            НастройкаDataGridView();

            // Загружаем данные
            ЗагрузитьКлиентов();
        }

        private void btnСчета_Click(object sender, EventArgs e)
        {
            new FormСчета().ShowDialog();
        }

        private void НастройкаDataGridView()
        {
            dgvКлиенты.AutoGenerateColumns = false;
            dgvКлиенты.Columns.Clear();

            dgvКлиенты.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdColumn",
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 60,
                ReadOnly = true
            });

            dgvКлиенты.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NameColumn",
                HeaderText = "Название клиента",
                DataPropertyName = "Название",
                Width = 500
            });

            dgvКлиенты.AllowUserToAddRows = false;
            dgvКлиенты.AllowUserToDeleteRows = false;
            dgvКлиенты.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvКлиенты.MultiSelect = false;
            dgvКлиенты.ReadOnly = false;
        }

        private void ЗагрузитьКлиентов()
        {
            try
            {
                var клиенты = клиентыRepo.GetAll();
                dgvКлиенты.DataSource = null;
                dgvКлиенты.DataSource = клиенты;
                this.Text = $"Справочник клиентов (записей: {клиенты.Count})";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnДобавить_Click(object sender, EventArgs e)
        {
            var форма = new Form
            {
                Text = "Добавление клиента",
                Width = 450,
                Height = 180,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var label = new Label
            {
                Text = "Введите название клиента:",
                Location = new System.Drawing.Point(15, 15),
                Width = 400,
                Height = 20
            };

            var textBox = new TextBox
            {
                Location = new System.Drawing.Point(15, 45),
                Width = 400
            };

            var btnOK = new Button
            {
                Text = "Добавить",
                Location = new System.Drawing.Point(150, 85),
                Width = 100,
                DialogResult = DialogResult.OK
            };

            var btnCancel = new Button
            {
                Text = "Отмена",
                Location = new System.Drawing.Point(260, 85),
                Width = 100,
                DialogResult = DialogResult.Cancel
            };

            форма.Controls.Add(label);
            форма.Controls.Add(textBox);
            форма.Controls.Add(btnOK);
            форма.Controls.Add(btnCancel);
            форма.AcceptButton = btnOK;
            форма.CancelButton = btnCancel;

            if (форма.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    MessageBox.Show("Название клиента не может быть пустым!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    клиентыRepo.Add(textBox.Text.Trim());
                    MessageBox.Show("✅ Клиент успешно добавлен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьКлиентов();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении:\n{ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnИзменить_Click(object sender, EventArgs e)
        {
            if (dgvКлиенты.CurrentRow == null)
            {
                MessageBox.Show("Выберите клиента для изменения!",
                    "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvКлиенты.CurrentRow.Cells["IdColumn"].Value);
            string староеНазвание = dgvКлиенты.CurrentRow.Cells["NameColumn"].Value.ToString();

            var форма = new Form
            {
                Text = "Изменение клиента",
                Width = 450,
                Height = 180,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var label = new Label
            {
                Text = "Введите новое название:",
                Location = new System.Drawing.Point(15, 15),
                Width = 400,
                Height = 20
            };

            var textBox = new TextBox
            {
                Location = new System.Drawing.Point(15, 45),
                Width = 400,
                Text = староеНазвание
            };

            var btnOK = new Button
            {
                Text = "Сохранить",
                Location = new System.Drawing.Point(150, 85),
                Width = 100,
                DialogResult = DialogResult.OK
            };

            var btnCancel = new Button
            {
                Text = "Отмена",
                Location = new System.Drawing.Point(260, 85),
                Width = 100,
                DialogResult = DialogResult.Cancel
            };

            форма.Controls.Add(label);
            форма.Controls.Add(textBox);
            форма.Controls.Add(btnOK);
            форма.Controls.Add(btnCancel);
            форма.AcceptButton = btnOK;
            форма.CancelButton = btnCancel;

            if (форма.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    MessageBox.Show("Название клиента не может быть пустым!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    клиентыRepo.Update(id, textBox.Text.Trim());
                    MessageBox.Show("✅ Клиент успешно изменён!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьКлиентов();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при изменении:\n{ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnУдалить_Click(object sender, EventArgs e)
        {
            if (dgvКлиенты.CurrentRow == null)
            {
                MessageBox.Show("Выберите клиента для удаления!",
                    "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvКлиенты.CurrentRow.Cells["IdColumn"].Value);
            string название = dgvКлиенты.CurrentRow.Cells["NameColumn"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить клиента?\n\n\"{название}\"\n\n" +
                $"ВНИМАНИЕ: Если у клиента есть счета, они тоже будут удалены!",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    клиентыRepo.Delete(id);
                    MessageBox.Show("✅ Клиент успешно удалён!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ЗагрузитьКлиентов();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении:\n{ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnОбновить_Click(object sender, EventArgs e)
        {
            ЗагрузитьКлиентов();
            MessageBox.Show("🔄 Данные клиентов обновлены!", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnТовары_Click(object sender, EventArgs e)
        {
            FormТовары формаТовары = new FormТовары();
            формаТовары.ShowDialog();
        }

        private void btnСклады_Click(object sender, EventArgs e)
        {
            new FormСклады().ShowDialog();
        }

        private void btnОтгрузки_Click(object sender, EventArgs e)
        {
            new FormОтгрузки().ShowDialog();
        }

        private void btnОплаты_Click(object sender, EventArgs e)
        {
            new FormОплаты().ShowDialog();
        }

        private void btnОтчёты_Click(object sender, EventArgs e)
        {
            new FormОтчёты().ShowDialog();
        }
    }
}