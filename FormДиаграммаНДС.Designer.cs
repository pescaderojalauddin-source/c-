namespace AccountingApp
{
    partial class FormДиаграммаНДС
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblПериод = new System.Windows.Forms.Label();
            this.dtpОт = new System.Windows.Forms.DateTimePicker();
            this.lblТире = new System.Windows.Forms.Label();
            this.dtpДо = new System.Windows.Forms.DateTimePicker();
            this.btnПоказать = new System.Windows.Forms.Button();
            this.btnЗакрыть = new System.Windows.Forms.Button();
            this.lblИтог = new System.Windows.Forms.Label();
            this.chartНДС = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartНДС)).BeginInit();
            this.SuspendLayout();

            // lblПериод
            this.lblПериод.AutoSize = true;
            this.lblПериод.Location = new System.Drawing.Point(16, 18);
            this.lblПериод.Text = "Период:";

            // dtpОт
            this.dtpОт.Location = new System.Drawing.Point(80, 14);
            this.dtpОт.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpОт.Width = 120;

            // lblТире
            this.lblТире.AutoSize = true;
            this.lblТире.Location = new System.Drawing.Point(206, 18);
            this.lblТире.Text = "—";

            // dtpДо
            this.dtpДо.Location = new System.Drawing.Point(225, 14);
            this.dtpДо.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpДо.Width = 120;

            // btnПоказать
            this.btnПоказать.Location = new System.Drawing.Point(360, 12);
            this.btnПоказать.Size = new System.Drawing.Size(120, 28);
            this.btnПоказать.Text = "Показать";
            this.btnПоказать.BackColor = System.Drawing.Color.LightGreen;
            this.btnПоказать.UseVisualStyleBackColor = false;

            // btnЗакрыть
            this.btnЗакрыть.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnЗакрыть.Location = new System.Drawing.Point(670, 12);
            this.btnЗакрыть.Size = new System.Drawing.Size(110, 28);
            this.btnЗакрыть.Text = "Закрыть";
            this.btnЗакрыть.UseVisualStyleBackColor = true;
            this.btnЗакрыть.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            // chartНДС
            this.chartНДС.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            var chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea("MainArea");
            this.chartНДС.ChartAreas.Add(chartArea);
            var legend = new System.Windows.Forms.DataVisualization.Charting.Legend("MainLegend");
            legend.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Right;
            this.chartНДС.Legends.Add(legend);
            this.chartНДС.Location = new System.Drawing.Point(16, 50);
            this.chartНДС.Name = "chartНДС";
            this.chartНДС.Size = new System.Drawing.Size(764, 400);

            var title = new System.Windows.Forms.DataVisualization.Charting.Title();
            title.Name = "MainTitle";
            title.Text = "Распределение проданных товаров по ставкам НДС";
            title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.chartНДС.Titles.Add(title);

            // lblИтог
            this.lblИтог.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblИтог.AutoSize = true;
            this.lblИтог.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblИтог.Location = new System.Drawing.Point(16, 460);
            this.lblИтог.Text = "Нажмите «Показать» для построения диаграммы";

            // FormДиаграммаНДС
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 490);
            this.Controls.Add(this.lblИтог);
            this.Controls.Add(this.chartНДС);
            this.Controls.Add(this.btnЗакрыть);
            this.Controls.Add(this.btnПоказать);
            this.Controls.Add(this.dtpДо);
            this.Controls.Add(this.lblТире);
            this.Controls.Add(this.dtpОт);
            this.Controls.Add(this.lblПериод);
            this.Name = "FormДиаграммаНДС";
            this.Text = "Диаграмма: НДС за период";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            this.Load += new System.EventHandler(this.FormДиаграммаНДС_Load);
            this.btnПоказать.Click += new System.EventHandler(this.btnПоказать_Click);

            ((System.ComponentModel.ISupportInitialize)(this.chartНДС)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblПериод;
        private System.Windows.Forms.DateTimePicker dtpОт;
        private System.Windows.Forms.Label lblТире;
        private System.Windows.Forms.DateTimePicker dtpДо;
        private System.Windows.Forms.Button btnПоказать;
        private System.Windows.Forms.Button btnЗакрыть;
        private System.Windows.Forms.Label lblИтог;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartНДС;
    }
}
