using System;
using System.Windows.Forms;

namespace AccountingApp
{
    public partial class FormОтчёты : Form
    {
        public FormОтчёты()
        {
            InitializeComponent();
        }

        private void btnПросрочка_Click(object sender, EventArgs e)
        {
            new FormОтчётПросрочка().ShowDialog();
        }

        private void btnДиаграмма_Click(object sender, EventArgs e)
        {
            new FormДиаграммаНДС().ShowDialog();
        }
    }
}
