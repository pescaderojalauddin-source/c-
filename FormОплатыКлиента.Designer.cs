namespace AccountingApp
{
    partial class FormОплатыКлиента
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
            this.lblКлиент = new System.Windows.Forms.Label();
            this.cmbКлиент = new System.Windows.Forms.ComboBox();
            this.lblПродажи = new System.Windows.Forms.Label();
            this.dgvПродажи = new System.Windows.Forms.DataGridView();
            this.lblПозиции = new System.Windows.Forms.Label();
            this.dgvПозиции = new System.Windows.Forms.DataGridView();
            this.lblОплаты = new System.Windows.Forms.Label();
            this.dgvОплаты = new System.Windows.Forms.DataGridView();
            this.lblИтог = new System.Windows.Forms.Label();
            this.btnЗакрыть = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvПродажи)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvПозиции)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvОплаты)).BeginInit();
            this.SuspendLayout();

            // lblКлиент
            this.lblКлиент.AutoSize = true;
            this.lblКлиент.Location = new System.Drawing.Point(15, 18);
            this.lblКлиент.Text = "Клиент:";

            // cmbКлиент
            this.cmbКлиент.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbКлиент.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbКлиент.Location = new System.Drawing.Point(85, 15);
            this.cmbКлиент.Size = new System.Drawing.Size(800, 24);
            this.cmbКлиент.TabIndex = 0;

            // lblПродажи
            this.lblПродажи.AutoSize = true;
            this.lblПродажи.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblПродажи.Location = new System.Drawing.Point(15, 55);
            this.lblПродажи.Text = "Продажи клиента";

            // dgvПродажи
            this.dgvПродажи.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvПродажи.AllowUserToAddRows = false;
            this.dgvПродажи.AllowUserToDeleteRows = false;
            this.dgvПродажи.ReadOnly = true;
            this.dgvПродажи.Location = new System.Drawing.Point(15, 80);
            this.dgvПродажи.Size = new System.Drawing.Size(870, 220);
            this.dgvПродажи.TabIndex = 1;

            // lblПозиции
            this.lblПозиции.AutoSize = true;
            this.lblПозиции.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblПозиции.Location = new System.Drawing.Point(15, 315);
            this.lblПозиции.Text = "Товары выбранной продажи";

            // dgvПозиции
            this.dgvПозиции.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left))));
            this.dgvПозиции.AllowUserToAddRows = false;
            this.dgvПозиции.AllowUserToDeleteRows = false;
            this.dgvПозиции.ReadOnly = true;
            this.dgvПозиции.Location = new System.Drawing.Point(15, 340);
            this.dgvПозиции.Size = new System.Drawing.Size(430, 200);
            this.dgvПозиции.TabIndex = 2;

            // lblОплаты
            this.lblОплаты.AutoSize = true;
            this.lblОплаты.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblОплаты.Location = new System.Drawing.Point(455, 315);
            this.lblОплаты.Text = "Оплаты по этой продаже";

            // dgvОплаты
            this.dgvОплаты.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Right))));
            this.dgvОплаты.AllowUserToAddRows = false;
            this.dgvОплаты.AllowUserToDeleteRows = false;
            this.dgvОплаты.ReadOnly = true;
            this.dgvОплаты.Location = new System.Drawing.Point(455, 340);
            this.dgvОплаты.Size = new System.Drawing.Size(430, 200);
            this.dgvОплаты.TabIndex = 3;

            // lblИтог
            this.lblИтог.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblИтог.AutoSize = true;
            this.lblИтог.Location = new System.Drawing.Point(15, 555);
            this.lblИтог.Text = "Итог по клиенту:";
            this.lblИтог.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            // btnЗакрыть
            this.btnЗакрыть.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnЗакрыть.Location = new System.Drawing.Point(785, 550);
            this.btnЗакрыть.Size = new System.Drawing.Size(100, 32);
            this.btnЗакрыть.TabIndex = 4;
            this.btnЗакрыть.Text = "Закрыть";

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.lblКлиент);
            this.Controls.Add(this.cmbКлиент);
            this.Controls.Add(this.lblПродажи);
            this.Controls.Add(this.dgvПродажи);
            this.Controls.Add(this.lblПозиции);
            this.Controls.Add(this.dgvПозиции);
            this.Controls.Add(this.lblОплаты);
            this.Controls.Add(this.dgvОплаты);
            this.Controls.Add(this.lblИтог);
            this.Controls.Add(this.btnЗакрыть);
            this.Name = "FormОплатыКлиента";
            this.Text = "Оплаты клиента";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            this.Load                            += new System.EventHandler(this.FormОплатыКлиента_Load);
            this.cmbКлиент.SelectedIndexChanged  += new System.EventHandler(this.cmbКлиент_SelectedIndexChanged);
            this.dgvПродажи.SelectionChanged     += new System.EventHandler(this.dgvПродажи_SelectionChanged);
            this.btnЗакрыть.Click                += new System.EventHandler((s, e) => this.Close());

            ((System.ComponentModel.ISupportInitialize)(this.dgvПродажи)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvПозиции)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvОплаты)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblКлиент;
        private System.Windows.Forms.ComboBox cmbКлиент;
        private System.Windows.Forms.Label lblПродажи;
        private System.Windows.Forms.DataGridView dgvПродажи;
        private System.Windows.Forms.Label lblПозиции;
        private System.Windows.Forms.DataGridView dgvПозиции;
        private System.Windows.Forms.Label lblОплаты;
        private System.Windows.Forms.DataGridView dgvОплаты;
        private System.Windows.Forms.Label lblИтог;
        private System.Windows.Forms.Button btnЗакрыть;
    }
}
