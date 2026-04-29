namespace AccountingApp
{
    partial class FormОтгрузки
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
            this.dgvОтгрузки = new System.Windows.Forms.DataGridView();
            this.btnДобавить = new System.Windows.Forms.Button();
            this.btnУдалить = new System.Windows.Forms.Button();
            this.btnОбновить = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvОтгрузки)).BeginInit();
            this.SuspendLayout();

            // dgvОтгрузки
            this.dgvОтгрузки.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvОтгрузки.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvОтгрузки.Location = new System.Drawing.Point(16, 15);
            this.dgvОтгрузки.Margin = new System.Windows.Forms.Padding(4);
            this.dgvОтгрузки.Name = "dgvОтгрузки";
            this.dgvОтгрузки.RowHeadersWidth = 51;
            this.dgvОтгрузки.Size = new System.Drawing.Size(1013, 492);
            this.dgvОтгрузки.TabIndex = 0;

            // btnДобавить
            this.btnДобавить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnДобавить.Location = new System.Drawing.Point(16, 523);
            this.btnДобавить.Margin = new System.Windows.Forms.Padding(4);
            this.btnДобавить.Name = "btnДобавить";
            this.btnДобавить.Size = new System.Drawing.Size(133, 37);
            this.btnДобавить.TabIndex = 1;
            this.btnДобавить.Text = "Добавить отгрузку";
            this.btnДобавить.UseVisualStyleBackColor = true;

            // btnУдалить
            this.btnУдалить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnУдалить.Location = new System.Drawing.Point(167, 523);
            this.btnУдалить.Margin = new System.Windows.Forms.Padding(4);
            this.btnУдалить.Name = "btnУдалить";
            this.btnУдалить.Size = new System.Drawing.Size(133, 37);
            this.btnУдалить.TabIndex = 2;
            this.btnУдалить.Text = "Удалить";
            this.btnУдалить.UseVisualStyleBackColor = true;

            // btnОбновить
            this.btnОбновить.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnОбновить.Location = new System.Drawing.Point(317, 523);
            this.btnОбновить.Margin = new System.Windows.Forms.Padding(4);
            this.btnОбновить.Name = "btnОбновить";
            this.btnОбновить.Size = new System.Drawing.Size(133, 37);
            this.btnОбновить.TabIndex = 3;
            this.btnОбновить.Text = "Обновить";
            this.btnОбновить.UseVisualStyleBackColor = true;

            // FormОтгрузки
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 567);
            this.Controls.Add(this.btnОбновить);
            this.Controls.Add(this.btnУдалить);
            this.Controls.Add(this.btnДобавить);
            this.Controls.Add(this.dgvОтгрузки);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormОтгрузки";
            this.Text = "Журнал отгрузок";
            ((System.ComponentModel.ISupportInitialize)(this.dgvОтгрузки)).EndInit();

            // События
            this.Load += new System.EventHandler(this.FormОтгрузки_Load);
            this.btnДобавить.Click += new System.EventHandler(this.btnДобавить_Click);
            this.btnУдалить.Click += new System.EventHandler(this.btnУдалить_Click);
            this.btnОбновить.Click += new System.EventHandler(this.btnОбновить_Click);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dgvОтгрузки;
        private System.Windows.Forms.Button btnДобавить;
        private System.Windows.Forms.Button btnУдалить;
        private System.Windows.Forms.Button btnОбновить;
    }
}