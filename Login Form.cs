using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySqlConnector;
using static C969_Performance_Assessment.Database.DBConnection;
using System.Globalization;
using System.Resources;
using System.Diagnostics;

namespace C969_Performance_Assessment
{
    public partial class LoginForm : Form
    {
        public static LoginForm Instance { get; } = new LoginForm();

        private readonly CultureInfo currentCulture;

        private string User { get; set; }
        private string Pass { get; set; }

        private string DBUser { get; set; }
        private string DBPass { get; set; }

        private string loginCorrect;
        private string loginError;
        private string formEmpty;

        public LoginForm()
        {
            InitializeComponent();
        }

        public LoginForm(CultureInfo currentCulture)
        {
            this.currentCulture = currentCulture;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResourceManager rm;

            if (CultureInfo.CurrentUICulture.Name == "es-MX")
            {
                rm = new ResourceManager("C969_Performance_Assessment.Strings_es-MX", typeof(LoginForm).Assembly);
            }
            else
            {
                rm = new ResourceManager("C969_Performance_Assessment.Strings_en-US", typeof(LoginForm).Assembly);
            }

            cancelButton.Text = rm.GetString("CancelButton");
            loginButton.Text = rm.GetString("LoginButton");
            userLabel.Text = rm.GetString("UsernameLabel");
            passLabel.Text = rm.GetString("PasswordLabel");
            loginLabel.Text = rm.GetString("LoginMessage");
            loginCorrect = rm.GetString("LoginCorrect");
            loginError = rm.GetString("LoginError");
            formEmpty = rm.GetString("FormEmpty");

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (passBox.Text == "" || userBox.Text == "")
            {
                MessageBox.Show(formEmpty);
                return;
            }

            User = userBox.Text;
            Pass = passBox.Text;
            

            string checkUsernamePassword = "SELECT userName, password FROM client_schedule.user WHERE userName=@User AND password=@Pass";

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = checkUsernamePassword;
            cmd.Parameters.AddWithValue("@User", User);
            cmd.Parameters.AddWithValue("@Pass", Pass);
            cmd.Connection = conn;

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    DBUser = reader["userName"].ToString();
                    DBPass = reader["password"].ToString();
                }
                
            }

            if (User == DBUser && Pass == DBPass)
            {
                CurrentUser.instance.Name = User;

                MessageBox.Show(loginCorrect);
                                
                MainForm newForm = new MainForm();
                newForm.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show(loginError);
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}



