using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using Npgsql.Internal;

namespace Задание_2
{
    public partial class Авторизация : Form
    {
        private System.Windows.Forms.Timer unlockTimer;
        public bool DB(string x, string c)
        {

            NpgsqlConnection conn = new NpgsqlConnection("User ID = postgres; Password = 12345; Host = localhost; Port = 5432;  Database = postgres;");
            conn.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string sql = ($"SELECT * FROM polz WHERE polzlogin = '{x}' and polzpassword = '{c}';");
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
            ds.Reset();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else 
            {
                return true;
            }
            
            conn.Close();




        }
            

        public Авторизация()
        {
            InitializeComponent();
            unlockTimer = new System.Windows.Forms.Timer();
            unlockTimer.Tick += unlockTimer_Tick;
        }

       
        private int invalidAttempts = 0;
        private DateTime lockoutEndTime;

        private void unlockTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now >= lockoutEndTime)
            {
                // Тайм-аут окончен
                invalidAttempts = 0;
                button1.Enabled = true;
                label3.Text = "Авторизация в системе";
                unlockTimer.Stop();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == default ^ textBox1.Text == string.Empty)
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка!");

            }
            else if (textBox2.Text == default ^ textBox2.Text == string.Empty)
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка!");

            }
            else
            {
                bool x = DB(textBox1.Text, textBox2.Text);
                if (x == true)
                {
                    MessageBox.Show("Вы успешно авторизировались!", "Ураа!");
                    textBox1.Clear();
                    textBox2.Clear();
                }
                else if (x == false)
                {
                    MessageBox.Show("Неверный логин или пароль","Ошибка!");
                    invalidAttempts++;
                    textBox1.Clear();
                    textBox2.Clear();
                    if (invalidAttempts >= 3)
                    {
                        lockoutEndTime = DateTime.Now.AddSeconds(20);
                        button1.Enabled = false;

                        // Запуск таймера для разблокировки через 20 секунд
                        label3.Text = "Ошибка! Попробуйте позже.";
                        
                        unlockTimer.Start();
                    }
                }
            }
                   
            
            
            
        }
    }
}
