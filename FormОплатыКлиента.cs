using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly ОплатыRepository оплатыRepo  = new ОплатыRepository();

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

            НастроитьГриды();
            ЗагрузитьКлиентов();
        }

        private void НастроитьГриды()
        {
            // Продажи клиента
            dgvПродажи.AutoGenerateColumns = false;
            dgvПродажи.Columns.Clear();
            dgvПродажи.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Id", HeaderText = "ID", DataPropertyName = "Id", Visible = false });
            dgvПродажи.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Номер", HeaderText = "№", DataPropertyName = "Id", Width = 50 });
            dgvПродажи.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Дата", HeaderText = "Дата", DataPropertyName = "Дата", Width = 100, DefaultCellStyle = { Format = "d" } });
            dgvПродажи.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Сумма", HeaderText = "Сумма", DataPropertyName = "Сумма", Width = 110, DefaultCellStyle = { Format = "N2" } });
            dgvПродажи.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Оплачено", HeaderText = "Оплачено", DataPropertyName = "Оплачено", Width = 110, DefaultCellStyle = { Format = "N2" } });
            dgvПродажи.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Долг", HeaderText = "Долг", DataPropertyName = "Долг", Width = 110, DefaultCellStyle = { Format = "N2" } });
            dgvПродажи.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Статус", HeaderText = "Статус", DataPropertyName = "Статус", Width = 110 });
            dgvПродажи.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvПродажи.MultiSelect = false;

            // Позиции
            dgvПозиции.AutoGenerateColumns = false;
            dgvПозиции.Columns.Clear();
            dgvПозиции.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Товар", HeaderText = "Товар", DataPropertyName = "Товар", Width = 200 });
            dgvПозиции.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Кол-во", HeaderText = "Кол-во", DataPropertyName = "Количество", Width = 60 });
            dgvПозиции.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Цена", HeaderText = "Цена", DataPropertyName = "Цена", Width = 70, DefaultCellStyle = { Format = "N2" } });
            dgvПозиции.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Сумма", HeaderText = "Сумма", DataPropertyName = "Сумма", Width = 80, DefaultCellStyle = { Format = "N2" } });
            dgvПозиции.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Оплаты по продаже
            dgvОплаты.AutoGenerateColumns = false;
            dgvОплаты.Columns.Clear();
            dgvОплаты.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Id", HeaderText = "ID", DataPropertyName = "Id", Visible = false });
            dgvОплаты.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Дата", HeaderText = "Дата оплаты", DataPropertyName = "Дата", Width = 130, DefaultCellStyle = { Format = "d" } });
            dgvОплаты.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "Сумма", HeaderText = "Сумма", DataPropertyName = "Сумма", Width = 130, DefaultCellStyle = { Format = "N2" } });
            dgvОплаты.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

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
        }

        private void cmbКлиент_SelectedIndexChanged(object sender, EventArgs e)
        {
            ЗагрузитьПродажи();
        }

        private void ЗагрузитьПродажи()
        {
            try
            {
                if (cmbКлиент.SelectedValue == null ||
                    !(cmbКлиент.SelectedValue is int) && !int.TryParse(cmbКлиент.SelectedValue.ToString(), out _))
                    return;

                int id = Convert.ToInt32(cmbКлиент.SelectedValue);
                var продажи = продажиRepo.GetByКлиент(id);
                dgvПродажи.DataSource = null;
                dgvПродажи.DataSource = продажи;

                ОбновитьИтог(продажи);
                ОчиститьНижниеГриды();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продаж клиента:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ОбновитьИтог(List<Прода> продажи)
        {
            decimal всего    = продажи.Sum(p => p.Сумма);
            decimal оплачено = продажи.Sum(p => p.Оплачено);
            decimal долг     = всего - оплачено;
            lblИтог.Text = $"Итог по клиенту: всего {всего:N2} ₽   |   оплачено {оплачено:N2} ₽   |   долг {долг:N2} ₽   |   продаж: {продажи.Count}";
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
