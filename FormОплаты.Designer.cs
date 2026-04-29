namespace AccountingApp
{
    partial class FormОплаты
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
            this.dgvОплаты = new System.Windows.Forms.DataGridView();
            this.btnДобавить = new System.Windows.Forms.Button();
            this.btnУдалить = new System.Windows.Forms.Button();
            this.btnОбновить = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvОплаты)).BeginInit();
            this.SuspendLayout();

            // dgvОплаты
            this.dgvОплаты.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvОплаты.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvОплаты.Location = new System.Drawing.Point(16, 15);
            this.dgvОплаты.Margin = new System.Windows.Forms.Padding(4);
            this.dgvОплаты.Name = "dgvОплаты";
            this.dgvОплаты.RowHeadersWidth = 51;
            this.dgvОплаты.Size = new System.Drawing.Size(1013, 492);
            this.dgvОплаты.TabIndex = 0;

            // btnДобавить
            this.btnДобавить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnДобавить.Location = new System.Drawing.Point(16, 523);
            this.btnДобавить.Margin = new System.Windows.Forms.Padding(4);
            this.btnДобавить.Name = "btnДобавить";
            this.btnДобавить.Size = new System.Drawing.Size(160, 37);
            this.btnДобавить.TabIndex = 1;
            this.btnДобавить.Text = "Добавить оплату";
            this.btnДобавить.UseVisualStyleBackColor = true;

            // btnУдалить
            this.btnУдалить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnУдалить.Location = new System.Drawing.Point(190, 523);
            this.btnУдалить.Margin = new System.Windows.Forms.Padding(4);
            this.btnУдалить.Name = "btnУдалить";
            this.btnУдалить.Size = new System.Drawing.Size(133, 37);
            this.btnУдалить.TabIndex = 2;
            this.btnУдалить.Text = "Удалить";
            this.btnУдалить.UseVisualStyleBackColor = true;

            // btnОбновить
            this.btnОбновить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnОбновить.Location = new System.Drawing.Point(335, 523);
            this.btnОбновить.Margin = new System.Windows.Forms.Padding(4);
            this.btnОбновить.Name = "btnОбновить";
            this.btnОбновить.Size = new System.Drawing.Size(133, 37);
            this.btnОбновить.TabIndex = 3;
            this.btnОбновить.Text = "Обновить";
            this.btnОбновить.UseVisualStyleBackColor = true;

            // FormОплаты
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 580);
            this.Controls.Add(this.btnОбновить);
            this.Controls.Add(this.btnУдалить);
            this.Controls.Add(this.btnДобавить);
            this.Controls.Add(this.dgvОплаты);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormОплаты";
            this.Text = "Оплаты по счетам";
            ((System.ComponentModel.ISupportInitialize)(this.dgvОплаты)).EndInit();

            // События
            this.Load += new System.EventHandler(this.FormОплаты_Load);
            this.btnДобавить.Click += new System.EventHandler(this.btnДобавить_Click);
            this.btnУдалить.Click += new System.EventHandler(this.btnУдалить_Click);
            this.btnОбновить.Click += new System.EventHandler(this.btnОбновить_Click);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dgvОплаты;
        private System.Windows.Forms.Button btnДобавить;
        private System.Windows.Forms.Button btnУдалить;
        private System.Windows.Forms.Button btnОбновить;
    }
}
