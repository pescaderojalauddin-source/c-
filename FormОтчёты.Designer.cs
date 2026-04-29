namespace AccountingApp
{
    partial class FormОтчёты
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
            this.lblЗаголовок = new System.Windows.Forms.Label();
            this.btnПросрочка = new System.Windows.Forms.Button();
            this.btnДиаграмма = new System.Windows.Forms.Button();
            this.btnЗакрыть = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblЗаголовок
            this.lblЗаголовок.AutoSize = false;
            this.lblЗаголовок.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblЗаголовок.Location = new System.Drawing.Point(20, 15);
            this.lblЗаголовок.Size = new System.Drawing.Size(420, 30);
            this.lblЗаголовок.Text = "📊  Отчёты и диаграммы";
            this.lblЗаголовок.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // btnПросрочка
            this.btnПросрочка.Location = new System.Drawing.Point(20, 65);
            this.btnПросрочка.Size = new System.Drawing.Size(420, 70);
            this.btnПросрочка.Text = "📋  Отчёт: Просроченные платежи\n(на дату, экспорт в Excel)";
            this.btnПросрочка.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnПросрочка.BackColor = System.Drawing.Color.LightCoral;
            this.btnПросрочка.UseVisualStyleBackColor = false;

            // btnДиаграмма
            this.btnДиаграмма.Location = new System.Drawing.Point(20, 145);
            this.btnДиаграмма.Size = new System.Drawing.Size(420, 70);
            this.btnДиаграмма.Text = "🥧  Диаграмма: НДС за период\n(круговая диаграмма по ставкам НДС)";
            this.btnДиаграмма.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnДиаграмма.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnДиаграмма.UseVisualStyleBackColor = false;

            // btnЗакрыть
            this.btnЗакрыть.Location = new System.Drawing.Point(170, 235);
            this.btnЗакрыть.Size = new System.Drawing.Size(120, 32);
            this.btnЗакрыть.Text = "Закрыть";
            this.btnЗакрыть.UseVisualStyleBackColor = true;
            this.btnЗакрыть.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            // FormОтчёты
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 280);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Controls.Add(this.btnЗакрыть);
            this.Controls.Add(this.btnДиаграмма);
            this.Controls.Add(this.btnПросрочка);
            this.Controls.Add(this.lblЗаголовок);
            this.Name = "FormОтчёты";
            this.Text = "Отчёты и диаграммы";

            this.btnПросрочка.Click += new System.EventHandler(this.btnПросрочка_Click);
            this.btnДиаграмма.Click += new System.EventHandler(this.btnДиаграмма_Click);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblЗаголовок;
        private System.Windows.Forms.Button btnПросрочка;
        private System.Windows.Forms.Button btnДиаграмма;
        private System.Windows.Forms.Button btnЗакрыть;
    }
}
