using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya
{
    public partial class Form4 : Form
    {
        // ДЛЯ БАЗЫ ДАННЫХ
        BD bd = new BD();
        MySqlDataAdapter adapter = new MySqlDataAdapter();

        //ПЕРЕМЕННЫЕ
        int count = 1;
        public string game_name;
        string true_answer;
        string slochnost = "Легкий";
        Point lastPoint;


        public Form4()
        {
            InitializeComponent();
        }

        private void button_next_Click(object sender, EventArgs e)
        {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == ""
                    || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
                {
                    MessageBox.Show("Все поля обязательны к введению!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false && radioButton4.Checked == false)
                {
                    MessageBox.Show("Выберите правильный ответ!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (count == 15)
                    {
                    MessageBox.Show("Игра была успешно создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    }
                    string questions = textBox1.Text;
                    string a = textBox2.Text;
                    string b = textBox3.Text;
                    string c = textBox4.Text;
                    string d = textBox5.Text;
                    if (count<=5)
                    {
                        slochnost = "Легкий";
                    }
                    if(count>=6 && count <=10)
                    {
                        slochnost = "Средний";
                    }
                    if (count >= 11)
                    {
                        slochnost = "Сложный";
                    }
                    if (radioButton1.Checked)
                    {
                        true_answer = textBox2.Text;
                    }
                    if (radioButton2.Checked)
                    {
                        true_answer = textBox3.Text;
                    }
                    if (radioButton3.Checked)
                    {
                        true_answer = textBox4.Text;
                    }
                    if (radioButton4.Checked)
                    {
                        true_answer = textBox5.Text;
                    }
                    

                    MySqlCommand command = new MySqlCommand("INSERT INTO `game_data` (`Name`, `Questions`, `var_a`, `var_b`, `var_c`, `var_d`, `true_answ`,`Slochnost`) " +
                        "                                   VALUES (@name, @questions, @a, @b, @c, @d, @true, @s)", bd.getConnection());
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = game_name;
                    command.Parameters.Add("@questions", MySqlDbType.VarChar).Value = questions;
                    command.Parameters.Add("@a", MySqlDbType.VarChar).Value = a;
                    command.Parameters.Add("@b", MySqlDbType.VarChar).Value = b;
                    command.Parameters.Add("@c", MySqlDbType.VarChar).Value = c;
                    command.Parameters.Add("@d", MySqlDbType.VarChar).Value = d;
                    command.Parameters.Add("@true", MySqlDbType.VarChar).Value = true_answer;
                    command.Parameters.Add("@s", MySqlDbType.VarChar).Value = slochnost;

                    bd.OpenConnection();
                    if (command.ExecuteNonQuery() == 1)
                    {
                        count++;
                        label2.Text = "Вопрос №" + count.ToString() + " / 15";
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                        radioButton1.Checked = false; radioButton2.Checked = false;
                        radioButton3.Checked = false; radioButton4.Checked = false;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка", "Произошла ошибка");
                    }
                    bd.CloseConnection();
                    if (count <= 5)
                    {
                        slochnost = "Легкий";
                    }
                    if (count >= 6 && count <= 10)
                    {
                        slochnost = "Средний";
                    }
                    if (count >= 11)
                    {
                        slochnost = "Сложный";
                    }
                }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                MessageBox.Show("Вы не ввели название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DataTable table_for_check_name = new DataTable();
                string tmp_name = textBox6.Text;
                MySqlCommand command_check = new MySqlCommand("SELECT * FROM `game_data` WHERE `Name` = @tmpname", bd.getConnection());
                command_check.Parameters.Add("@tmpname", MySqlDbType.VarChar).Value = tmp_name;
                adapter.SelectCommand = command_check;
                adapter.Fill(table_for_check_name);
                if (table_for_check_name.Rows.Count == 0)
                {
                    this.Size = new Size(756, 378);
                    this.CenterToScreen();
                    DataTable table_for_check_null = new DataTable();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM `game_data`", bd.getConnection());
                    game_name = textBox6.Text;
                    panel1.Hide();
                    adapter.SelectCommand = command;
                    adapter.Fill(table_for_check_null);
                    if (table_for_check_null.Rows.Count == 0)
                    {
                        bd.OpenConnection();

                        MySqlCommand command2 = new MySqlCommand("TRUNCATE TABLE `game_data`", bd.getConnection());
                        command2.ExecuteNonQuery();
                        bd.CloseConnection();
                    }
                }
                else
                {
                    MessageBox.Show("Игра с таким названием уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form4_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void Form4_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            bd.OpenConnection();
            if (panel1.Visible == false && count !=16)
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM `game_data` WHERE `Name` = @name", bd.getConnection());
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = game_name;
                command.ExecuteNonQuery();
            }
            bd.CloseConnection();
        }

        private void button_game_Click(object sender, EventArgs e)
        {
            data_program.slochnost_tmp = slochnost;
            Form6 F6 = new Form6();
            F6.ShowDialog(this);
        }
    }
}
