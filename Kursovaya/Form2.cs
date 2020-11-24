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
using System.Threading;
using System.Drawing.Drawing2D;
using System.Reflection.Emit;
using System.Media;
using Kursovaya.Properties;
using System.Windows.Forms.VisualStyles;

namespace Kursovaya
{
    public partial class Form2 : Form
    {
        _Sound Sound = new _Sound();
        private FormSettings FS = new FormSettings();
        private Balance balance = new Balance();
        private Random rnd = new Random();
        private BD bd = new BD();
        private int current_question = 0; 
        private List<string[]> data = new List<string[]>();
        private SoundPlayer start_game;
        private SoundPlayer otvet;
        private SoundPlayer false_answer;
        private SoundPlayer on50;
        private SoundPlayer sms;
        private SoundPlayer true_answer;
        private SoundPlayer phone;
        private SoundPlayer win;

        public Form2()
        {
            InitializeComponent();
            bd.OpenConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM game_data WHERE Name = @N", bd.getConnection());
            command.Parameters.Add("@N", MySqlDbType.VarChar).Value = data_program.game_name;
            MySqlDataReader reader = command.ExecuteReader();
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
            InitializeGame();
        }
        private void InitializeGame()
        {
            balance.get_argument(label3);
            initialize_sound();
            label2.Text = "Несгораемая сумма: " + data_program.summa_nesgor;
            game_start();
        }
        private async Task game_start()
        {
            Sound.SoundProccesing(start_game);
            debrov("Добро пожаловать в игру!");
            button_enabled(false);
            button_question.Text = data[0][1];
            await Task.Delay(TimeSpan.FromSeconds(1));
            button_A.Text = data[0][2];
            await Task.Delay(TimeSpan.FromSeconds(1));
            button_B.Text = data[0][3];
            await Task.Delay(TimeSpan.FromSeconds(1));
            button_C.Text = data[0][4];
            await Task.Delay(TimeSpan.FromSeconds(1));
            button_D.Text = data[0][5];
            button_enabled(true);
        }
        private void Form2_MouseMove(object sender, MouseEventArgs e) // FOR MOVING FORM
        {
            FS.window_movement_move(this, e);
        } 
        private void Form2_MouseDown(object sender, MouseEventArgs e) // FOR MOVING FORM
        {
            FS.window_movement_down(this, e);
        } 
        private async Task Continuation(Button but) // Processing the response
        {
            if (but.Text == data[current_question][6])
            {

                current_question++;
                if (current_question == data.Count)
                {
                    green_button(but, false);
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    balance.Upd();
                    button_enabled(true);
                    good_finish();
                }
                else
                {
                    await green_button(but, false);
                    await debrov("Следующий вопрос..");
                    button_reset(but);
                    if (panel1.Visible == true)
                    {
                        panel1.Visible = false;
                    }
                    if (x2_active.Visible == true)
                    {
                        x2_active.Visible = false;
                    }
                    label1.Text = "Вопрос " + (current_question + 1) + " / 15";
                    balance.Upd();
                    button_question.Text = data[current_question][1];
                    button_enabled(false);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    button_A.Text = data[current_question][2];
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    button_B.Text = data[current_question][3];
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    button_C.Text = data[current_question][4];
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    button_D.Text = data[current_question][5];
                    button_enabled(true);
                }
            }
            else
            {
                debrov("К сожалению игра закончена, мне жаль!");
                Sound.SoundProccesing(false_answer);
                Button true_b = checked_true_button(data[current_question][6]);
                game_over(true_b, but);
            }
        }
        private async void next_quest(object sender, EventArgs e) // response
        {
            Sound.SoundProccesing(otvet);
            Button but = sender as Button;
            btn_focus.Focus();
            but.BackColor = ColorTranslator.FromHtml("#a46400");
            but.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#a46400");
            but.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#a46400");
            button_enabled(false);
            if (but == button_A)
            {
                pictureBox1.Image = Game_Pict.game_cap_select_a;
            }
            if (but == button_B)
            {
                pictureBox1.Image = Game_Pict.game_cap_select_b;
            }
            if (but == button_C)
            {
                pictureBox1.Image = Game_Pict.game_cap_select_c;
            }
            if (but == button_D)
            {
                pictureBox1.Image = Game_Pict.game_cap_select_d;
            }
            await debrov("Внимание...");
            Continuation(but);
        }
        private void button_enabled(bool enable)
        {
            if (enable == false)
            {
                this.button_A.Click -= new System.EventHandler(this.next_quest);
                this.button_B.Click -= new System.EventHandler(this.next_quest);
                this.button_C.Click -= new System.EventHandler(this.next_quest);
                this.button_D.Click -= new System.EventHandler(this.next_quest);
            }
            else
            {
                this.button_A.Click += new System.EventHandler(this.next_quest);
                this.button_B.Click += new System.EventHandler(this.next_quest);
                this.button_C.Click += new System.EventHandler(this.next_quest);
                this.button_D.Click += new System.EventHandler(this.next_quest);
            }
            button_50na50.Enabled = enable;
            button_zall.Enabled = enable;
            button_x2.Enabled = enable;
            button_call.Enabled = enable;
        }
        private void button_reset(Button but)
        {
            but.BackColor = ColorTranslator.FromHtml("#180246");
            but.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#180246");
            but.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#180246");
            pictureBox1.Image = Game_Pict.game_cap;
            button_A.Visible = true;
            button_B.Visible = true;
            button_C.Visible = true;
            button_D.Visible = true;
            button_call.Enabled = true;
            button_x2.Enabled = true;
            button_zall.Enabled = true;
            button_50na50.Enabled = true;
            button_A.Text = ""; button_B.Text = ""; button_C.Text = ""; button_D.Text = "";
            if (label4.Visible == true)
            {
                label4.Visible = false;
                pictureBox4.Visible = false;
            }
        }
        private Button checked_true_button(string quest)
        {
            Button true_button = new Button();
            if(button_A.Text == quest)
            {
                true_button = button_A;
            }
            if(button_B.Text == quest)
            {
                true_button = button_B;
            }
            if (button_C.Text == quest)
            {
                true_button = button_C;
            }
            if (button_D.Text == quest)
            {
                true_button = button_D;
            }
            return true_button;
        }
        private async Task green_button(Button but, bool game_over)
        {
            but.BackColor = ColorTranslator.FromHtml("#08ac00");
            but.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#08ac00");
            but.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#08ac00");
            if (button_A == but)
            {
                pictureBox1.Image = Game_Pict.game_cap_select_a_true;
            }
            if (button_B == but)
            {
                pictureBox1.Image = Game_Pict.game_cap_select_b_true;
            }
            if (button_C == but)
            {
                pictureBox1.Image = Game_Pict.game_cap_select_c_true;
            }
            if (button_D == but)
            {
                pictureBox1.Image = Game_Pict.game_cap_select_d_true;
            }
            if (game_over == false)
            {
                Sound.SoundProccesing(true_answer);
                await debrov("Это правильный ответ!");
                
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e) // close programm
        {
            Application.Exit();
        } 
        private void pictureBox3_Click(object sender, EventArgs e) // folding
        {
            FS.folding(this);
        }
        private async void button_50na50_Click(object sender, EventArgs e)
        {
            Sound.SoundProccesing(on50);
            Button true_b = checked_true_button(data[current_question][6]);
            int true_c = 0;
            if(true_b==button_A)
            {
                true_c = 1;
            }
            else if(true_b==button_B)
            {
                true_c = 2;
            }
            else if(true_b==button_C)
            {
                true_c = 3;
            }
            else if(true_b==button_D)
            {
                true_c = 4;
            }

            while(true)
            {
                int random_c = rnd.Next(1, 5);
                int random_c2 = rnd.Next(1, 5);
                if(random_c != true_c && random_c2 != true_c && random_c != random_c2)
                {
                    switch(random_c)
                    {
                        case 1:
                            {
                                button_A.Hide();
                                break;
                            }
                        case 2:
                            {
                                button_B.Hide();
                                break;
                            }
                        case 3:
                            {
                                button_C.Hide();
                                break;
                            }
                        case 4:
                            {
                                button_D.Hide();
                                break;
                            }
                    }
                    switch (random_c2)
                    {
                        case 1:
                            {
                                button_A.Hide();
                                break;
                            }
                        case 2:
                            {
                                button_B.Hide();
                                break;
                            }
                        case 3:
                            {
                                button_C.Hide();
                                break;
                            }
                        case 4:
                            {
                                button_D.Hide();
                                break;
                            }
                    }
                    break;
                }
            }
            button_50na50.Visible = false;
            button_enabled(false);
            await debrov("Шансы 50 на 50!");
            button_enabled(true);
            button_call.Enabled = false;
            button_x2.Enabled = false;
            button_zall.Enabled = false;
            button_50na50.Enabled = false;
        }
        private async void button_call_Click(object sender, EventArgs e)
        {
            debrov("Надеюсь друг вас не подведет!");
            Sound.SoundProccesing(phone);
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
            }
            if(x2_active.Visible == true)
            {
                x2_active.Visible = false;
            }
            button_call.Visible = false;
            pictureBox5.Show();
            await Task.Delay(TimeSpan.FromSeconds(0.01));
            label5.Show();
            label5.BringToFront();
            button_enabled(false);
            for (int i = 0; i < 6; i++)
            {
                label5.Text += ".";
                if(label5.Text == "Вызов....")
                {
                    label5.Text = "Вызов";
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            pictureBox4.Visible = true;
            label4.Visible = true;
            label4.BringToFront();
            label5.Hide();
            pictureBox5.Hide();
            Button true_b = checked_true_button(data[current_question][6]);
            if (true_b == button_A)
            {
                label4.Text = "A";
            }
            else if (true_b == button_B)
            {
                label4.Text = "B";
            }
            else if (true_b == button_C)
            {
                label4.Text = "C";
            }
            else if (true_b == button_D)
            {
                label4.Text = "D";
            }
            button_enabled(true);
            button_call.Enabled = false;
            button_x2.Enabled = false;
            button_zall.Enabled = false;
            button_50na50.Enabled = false;
        }
        private async void button_x2_Click(object sender, EventArgs e)
        {
            Sound.SoundProccesing(on50);
            x2_active.Show();
            button_x2.Hide();
            if (label4.Visible == true)
            {
                label4.Visible = false;
                pictureBox4.Visible = false;
            }
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
            }
            button_enabled(false);
            await debrov("У вас есть право на ошибку!");
            button_enabled(true);
            button_call.Enabled = false;
            button_x2.Enabled = false;
            button_zall.Enabled = false;
            button_50na50.Enabled = false;
        }
        private async Task zall_process()
        {
            button_zall.Hide();
            panel1.Show();
            label15.Show();
            for (int i = 0; i < 10; i++)
            {
                label15.Text += ".";
                if (label15.Text == "Голосование....")
                {
                    label15.Text = "Голосование";
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            label15.Visible = false;
            label14.Visible = true;
            label6.Visible = true; label7.Visible = true; label8.Visible = true; label9.Visible = true;
            label10.Visible = true; label11.Visible = true; label12.Visible = true; label13.Visible = true;
        }
        private async void button_zall_Click(object sender, EventArgs e)
        {
            Sound.SoundProccesing(sms);
            zall_process();
            button_enabled(false);
            await debrov("Будем надеяться на помощь зала!");
            button_enabled(true);
            button_call.Enabled = false;
            button_x2.Enabled = false;
            button_zall.Enabled = false;
            button_50na50.Enabled = false;
            if (label4.Visible == true)
            {
                label4.Visible = false;
                pictureBox4.Visible = false;
            }
            if (x2_active.Visible == true)
            {
                x2_active.Visible = false;
            }
            int x1, x2, x3, x4;
            System.Windows.Forms.Label LB = new System.Windows.Forms.Label();
            x1 = rnd.Next(50, 90);
            x2 = rnd.Next(1, 100-x1);
            if(x1 + x2 >= 100)
            {
                x3 = 0;
                x4 = 0;
            }
            else
            {
                x3 = rnd.Next(1, 100-(x1+x2));
                if (x1 + x2 + x3 >= 100)
                {
                    x4 = 0;
                }
                else
                {
                    x4 = 100 - (x1 + x2 + x3);
                }
            }

            Button true_b = checked_true_button(data[current_question][6]);
            if (true_b == button_A)
            {
                LB = label10;
            }
            else if (true_b == button_B)
            {
                LB = label11;
            }
            else if (true_b == button_C)
            {
                LB = label12;
            }
            else if (true_b == button_D)
            {
                LB = label13;
            }
            LB.Text = "думает " + x1.ToString() + "% людей";
            if(LB == label10)
            {
                label11.Text = "думает " + x2.ToString() + "% людей";
                label12.Text = "думает " + x3.ToString() + "% людей";
                label13.Text = "думает " + x4.ToString() + "% людей";
            }
            if(LB == label11)
            {
                label10.Text = "думает " + x2.ToString() + "% людей";
                label12.Text = "думает " + x3.ToString() + "% людей";
                label13.Text = "думает " + x4.ToString() + "% людей";
            }
            if(LB == label12)
            {
                label11.Text = "думает " + x2.ToString() + "% людей";
                label10.Text = "думает " + x3.ToString() + "% людей";
                label13.Text = "думает " + x4.ToString() + "% людей";
            }
            if(LB == label13)
            {
                label11.Text = "думает " + x2.ToString() + "% людей";
                label10.Text = "думает " + x3.ToString() + "% людей";
                label12.Text = "думает " + x4.ToString() + "% людей";
            }
        }
        private void good_finish()
        {
            if (balance.balanceCurrent >= data_program.summa_nesgor)
            {
                data_program.Message[0] = "Вы проиграли! Не огорчайтесь!";
                data_program.Message[1] = data_program.game_name;
                data_program.Message[2] = data_program.summa_nesgor.ToString();
            }
            if (current_question == data.Count)
            {
                Sound.SoundProccesing(win);
                data_program.Message[0] = "Поздравляем, вы победили!";
                data_program.Message[1] = data_program.game_name;
                data_program.Message[2] = balance.balanceCurrent.ToString();
            }
            End FG = new End();
            FG.ShowDialog(this);
        }
        private void game_over(Button true_button, Button select_button)
        {
            if (x2_active.Visible == true)
            {
                button_enabled(true);
                select_button.BackColor = ColorTranslator.FromHtml("#180246");
                select_button.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#180246");
                select_button.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#180246");
                pictureBox1.Image = Game_Pict.game_cap;
                select_button.Hide();
                x2_active.Visible = false;
            }
            else
            {
                green_button(true_button, true);
                if (true_button == button_A)
                {
                    if (select_button == button_B)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_a_true_b_false;
                    }
                    if (select_button == button_C)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_a_true_c_false;
                    }
                    if (select_button == button_D)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_a_true_d_false;
                    }
                }
                if (true_button == button_B)
                {
                    if (select_button == button_A)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_b_true_a_false;
                    }
                    if (select_button == button_C)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_b_true_c_false;
                    }
                    if (select_button == button_D)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_b_true_d_false;
                    }
                }
                if (true_button == button_C)
                {
                    if (select_button == button_A)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_c_true_a_false;
                    }
                    if (select_button == button_B)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_c_true_b_false;
                    }
                    if (select_button == button_D)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_c_true_d_false;
                    }
                }
                if (true_button == button_D)
                {
                    if (select_button == button_A)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_d_true_a_false;
                    }
                    if (select_button == button_B)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_d_true_b_false;
                    }
                    if (select_button == button_C)
                    {
                        pictureBox1.Image = Game_Pict.game_cap_select_d_true_c_false;
                    }
                }
                select_button.BackColor = ColorTranslator.FromHtml("#ff0000");
                select_button.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#ff0000");
                select_button.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#ff0000");
                data_program.Message[0] = "Вы проиграли! Не огорчайтесь!";
                data_program.Message[1] = data_program.game_name;
                if (balance.balanceCurrent > data_program.summa_nesgor)
                {
                    data_program.Message[2] = data_program.summa_nesgor.ToString();
                }
                else
                {
                    data_program.Message[2] = "0";
                }
                End FG = new End();
                FG.ShowDialog(this);
            }
        }
        private void initialize_sound()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream resourceStream_start = assembly.GetManifestResourceStream(@"Kursovaya.start_game.wav");
            System.IO.Stream resourceStream_otvet = assembly.GetManifestResourceStream(@"Kursovaya.otvet.wav");
            System.IO.Stream resourceStream_otvet_false = assembly.GetManifestResourceStream(@"Kursovaya.false_answer.wav");
            System.IO.Stream resourceStream_50na50 = assembly.GetManifestResourceStream(@"Kursovaya.50_on_50.wav");
            System.IO.Stream resourceStream_sms = assembly.GetManifestResourceStream(@"Kursovaya.sms.wav");
            System.IO.Stream resourceStream_true_answ = assembly.GetManifestResourceStream(@"Kursovaya.true_answ.wav");
            System.IO.Stream resourceStream_phone = assembly.GetManifestResourceStream(@"Kursovaya.phone.wav");
            System.IO.Stream resourceStream_win = assembly.GetManifestResourceStream(@"Kursovaya.winner.wav");
            start_game = new SoundPlayer(resourceStream_start);
            otvet = new SoundPlayer(resourceStream_otvet);
            false_answer = new SoundPlayer(resourceStream_otvet_false);
            on50 = new SoundPlayer(resourceStream_50na50);
            sms = new SoundPlayer(resourceStream_sms);
            true_answer = new SoundPlayer(resourceStream_true_answ);
            phone = new SoundPlayer(resourceStream_phone);
            win = new SoundPlayer(resourceStream_win);
        }
        async Task debrov(string Message)
        {
            debrov_text.Text = Message;
            debrov_text.Show();
            for(int i = 0;  i < 10; i++)
            {
                vedushiy.Image = Resources.rot_false;
                await Task.Delay(TimeSpan.FromSeconds(0.1));
                vedushiy.Image = Resources.rot_true;
                await Task.Delay(TimeSpan.FromSeconds(0.1));
            }
            debrov_text.Hide();
        }

        private void picture_money_Click(object sender, EventArgs e)
        {
            if (debrov_text.Visible == false)
            {
                DialogResult result = MessageBox.Show(
                    "Вы действительно хотите забрать деньги?",
                    "Забрать деньги",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes)
                {
                    Sound.SoundProccesing(win);
                    data_program.Message[0] = "Вы забрали деньги!";
                    data_program.Message[1] = data_program.game_name;
                    data_program.Message[2] = balance.balanceCurrent.ToString();
                    End FG = new End();
                    FG.ShowDialog(this);
                }
            }
        }

        private void pictureBox_volume_Click(object sender, EventArgs e)
        {
            if (data_program.volume == true)
            {
                pictureBox_volume.Image = Game_Pict.volume_off;
                data_program.volume = false;
            }
            else
            {
                pictureBox_volume.Image = Game_Pict.volume_on;
                data_program.volume = true;
            }
        }
    }
    public class Balance 
    {
        public int balanceCurrent;
        List<int> balance = new List<int>() { 0, 500, 1000, 2000, 3000, 5000, 10000, 15000, 25000, 50000, 100000, 200000, 400000, 800000, 1500000, 3000000 };
        int metka = 0;
        System.Windows.Forms.Label Argument = new System.Windows.Forms.Label();
        public void Upd()
        {
            metka++;
            balanceCurrent = balance[metka];
            Argument.Text = "Баланс: " + balanceCurrent.ToString();
        }
        public void get_argument(System.Windows.Forms.Label LBL)
        {
            Argument = LBL;
        }
    }
    public class _Sound
    {
        public void SoundProccesing(SoundPlayer SP)
        {
            if(data_program.volume==true)
            {
                SP.Play();
            }
        }
    }
}
