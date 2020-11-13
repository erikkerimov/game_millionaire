using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya
{
    public partial class Form1 : Form
    {
        SoundPlayer soundtrack;
        public Form1()
        {
            InitializeComponent();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream resourceStream_track = assembly.GetManifestResourceStream(@"Kursovaya.soundtrack.wav");
            soundtrack = new SoundPlayer(resourceStream_track);
            soundtrack.PlayLooping();
        }

        private void button_game_Click(object sender, EventArgs e)
        {
            try
            {
                Form F3 = new Form3();
                F3.ShowDialog(this);
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        
        Point lastPoint;
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void button_make_Click(object sender, EventArgs e)
        {
            try
            {
                BD bd = new BD();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                bd.OpenConnection();
                bd.CloseConnection();
                Form F4 = new Form4();
                F4.ShowDialog(this);
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BD bd = new BD();
            DataTable table = new DataTable();
            try
            {
                bd.OpenConnection();

                MySqlCommand command2 = new MySqlCommand("TRUNCATE TABLE `game_data`", bd.getConnection());
                command2.ExecuteNonQuery();
                bd.CloseConnection();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
