namespace AccountingApp
{
    partial class FormСчета
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvСчета = new System.Windows.Forms.DataGridView();
            this.dgvПозиции = new System.Windows.Forms.DataGridView();
            this.btnДобавитьСчет = new System.Windows.Forms.Button();
            this.btnУдалитьСчет = new System.Windows.Forms.Button();
            this.btnЭкспортСчета = new System.Windows.Forms.Button();
            this.btnДобавитьПозицию = new System.Windows.Forms.Button();
            this.btnУдалитьПозицию = new System.Windows.Forms.Button();
            this.lblСчета = new System.Windows.Forms.Label();
            this.lblПозиции = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvСчета)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvПозиции)).BeginInit();
            this.SuspendLayout();

            // dgvСчета
            this.dgvСчета.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvСчета.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvСчета.Location = new System.Drawing.Point(16, 40);
            this.dgvСчета.Name = "dgvСчета";
            this.dgvСчета.Size = new System.Drawing.Size(950, 250);
            this.dgvСчета.TabIndex = 0;
            this.dgvСчета.SelectionChanged += new System.EventHandler(this.dgvСчета_SelectionChanged);

            // dgvПозиции
            this.dgvПозиции.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvПозиции.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvПозиции.Location = new System.Drawing.Point(16, 330);
            this.dgvПозиции.Name = "dgvПозиции";
            this.dgvПозиции.Size = new System.Drawing.Size(950, 200);
            this.dgvПозиции.TabIndex = 1;

            // btnДобавитьСчет
            this.btnДобавитьСчет.Location = new System.Drawing.Point(16, 8);
            this.btnДобавитьСчет.Name = "btnДобавитьСчет";
            this.btnДобавитьСчет.Size = new System.Drawing.Size(120, 25);
            this.btnДобавитьСчет.Text = "Добавить счёт";
            this.btnДобавитьСчет.Click += new System.EventHandler(this.btnДобавитьСчет_Click);

            // btnУдалитьСчет
            this.btnУдалитьСчет.Location = new System.Drawing.Point(145, 8);
            this.btnУдалитьСчет.Name = "btnУдалитьСчет";
            this.btnУдалитьСчет.Size = new System.Drawing.Size(120, 25);
            this.btnУдалитьСчет.Text = "Удалить счёт";
            this.btnУдалитьСчет.Click += new System.EventHandler(this.btnУдалитьСчет_Click);

            // btnЭкспортСчета
            this.btnЭкспортСчета.Location = new System.Drawing.Point(275, 8);
            this.btnЭкспортСчета.Name = "btnЭкспортСчета";
            this.btnЭкспортСчета.Size = new System.Drawing.Size(220, 25);
            this.btnЭкспортСчета.Text = "📥 Экспорт счёта-фактуры в Excel";
            this.btnЭкспортСчета.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnЭкспортСчета.UseVisualStyleBackColor = false;
            this.btnЭкспортСчета.Click += new System.EventHandler(this.btnЭкспортСчета_Click);

            // btnДобавитьПозицию
            this.btnДобавитьПозицию.Location = new System.Drawing.Point(16, 300);
            this.btnДобавитьПозицию.Name = "btnДобавитьПозицию";
            this.btnДобавитьПозицию.Size = new System.Drawing.Size(130, 25);
            this.btnДобавитьПозицию.Text = "Добавить позицию";
            this.btnДобавитьПозицию.Click += new System.EventHandler(this.btnДобавитьПозицию_Click);

            // btnУдалитьПозицию
            this.btnУдалитьПозицию.Location = new System.Drawing.Point(155, 300);
            this.btnУдалитьПозицию.Name = "btnУдалитьПозицию";
            this.btnУдалитьПозицию.Size = new System.Drawing.Size(130, 25);
            this.btnУдалитьПозицию.Text = "Удалить позицию";
            this.btnУдалитьПозицию.Click += new System.EventHandler(this.btnУдалитьПозицию_Click);

            // lblСчета
            this.lblСчета.AutoSize = true;
            this.lblСчета.Location = new System.Drawing.Point(16, 20);
            this.lblСчета.Text = "Счета-накладные:";
            this.lblСчета.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);

            // lblПозиции
            this.lblПозиции.AutoSize = true;
            this.lblПозиции.Location = new System.Drawing.Point(16, 310);
            this.lblПозиции.Text = "Позиции выбранного счёта:";
            this.lblПозиции.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);

            // FormСчета
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 545);
            this.Controls.Add(this.btnУдалитьПозицию);
            this.Controls.Add(this.btnДобавитьПозицию);
            this.Controls.Add(this.btnЭкспортСчета);
            this.Controls.Add(this.btnУдалитьСчет);
            this.Controls.Add(this.btnДобавитьСчет);
            this.Controls.Add(this.lblПозиции);
            this.Controls.Add(this.lblСчета);
            this.Controls.Add(this.dgvПозиции);
            this.Controls.Add(this.dgvСчета);
            this.Name = "FormСчета";
            this.Text = "Управление счетами-накладными";
            this.Load += new System.EventHandler(this.FormСчета_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvСчета)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvПозиции)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView dgvСчета;
        private System.Windows.Forms.DataGridView dgvПозиции;
        private System.Windows.Forms.Button btnДобавитьСчет;
        private System.Windows.Forms.Button btnУдалитьСчет;
        private System.Windows.Forms.Button btnЭкспортСчета;
        private System.Windows.Forms.Button btnДобавитьПозицию;
        private System.Windows.Forms.Button btnУдалитьПозицию;
        private System.Windows.Forms.Label lblСчета;
        private System.Windows.Forms.Label lblПозиции;
    }
}
