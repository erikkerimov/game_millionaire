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
        private SoundPlayer soundtrack;
        private FormSettings FS = new FormSettings();
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

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            FS.window_movement_move(this, e);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            FS.window_movement_down(this, e);
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
            FS.folding(this);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
