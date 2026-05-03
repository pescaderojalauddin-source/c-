using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AccountingApp.Models;
using AccountingApp.Repositories;
using Npgsql;

namespace AccountingApp
{
    /// <summary>
    /// Форма «Оплаты клиента» — drill-down:
    ///   ComboBox клиента -> грид его продаж -> позиции + оплаты выбранной продажи.
    /// </summary>
    public partial class FormОплатыКлиента : Form
    {
        private readonly ПродажиRepository продажиRepo = new ПродажиRepository();
        private readonly ОплатыRepository  оплатыRepo  = new ОплатыRepository();

        public FormОплатыКлиента()
        {
            InitializeComponent();
        }

        private void FormОплатыКлиента_Load(object sender, EventArgs e)
        {
            if (!DbConnectionHelper.TestConnection())
            {
                MessageBox.Show("Нет подключения к БД!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ДобавитьКолонки();
            ЗагрузитьКлиентов();
        }

        private void ДобавитьКолонки()
        {
            // ----- Продажи клиента -----
            dgvПродажи.Columns.Clear();
            dgvПродажи.Columns.Add(NewCol("Id",       "ID",        "Id",        50,  visible: false));
            dgvПродажи.Columns.Add(NewCol("Номер",    "№ продажи", "Id",        90));
            dgvПродажи.Columns.Add(NewDateCol("Дата", "Дата",      "Дата",      110));
            dgvПродажи.Columns.Add(NewMoneyCol("Сумма",    "Сумма",    "Сумма",    120));
            dgvПродажи.Columns.Add(NewMoneyCol("Оплачено", "Оплачено", "Оплачено", 120));
            dgvПродажи.Columns.Add(NewMoneyCol("Долг",     "Долг",     "Долг",     120));
            dgvПродажи.Columns.Add(NewCol("Статус",  "Статус",    "Статус",    130));

            // ----- Позиции -----
            dgvПозиции.Columns.Clear();
            dgvПозиции.Columns.Add(NewCol("Товар",      "Товар",  "Товар",      230));
            dgvПозиции.Columns.Add(NewCol("Количество", "Кол-во", "Количество",  70));
            dgvПозиции.Columns.Add(NewMoneyCol("Цена",  "Цена",   "Цена",        90));
            dgvПозиции.Columns.Add(NewMoneyCol("Сумма", "Сумма",  "Сумма",      100));

            // ----- Оплаты -----
            dgvОплаты.Columns.Clear();
            dgvОплаты.Columns.Add(NewCol("Id",         "ID",          "Id",     50, visible: false));
            dgvОплаты.Columns.Add(NewDateCol("Дата",   "Дата оплаты", "Дата",  140));
            dgvОплаты.Columns.Add(NewMoneyCol("Сумма", "Сумма",       "Сумма", 160));
        }

        private static DataGridViewTextBoxColumn NewCol(string name, string header, string prop, int width, bool visible = true)
            => new DataGridViewTextBoxColumn
            {
                Name = name, HeaderText = header, DataPropertyName = prop,
                Width = width, Visible = visible,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(6, 0, 6, 0) }
            };

        private static DataGridViewTextBoxColumn NewMoneyCol(string name, string header, string prop, int width)
            => new DataGridViewTextBoxColumn
            {
                Name = name, HeaderText = header, DataPropertyName = prop, Width = width,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N2",
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Padding = new Padding(6, 0, 8, 0)
                }
            };

        private static DataGridViewTextBoxColumn NewDateCol(string name, string header, string prop, int width)
            => new DataGridViewTextBoxColumn
            {
                Name = name, HeaderText = header, DataPropertyName = prop, Width = width,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd.MM.yyyy",
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

        private void ЗагрузитьКлиентов()
        {
            var dt = new DataTable();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "SELECT id, \"Название\" FROM \"Клиенты\" ORDER BY \"Название\"", conn))
                using (var da = new NpgsqlDataAdapter(cmd))
                    da.Fill(dt);
            }

            cmbКлиент.DataSource = dt;
            cmbКлиент.DisplayMember = "Название";
            cmbКлиент.ValueMember = "id";

            if (dt.Rows.Count > 0)
            {
                cmbКлиент.SelectedIndex = 0;
                ЗагрузитьПродажи();
            }
            else
            {
                lblИтог.Text = "В базе нет клиентов.";
            }
        }

        private void cmbКлиент_SelectedIndexChanged(object sender, EventArgs e)
        {
            ЗагрузитьПродажи();
        }

        private void ЗагрузитьПродажи()
        {
            try
            {
                if (cmbКлиент.SelectedValue == null) return;
                if (!int.TryParse(cmbКлиент.SelectedValue.ToString(), out int id)) return;

                var продажи = продажиRepo.GetByКлиент(id);
                dgvПродажи.DataSource = null;
                dgvПродажи.DataSource = продажи;

                ПодсветитьСтрокиПродаж();
                ОбновитьИтог(продажи);
                ОчиститьНижниеГриды();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продаж клиента:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ПодсветитьСтрокиПродаж()
        {
            foreach (DataGridViewRow row in dgvПродажи.Rows)
            {
                var статус = row.Cells["Статус"].Value?.ToString();
                switch (статус)
                {
                    case "Просрочен":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 234, 234);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(160, 0, 0);
                        break;
                    case "Оплачен":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(27, 94, 32);
                        break;
                    case "Частично":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 225);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(133, 100, 4);
                        break;
                }
            }
        }

        private void ОбновитьИтог(List<Прода> продажи)
        {
            decimal всего    = продажи.Sum(p => p.Сумма);
            decimal оплачено = продажи.Sum(p => p.Оплачено);
            decimal долг     = всего - оплачено;
            lblИтог.Text =
                $"Продаж: {продажи.Count}    "
                + $"|    Сумма: {всего:N2} ₽    "
                + $"|    Оплачено: {оплачено:N2} ₽    "
                + $"|    Долг: {долг:N2} ₽";
        }

        private void ОчиститьНижниеГриды()
        {
            dgvПозиции.DataSource = null;
            dgvОплаты.DataSource  = null;
        }

        private void dgvПродажи_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvПродажи.CurrentRow == null || dgvПродажи.CurrentRow.Cells["Id"].Value == null)
            {
                ОчиститьНижниеГриды();
                return;
            }

            int idProdaji = Convert.ToInt32(dgvПродажи.CurrentRow.Cells["Id"].Value);

            try
            {
                var позиции = продажиRepo.GetПозиции(idProdaji);
                dgvПозиции.DataSource = null;
                dgvПозиции.DataSource = позиции;

                var оплаты = оплатыRepo.GetByProdaja(idProdaji);
                dgvОплаты.DataSource = null;
                dgvОплаты.DataSource = оплаты;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных продажи:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
