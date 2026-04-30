using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Npgsql;

namespace AccountingApp
{
    public partial class FormДиаграммаНДС : Form
    {
        public FormДиаграммаНДС()
        {
            InitializeComponent();
        }

        private void FormДиаграммаНДС_Load(object sender, EventArgs e)
        {
            if (!DbConnectionHelper.TestConnection())
            {
                MessageBox.Show("Нет подключения к БД!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // По умолчанию — последние 30 дней
            dtpОт.Value = DateTime.Today.AddDays(-30);
            dtpДо.Value = DateTime.Today;

            ПостроитьДиаграмму();
        }

        private void btnПоказать_Click(object sender, EventArgs e)
        {
            if (dtpОт.Value.Date > dtpДо.Value.Date)
            {
                MessageBox.Show("Дата начала не может быть позже даты конца!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ПостроитьДиаграмму();
        }

        private void ПостроитьДиаграмму()
        {
            try
            {
                var данные = ПолучитьДанные(dtpОт.Value.Date, dtpДо.Value.Date);

                chartНДС.Series.Clear();
                var series = new Series("НДС")
                {
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true,
                    Label = "#PERCENT{P1}",
                    LegendText = "#VALX (#VALY{N2} ₽)"
                };

                decimal итог = 0m;
                foreach (var pair in данные)
                {
                    int ставка = pair.Key;
                    decimal сумма = pair.Value;
                    итог += сумма;
                    var pt = series.Points.AddXY($"{ставка}%", (double)сумма);
                    series.Points[pt].LegendText = $"{ставка}% — {сумма:N2} ₽";
                }

                chartНДС.Series.Add(series);

                if (данные.Count == 0)
                {
                    lblИтог.Text = "За выбранный период нет данных";
                }
                else
                {
                    var детали = new List<string>();
                    foreach (var p in данные)
                    {
                        decimal доля = итог == 0 ? 0 : p.Value / итог * 100m;
                        детали.Add($"{p.Key}%: {p.Value:N2} ₽ ({доля:N1}%)");
                    }
                    lblИтог.Text = $"Итого: {итог:N2} ₽   |   " + string.Join("   |   ", детали);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка построения диаграммы:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private SortedDictionary<int, decimal> ПолучитьДанные(DateTime от, DateTime до)
        {
            var dict = new SortedDictionary<int, decimal>();

            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT t.""СтавкаНДС""               AS ""Ставка"",
                           SUM(pi.quantity * pi.price)   AS ""Сумма""
                    FROM ""prodaja_info"" pi
                    JOIN ""Товары""  t ON pi.idproduct = t.id
                    JOIN ""prodaja"" p ON pi.idprodaji = p.id
                    WHERE p.data BETWEEN @от AND @до
                    GROUP BY t.""СтавкаНДС""
                    ORDER BY t.""СтавкаНДС""";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@от", от);
                    cmd.Parameters.AddWithValue("@до", до);
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            int ставка = rd.IsDBNull(0) ? 0 : Convert.ToInt32(rd.GetValue(0));
                            decimal сумма = rd.IsDBNull(1) ? 0m : rd.GetDecimal(1);
                            if (сумма > 0) dict[ставка] = сумма;
                        }
                    }
                }
            }
            return dict;
        }
    }
}
