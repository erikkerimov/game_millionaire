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
    public partial class Form3 : Form
    {
        // ДЛЯ БАЗЫ ДАННЫХ
        BD bd = new BD();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        public Form3()
        {
            InitializeComponent();
            bd.OpenConnection();
            DataTable table = new DataTable();
            MySqlCommand command = new MySqlCommand("SELECT DISTINCT Name FROM game_data", bd.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[1]);

                data[data.Count - 1][0] = reader[0].ToString();
            }
            reader.Close();

            foreach(string[] s in data)
            {
                comboBox1.Items.Add(s[0]);
            }
            bd.CloseConnection();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button_game_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Выберите игру из списка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                data_program.game_name = comboBox1.Text;
                Form5 F5 = new Form5();
                F5.ShowDialog(this.Owner);
                this.Hide();
            }
        }

        Point lastPoint;
        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

    }
}
