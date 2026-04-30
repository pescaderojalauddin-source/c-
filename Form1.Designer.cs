namespace AccountingApp
{
    partial class Form1
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
            this.dgvКлиенты   = new System.Windows.Forms.DataGridView();
            this.btnДобавить  = new System.Windows.Forms.Button();
            this.btnИзменить  = new System.Windows.Forms.Button();
            this.btnУдалить   = new System.Windows.Forms.Button();
            this.btnОбновить  = new System.Windows.Forms.Button();
            this.btnТовары    = new System.Windows.Forms.Button();
            this.btnСчета     = new System.Windows.Forms.Button();
            this.btnСклады    = new System.Windows.Forms.Button();
            this.btnОтгрузки  = new System.Windows.Forms.Button();
            this.btnОплаты    = new System.Windows.Forms.Button();
            this.btnОтчёты    = new System.Windows.Forms.Button();
            this.btnОплатыКлиента = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvКлиенты)).BeginInit();
            this.SuspendLayout();

            // dgvКлиенты
            this.dgvКлиенты.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvКлиенты.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvКлиенты.Location = new System.Drawing.Point(16, 15);
            this.dgvКлиенты.Margin = new System.Windows.Forms.Padding(4);
            this.dgvКлиенты.Name = "dgvКлиенты";
            this.dgvКлиенты.RowHeadersWidth = 51;
            this.dgvКлиенты.Size = new System.Drawing.Size(1208, 520);
            this.dgvКлиенты.TabIndex = 0;

            // btnДобавить
            this.btnДобавить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnДобавить.Location = new System.Drawing.Point(16, 553);
            this.btnДобавить.Margin = new System.Windows.Forms.Padding(4);
            this.btnДобавить.Name = "btnДобавить";
            this.btnДобавить.Size = new System.Drawing.Size(110, 37);
            this.btnДобавить.TabIndex = 1;
            this.btnДобавить.Text = "Добавить";
            this.btnДобавить.UseVisualStyleBackColor = true;

            // btnИзменить
            this.btnИзменить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnИзменить.Location = new System.Drawing.Point(136, 553);
            this.btnИзменить.Margin = new System.Windows.Forms.Padding(4);
            this.btnИзменить.Name = "btnИзменить";
            this.btnИзменить.Size = new System.Drawing.Size(110, 37);
            this.btnИзменить.TabIndex = 2;
            this.btnИзменить.Text = "Изменить";
            this.btnИзменить.UseVisualStyleBackColor = true;

            // btnУдалить
            this.btnУдалить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnУдалить.Location = new System.Drawing.Point(256, 553);
            this.btnУдалить.Margin = new System.Windows.Forms.Padding(4);
            this.btnУдалить.Name = "btnУдалить";
            this.btnУдалить.Size = new System.Drawing.Size(110, 37);
            this.btnУдалить.TabIndex = 3;
            this.btnУдалить.Text = "Удалить";
            this.btnУдалить.UseVisualStyleBackColor = true;

            // btnОбновить
            this.btnОбновить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnОбновить.Location = new System.Drawing.Point(376, 553);
            this.btnОбновить.Margin = new System.Windows.Forms.Padding(4);
            this.btnОбновить.Name = "btnОбновить";
            this.btnОбновить.Size = new System.Drawing.Size(110, 37);
            this.btnОбновить.TabIndex = 4;
            this.btnОбновить.Text = "Обновить";
            this.btnОбновить.UseVisualStyleBackColor = true;

            // btnОплатыКлиента (ОПЛАТЫ КЛИЕНТА — новая кнопка)
            this.btnОплатыКлиента.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnОплатыКлиента.Location = new System.Drawing.Point(514, 553);
            this.btnОплатыКлиента.Margin = new System.Windows.Forms.Padding(4);
            this.btnОплатыКлиента.Name = "btnОплатыКлиента";
            this.btnОплатыКлиента.Size = new System.Drawing.Size(120, 37);
            this.btnОплатыКлиента.TabIndex = 11;
            this.btnОплатыКлиента.Text = "Оплаты клиента";
            this.btnОплатыКлиента.UseVisualStyleBackColor = true;
            this.btnОплатыКлиента.BackColor = System.Drawing.Color.LightSkyBlue;

            // btnОтчёты
            this.btnОтчёты.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnОтчёты.Location = new System.Drawing.Point(644, 553);
            this.btnОтчёты.Margin = new System.Windows.Forms.Padding(4);
            this.btnОтчёты.Name = "btnОтчёты";
            this.btnОтчёты.Size = new System.Drawing.Size(110, 37);
            this.btnОтчёты.TabIndex = 5;
            this.btnОтчёты.Text = "Отчёты 📊";
            this.btnОтчёты.UseVisualStyleBackColor = true;
            this.btnОтчёты.BackColor = System.Drawing.Color.Plum;

            // btnОплаты
            this.btnОплаты.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnОплаты.Location = new System.Drawing.Point(764, 553);
            this.btnОплаты.Margin = new System.Windows.Forms.Padding(4);
            this.btnОплаты.Name = "btnОплаты";
            this.btnОплаты.Size = new System.Drawing.Size(110, 37);
            this.btnОплаты.TabIndex = 6;
            this.btnОплаты.Text = "Оплаты 💰";
            this.btnОплаты.UseVisualStyleBackColor = true;
            this.btnОплаты.BackColor = System.Drawing.Color.LightCoral;

            // btnОтгрузки
            this.btnОтгрузки.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnОтгрузки.Location = new System.Drawing.Point(884, 553);
            this.btnОтгрузки.Margin = new System.Windows.Forms.Padding(4);
            this.btnОтгрузки.Name = "btnОтгрузки";
            this.btnОтгрузки.Size = new System.Drawing.Size(110, 37);
            this.btnОтгрузки.TabIndex = 7;
            this.btnОтгрузки.Text = "Отгрузки 🚛";
            this.btnОтгрузки.UseVisualStyleBackColor = true;
            this.btnОтгрузки.BackColor = System.Drawing.Color.Orange;

            // btnСклады
            this.btnСклады.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnСклады.Location = new System.Drawing.Point(1004, 553);
            this.btnСклады.Margin = new System.Windows.Forms.Padding(4);
            this.btnСклады.Name = "btnСклады";
            this.btnСклады.Size = new System.Drawing.Size(110, 37);
            this.btnСклады.TabIndex = 8;
            this.btnСклады.Text = "Склады 🏭";
            this.btnСклады.UseVisualStyleBackColor = true;
            this.btnСклады.BackColor = System.Drawing.Color.LightYellow;

            // btnСчета
            this.btnСчета.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnСчета.Location = new System.Drawing.Point(1124, 553);
            this.btnСчета.Margin = new System.Windows.Forms.Padding(4);
            this.btnСчета.Name = "btnСчета";
            this.btnСчета.Size = new System.Drawing.Size(110, 37);
            this.btnСчета.TabIndex = 9;
            this.btnСчета.Text = "Счета 📄";
            this.btnСчета.UseVisualStyleBackColor = true;
            this.btnСчета.BackColor = System.Drawing.Color.LightGreen;

            // btnТовары
            this.btnТовары.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnТовары.Location = new System.Drawing.Point(1244, 553);
            this.btnТовары.Margin = new System.Windows.Forms.Padding(4);
            this.btnТовары.Name = "btnТовары";
            this.btnТовары.Size = new System.Drawing.Size(110, 37);
            this.btnТовары.TabIndex = 10;
            this.btnТовары.Text = "Товары 📦";
            this.btnТовары.UseVisualStyleBackColor = true;
            this.btnТовары.BackColor = System.Drawing.Color.LightBlue;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 600);
            this.Controls.Add(this.btnТовары);
            this.Controls.Add(this.btnСчета);
            this.Controls.Add(this.btnСклады);
            this.Controls.Add(this.btnОтгрузки);
            this.Controls.Add(this.btnОплаты);
            this.Controls.Add(this.btnОтчёты);
            this.Controls.Add(this.btnОплатыКлиента);
            this.Controls.Add(this.btnОбновить);
            this.Controls.Add(this.btnУдалить);
            this.Controls.Add(this.btnИзменить);
            this.Controls.Add(this.btnДобавить);
            this.Controls.Add(this.dgvКлиенты);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Справочник клиентов";
            ((System.ComponentModel.ISupportInitialize)(this.dgvКлиенты)).EndInit();

            // События
            this.Load                += new System.EventHandler(this.Form1_Load);
            this.btnДобавить.Click   += new System.EventHandler(this.btnДобавить_Click);
            this.btnИзменить.Click   += new System.EventHandler(this.btnИзменить_Click);
            this.btnУдалить.Click    += new System.EventHandler(this.btnУдалить_Click);
            this.btnОбновить.Click   += new System.EventHandler(this.btnОбновить_Click);
            this.btnТовары.Click     += new System.EventHandler(this.btnТовары_Click);
            this.btnСчета.Click      += new System.EventHandler(this.btnСчета_Click);
            this.btnСклады.Click     += new System.EventHandler(this.btnСклады_Click);
            this.btnОтгрузки.Click   += new System.EventHandler(this.btnОтгрузки_Click);
            this.btnОплаты.Click     += new System.EventHandler(this.btnОплаты_Click);
            this.btnОтчёты.Click     += new System.EventHandler(this.btnОтчёты_Click);
            this.btnОплатыКлиента.Click += new System.EventHandler(this.btnОплатыКлиента_Click);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dgvКлиенты;
        private System.Windows.Forms.Button btnДобавить;
        private System.Windows.Forms.Button btnИзменить;
        private System.Windows.Forms.Button btnУдалить;
        private System.Windows.Forms.Button btnОбновить;
        private System.Windows.Forms.Button btnТовары;
        private System.Windows.Forms.Button btnСчета;
        private System.Windows.Forms.Button btnСклады;
        private System.Windows.Forms.Button btnОтгрузки;
        private System.Windows.Forms.Button btnОплаты;
        private System.Windows.Forms.Button btnОтчёты;
        private System.Windows.Forms.Button btnОплатыКлиента;
    }
}
