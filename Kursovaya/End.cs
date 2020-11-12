using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya
{
    public partial class End : Form
    {
        public End()
        {
            InitializeComponent();
            label1.Text = data_program.Message[0];
            label2.Text += " " + data_program.Message[1];
            label3.Text += " " + data_program.Message[2];
            if (data_program.Message[0] == "Вы проиграли! Не огорчайтесь!")
            {
                label1.ForeColor = ColorTranslator.FromHtml("#ff0000");
            }
            if (data_program.Message[0] == "Поздравляем, вы победили!")
            {
                label1.ForeColor = ColorTranslator.FromHtml("#00ff45");
            }
        }

        private void button_inmenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Owner.Hide();
            Form F1 = new Form1();
            F1.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
