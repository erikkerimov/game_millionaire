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
    public partial class Form5 : Form
    {
        private FormSettings FS = new FormSettings();       
        public Form5()
        {
            InitializeComponent();
        }
        private void Form5_MouseMove(object sender, MouseEventArgs e)
        {
            FS.window_movement_move(this, e);
        }

        private void Form5_MouseDown(object sender, MouseEventArgs e)
        {
            FS.window_movement_down(this, e);
        }


        private void button_game_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите несгораемую сумму!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                switch(listBox1.SelectedIndex)
                {
                    case 0:
                        {
                            data_program.summa_nesgor = 1500000;
                            break;
                        }
                    case 1:
                        {
                            data_program.summa_nesgor = 800000;
                            break;
                        }
                    case 2:
                        {
                            data_program.summa_nesgor = 400000;
                            break;
                        }
                    case 3:
                        {
                            data_program.summa_nesgor = 200000;
                            break;
                        }
                    case 4:
                        {
                            data_program.summa_nesgor = 100000;
                            break;
                        }
                    case 5:
                        {
                            data_program.summa_nesgor = 50000;
                            break;
                        }
                    case 6:
                        {
                            data_program.summa_nesgor = 25000;
                            break;
                        }
                    case 7:
                        {
                            data_program.summa_nesgor = 15000;
                            break;
                        }
                    case 8:
                        {
                            data_program.summa_nesgor = 10000;
                            break;
                        }
                    case 9:
                        {
                            data_program.summa_nesgor = 5000;
                            break;
                        }
                }
                Form2 F2 = new Form2();
                F2.Show();
                this.Hide();
                this.Owner.Hide();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            FS.folding(this);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
