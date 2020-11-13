using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya
{
    public partial class Form6 : Form
    {
        private FormSettings FS = new FormSettings();
        Form4 F4;
        BD bd = new BD();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public Form6()
        {
            InitializeComponent();
        }

        private void button_game_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите вопрос", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                F4 = this.Owner as Form4;
                bd.OpenConnection();
                MySqlCommand command = new MySqlCommand("SELECT DISTINCT * FROM game_data WHERE Questions = @Q AND Name <> @N", bd.getConnection());
                command.Parameters.Add("@Q", MySqlDbType.VarChar).Value = comboBox1.SelectedItem;
                command.Parameters.Add("@N", MySqlDbType.VarChar).Value = F4.game_name;
                MySqlDataReader reader = command.ExecuteReader();
                List<string[]> data = new List<string[]>();
                while (reader.Read())
                {
                    data.Add(new string[7]);

                    data[data.Count - 1][0] = reader[0].ToString();
                    data[data.Count - 1][1] = reader[1].ToString();
                    data[data.Count - 1][2] = reader[2].ToString();
                    data[data.Count - 1][3] = reader[3].ToString();
                    data[data.Count - 1][4] = reader[4].ToString();
                    data[data.Count - 1][5] = reader[5].ToString();
                    data[data.Count - 1][6] = reader[6].ToString();
                }
                reader.Close();
                F4.textBox1.Text = data[0][1];
                F4.textBox2.Text = data[0][2];
                F4.textBox3.Text = data[0][3];
                F4.textBox4.Text = data[0][4];
                F4.textBox5.Text = data[0][5];
                if(data[0][6] == F4.textBox2.Text)
                {
                    F4.radioButton1.Checked = true;
                }
                else if(data[0][6] == F4.textBox3.Text)
                {
                    F4.radioButton2.Checked = true;
                }
                else if (data[0][6] == F4.textBox4.Text)
                {
                    F4.radioButton3.Checked = true;
                }
                else if (data[0][6] == F4.textBox5.Text)
                {
                    F4.radioButton4.Checked = true;
                }
                this.Close();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            FS.folding(this);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            F4 = this.Owner as Form4;
            bd.OpenConnection();
            MySqlCommand command = new MySqlCommand("SELECT DISTINCT Questions FROM game_data WHERE Slochnost = @S AND Name <> @N", bd.getConnection());
            command.Parameters.Add("@S", MySqlDbType.VarChar).Value = data_program.slochnost_tmp;
            command.Parameters.Add("@N", MySqlDbType.VarChar).Value = F4.game_name;
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[1]);

                data[data.Count - 1][0] = reader[0].ToString();
            }
            reader.Close();

            foreach (string[] s in data)
            {
                comboBox1.Items.Add(s[0]);
            }
            bd.CloseConnection();
        }

        private void Form6_MouseMove(object sender, MouseEventArgs e)
        {
            FS.window_movement_move(this, e);
        }

        private void Form6_MouseDown(object sender, MouseEventArgs e)
        {
            FS.window_movement_down(this, e);
        }
    }
}
