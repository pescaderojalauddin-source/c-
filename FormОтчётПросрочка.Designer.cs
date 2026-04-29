namespace AccountingApp
{
    partial class FormОтчётПросрочка
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
            this.lblКлиенты = new System.Windows.Forms.Label();
            this.clbКлиенты = new System.Windows.Forms.CheckedListBox();
            this.btnВыбратьВсех = new System.Windows.Forms.Button();
            this.btnСнять = new System.Windows.Forms.Button();
            this.lblДата = new System.Windows.Forms.Label();
            this.dtpНаДату = new System.Windows.Forms.DateTimePicker();
            this.btnСформировать = new System.Windows.Forms.Button();
            this.btnЭкспорт = new System.Windows.Forms.Button();
            this.btnЗакрыть = new System.Windows.Forms.Button();
            this.dgvРезультат = new System.Windows.Forms.DataGridView();
            this.lblИтого = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvРезультат)).BeginInit();
            this.SuspendLayout();

            // lblКлиенты
            this.lblКлиенты.AutoSize = true;
            this.lblКлиенты.Location = new System.Drawing.Point(16, 15);
            this.lblКлиенты.Text = "Клиенты:";

            // clbКлиенты
            this.clbКлиенты.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
            this.clbКлиенты.Location = new System.Drawing.Point(16, 35);
            this.clbКлиенты.Name = "clbКлиенты";
            this.clbКлиенты.Size = new System.Drawing.Size(280, 410);
            this.clbКлиенты.CheckOnClick = true;
            this.clbКлиенты.TabIndex = 0;

            // btnВыбратьВсех
            this.btnВыбратьВсех.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnВыбратьВсех.Location = new System.Drawing.Point(16, 455);
            this.btnВыбратьВсех.Name = "btnВыбратьВсех";
            this.btnВыбратьВсех.Size = new System.Drawing.Size(135, 30);
            this.btnВыбратьВсех.Text = "Выбрать всех";
            this.btnВыбратьВсех.UseVisualStyleBackColor = true;

            // btnСнять
            this.btnСнять.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnСнять.Location = new System.Drawing.Point(161, 455);
            this.btnСнять.Name = "btnСнять";
            this.btnСнять.Size = new System.Drawing.Size(135, 30);
            this.btnСнять.Text = "Снять выделение";
            this.btnСнять.UseVisualStyleBackColor = true;

            // lblДата
            this.lblДата.AutoSize = true;
            this.lblДата.Location = new System.Drawing.Point(316, 15);
            this.lblДата.Text = "На дату:";

            // dtpНаДату
            this.dtpНаДату.Location = new System.Drawing.Point(316, 35);
            this.dtpНаДату.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpНаДату.Width = 130;

            // btnСформировать
            this.btnСформировать.Location = new System.Drawing.Point(456, 33);
            this.btnСформировать.Size = new System.Drawing.Size(140, 28);
            this.btnСформировать.Text = "Сформировать";
            this.btnСформировать.BackColor = System.Drawing.Color.LightGreen;
            this.btnСформировать.UseVisualStyleBackColor = false;

            // btnЭкспорт
            this.btnЭкспорт.Location = new System.Drawing.Point(606, 33);
            this.btnЭкспорт.Size = new System.Drawing.Size(160, 28);
            this.btnЭкспорт.Text = "📥 Экспорт в Excel";
            this.btnЭкспорт.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnЭкспорт.UseVisualStyleBackColor = false;

            // btnЗакрыть
            this.btnЗакрыть.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnЗакрыть.Location = new System.Drawing.Point(776, 455);
            this.btnЗакрыть.Size = new System.Drawing.Size(110, 30);
            this.btnЗакрыть.Text = "Закрыть";
            this.btnЗакрыть.UseVisualStyleBackColor = true;
            this.btnЗакрыть.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            // dgvРезультат
            this.dgvРезультат.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvРезультат.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvРезультат.Location = new System.Drawing.Point(316, 75);
            this.dgvРезультат.Name = "dgvРезультат";
            this.dgvРезультат.RowHeadersWidth = 51;
            this.dgvРезультат.Size = new System.Drawing.Size(580, 370);
            this.dgvРезультат.AllowUserToAddRows = false;
            this.dgvРезультат.AllowUserToDeleteRows = false;
            this.dgvРезультат.ReadOnly = true;
            this.dgvРезультат.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // lblИтого
            this.lblИтого.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblИтого.AutoSize = true;
            this.lblИтого.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblИтого.Location = new System.Drawing.Point(316, 460);
            this.lblИтого.Text = "Итого долг: 0,00 ₽";

            // FormОтчётПросрочка
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 500);
            this.Controls.Add(this.lblИтого);
            this.Controls.Add(this.dgvРезультат);
            this.Controls.Add(this.btnЗакрыть);
            this.Controls.Add(this.btnЭкспорт);
            this.Controls.Add(this.btnСформировать);
            this.Controls.Add(this.dtpНаДату);
            this.Controls.Add(this.lblДата);
            this.Controls.Add(this.btnСнять);
            this.Controls.Add(this.btnВыбратьВсех);
            this.Controls.Add(this.clbКлиенты);
            this.Controls.Add(this.lblКлиенты);
            this.Name = "FormОтчётПросрочка";
            this.Text = "Отчёт: Просроченные платежи";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            // События
            this.Load += new System.EventHandler(this.FormОтчётПросрочка_Load);
            this.btnВыбратьВсех.Click += new System.EventHandler(this.btnВыбратьВсех_Click);
            this.btnСнять.Click += new System.EventHandler(this.btnСнять_Click);
            this.btnСформировать.Click += new System.EventHandler(this.btnСформировать_Click);
            this.btnЭкспорт.Click += new System.EventHandler(this.btnЭкспорт_Click);

            ((System.ComponentModel.ISupportInitialize)(this.dgvРезультат)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblКлиенты;
        private System.Windows.Forms.CheckedListBox clbКлиенты;
        private System.Windows.Forms.Button btnВыбратьВсех;
        private System.Windows.Forms.Button btnСнять;
        private System.Windows.Forms.Label lblДата;
        private System.Windows.Forms.DateTimePicker dtpНаДату;
        private System.Windows.Forms.Button btnСформировать;
        private System.Windows.Forms.Button btnЭкспорт;
        private System.Windows.Forms.Button btnЗакрыть;
        private System.Windows.Forms.DataGridView dgvРезультат;
        private System.Windows.Forms.Label lblИтого;
    }
}
