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
            this.pnlHeader   = new System.Windows.Forms.Panel();
            this.lblTitle    = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.pnlClient   = new System.Windows.Forms.Panel();
            this.lblКлиент   = new System.Windows.Forms.Label();
            this.cmbКлиент   = new System.Windows.Forms.ComboBox();

            this.grpПродажи  = new System.Windows.Forms.GroupBox();
            this.dgvПродажи  = new System.Windows.Forms.DataGridView();

            this.splitBottom = new System.Windows.Forms.SplitContainer();
            this.grpПозиции  = new System.Windows.Forms.GroupBox();
            this.dgvПозиции  = new System.Windows.Forms.DataGridView();
            this.grpОплаты   = new System.Windows.Forms.GroupBox();
            this.dgvОплаты   = new System.Windows.Forms.DataGridView();

            this.pnlFooter   = new System.Windows.Forms.Panel();
            this.lblИтог     = new System.Windows.Forms.Label();
            this.btnЗакрыть  = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlClient.SuspendLayout();
            this.grpПродажи.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).BeginInit();
            this.splitBottom.Panel1.SuspendLayout();
            this.splitBottom.Panel2.SuspendLayout();
            this.splitBottom.SuspendLayout();
            this.grpПозиции.SuspendLayout();
            this.grpОплаты.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvПродажи)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvПозиции)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvОплаты)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // ---------- Header ----------
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(46, 117, 182);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 70;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 8);
            this.lblTitle.Text = "Оплаты клиента";

            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(220, 235, 250);
            this.lblSubtitle.Location = new System.Drawing.Point(22, 42);
            this.lblSubtitle.Text = "Выберите клиента, затем продажу — снизу появятся товары и оплаты";

            // ---------- Client picker ----------
            this.pnlClient.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.pnlClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlClient.Height = 60;
            this.pnlClient.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);
            this.pnlClient.Controls.Add(this.cmbКлиент);
            this.pnlClient.Controls.Add(this.lblКлиент);

            this.lblКлиент.AutoSize = true;
            this.lblКлиент.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblКлиент.ForeColor = System.Drawing.Color.FromArgb(46, 117, 182);
            this.lblКлиент.Location = new System.Drawing.Point(22, 22);
            this.lblКлиент.Text = "Клиент:";

            this.cmbКлиент.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbКлиент.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbКлиент.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbКлиент.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbКлиент.Location = new System.Drawing.Point(95, 18);
            this.cmbКлиент.Size = new System.Drawing.Size(880, 26);
            this.cmbКлиент.TabIndex = 0;

            // ---------- Sales group ----------
            this.grpПродажи.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpПродажи.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpПродажи.ForeColor = System.Drawing.Color.FromArgb(46, 117, 182);
            this.grpПродажи.Height = 270;
            this.grpПродажи.Padding = new System.Windows.Forms.Padding(10, 12, 10, 10);
            this.grpПродажи.Text = " Продажи клиента ";
            this.grpПродажи.Controls.Add(this.dgvПродажи);

            this.dgvПродажи.Dock = System.Windows.Forms.DockStyle.Fill;
            ConfigureGrid(this.dgvПродажи);

            // ---------- Bottom split (positions + payments) ----------
            this.splitBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBottom.SplitterDistance = 500;
            this.splitBottom.Panel1.Controls.Add(this.grpПозиции);
            this.splitBottom.Panel2.Controls.Add(this.grpОплаты);

            this.grpПозиции.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpПозиции.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpПозиции.ForeColor = System.Drawing.Color.FromArgb(46, 117, 182);
            this.grpПозиции.Padding = new System.Windows.Forms.Padding(10);
            this.grpПозиции.Text = " Товары выбранной продажи ";
            this.grpПозиции.Controls.Add(this.dgvПозиции);

            this.dgvПозиции.Dock = System.Windows.Forms.DockStyle.Fill;
            ConfigureGrid(this.dgvПозиции);

            this.grpОплаты.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpОплаты.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpОплаты.ForeColor = System.Drawing.Color.FromArgb(46, 117, 182);
            this.grpОплаты.Padding = new System.Windows.Forms.Padding(10);
            this.grpОплаты.Text = " Оплаты по продаже ";
            this.grpОплаты.Controls.Add(this.dgvОплаты);

            this.dgvОплаты.Dock = System.Windows.Forms.DockStyle.Fill;
            ConfigureGrid(this.dgvОплаты);

            // ---------- Footer ----------
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(46, 117, 182);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 56;
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlFooter.Controls.Add(this.btnЗакрыть);
            this.pnlFooter.Controls.Add(this.lblИтог);

            this.lblИтог.AutoSize = false;
            this.lblИтог.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblИтог.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblИтог.ForeColor = System.Drawing.Color.White;
            this.lblИтог.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblИтог.Width = 800;
            this.lblИтог.Text = "Выберите клиента, чтобы увидеть итог.";

            this.btnЗакрыть.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnЗакрыть.BackColor = System.Drawing.Color.White;
            this.btnЗакрыть.FlatAppearance.BorderSize = 0;
            this.btnЗакрыть.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnЗакрыть.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnЗакрыть.ForeColor = System.Drawing.Color.FromArgb(46, 117, 182);
            this.btnЗакрыть.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnЗакрыть.Location = new System.Drawing.Point(870, 10);
            this.btnЗакрыть.Size = new System.Drawing.Size(120, 36);
            this.btnЗакрыть.Text = "Закрыть";
            this.btnЗакрыть.UseVisualStyleBackColor = false;

            // ---------- Form ----------
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.ClientSize = new System.Drawing.Size(1020, 720);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MinimumSize = new System.Drawing.Size(900, 600);

            // ВАЖЕН порядок добавления для Dock — Fill добавляется первым,
            // дальше Top/Bottom в нужном порядке снизу-вверх.
            this.Controls.Add(this.splitBottom);
            this.Controls.Add(this.grpПродажи);
            this.Controls.Add(this.pnlClient);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlFooter);

            this.Name = "FormОплатыКлиента";
            this.Text = "Оплаты клиента";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            this.Load                            += new System.EventHandler(this.FormОплатыКлиента_Load);
            this.cmbКлиент.SelectedIndexChanged  += new System.EventHandler(this.cmbКлиент_SelectedIndexChanged);
            this.dgvПродажи.SelectionChanged     += new System.EventHandler(this.dgvПродажи_SelectionChanged);
            this.btnЗакрыть.Click                += new System.EventHandler((s, e) => this.Close());

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlClient.ResumeLayout(false);
            this.pnlClient.PerformLayout();
            this.grpПродажи.ResumeLayout(false);
            this.splitBottom.Panel1.ResumeLayout(false);
            this.splitBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).EndInit();
            this.splitBottom.ResumeLayout(false);
            this.grpПозиции.ResumeLayout(false);
            this.grpОплаты.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvПродажи)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvПозиции)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvОплаты)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private static void ConfigureGrid(System.Windows.Forms.DataGridView g)
        {
            g.BackgroundColor = System.Drawing.Color.White;
            g.BorderStyle = System.Windows.Forms.BorderStyle.None;
            g.GridColor = System.Drawing.Color.FromArgb(225, 232, 240);
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.ReadOnly = true;
            g.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.AutoGenerateColumns = false;
            g.RowTemplate.Height = 28;
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(225, 235, 247);
            g.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(31, 78, 121);
            g.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            g.ColumnHeadersHeight = 32;
            g.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            g.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(247, 251, 255);
            g.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(46, 117, 182);
            g.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            g.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            g.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
        }

        private System.Windows.Forms.Panel       pnlHeader;
        private System.Windows.Forms.Label       lblTitle;
        private System.Windows.Forms.Label       lblSubtitle;

        private System.Windows.Forms.Panel       pnlClient;
        private System.Windows.Forms.Label       lblКлиент;
        private System.Windows.Forms.ComboBox    cmbКлиент;

        private System.Windows.Forms.GroupBox    grpПродажи;
        private System.Windows.Forms.DataGridView dgvПродажи;

        private System.Windows.Forms.SplitContainer splitBottom;
        private System.Windows.Forms.GroupBox    grpПозиции;
        private System.Windows.Forms.DataGridView dgvПозиции;
        private System.Windows.Forms.GroupBox    grpОплаты;
        private System.Windows.Forms.DataGridView dgvОплаты;

        private System.Windows.Forms.Panel       pnlFooter;
        private System.Windows.Forms.Label       lblИтог;
        private System.Windows.Forms.Button      btnЗакрыть;
    }
}
