using System;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace register_Ca
{
    public partial class Form1 : Form
    {
        private TextBox textBox_login;
        private TextBox textBox_password;
        private Button btnEnter;
        private Button btnRegister;

        public Form1()
        {
            InitializeComponent();
            InitializeLoginForm();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeLoginForm()
        {

            textBox_login = new TextBox()
            {
                Location = new Point(325, 150),
                Size = new Size(150, 35)
            };

            textBox_password = new TextBox()
            {
                Location = new Point(325, 200),
                Size = new Size(150, 35),
                PasswordChar = '●'
            };

            // Создание кнопок входа и регистрации
            btnEnter = new Button()
            {
                Text = "Log In",
                Location = new Point(320, 235),
                Size = new Size(70, 33)
            };
            btnEnter.Click += BtnEnterClick;

            btnRegister = new Button()
            {
                Text = "Register",
                Location = new Point(410, 235),
                Size = new Size(70, 33)
            };
            btnRegister.Click += BtnRegisterClick;

            // Добавление элементов на форму
            Controls.Add(textBox_login);
            Controls.Add(textBox_password);
            Controls.Add(btnEnter);
            Controls.Add(btnRegister);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Код из Form1_Load
            textBox_password.PasswordChar = '*';
            textBox_login.MaxLength = 50;
            textBox_password.MaxLength = 50;
        }

        private void BtnEnterClick(object sender, EventArgs e)
        {
            var loginUser = textBox_login.Text;
            var passUser = textBox_password.Text;

            string connectionString = "Server=DESKTOP-QNI5V5S;Database=registration_DB;Trusted_Connection=True;Encrypt=False;";
            // Создание соединения с базой данных
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Создание строки запроса
                string querystring = $"select id_user, login_user, password_user from register where login_user = '{loginUser}' and password_user = '{passUser}'";

                SqlDataAdapter adapter = new SqlDataAdapter(querystring, connection);
                DataTable table = new DataTable();

                adapter.Fill(table);

                if (table.Rows.Count == 1)
                {
                    MessageBox.Show("You are successfully join!", "Successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // После успешного входа вы можете открыть новую форму или выполнить другие действия
                }
                else
                {
                    MessageBox.Show("This account doesn't exist!", "Account doesn't exist!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BtnRegisterClick(object sender, EventArgs e)
        {
            var loginUser = textBox_login.Text;
            var passUser = textBox_password.Text;

            string connectionString = "Server=DESKTOP-QNI5V5S;Database=registration_DB;Trusted_Connection=True;Encrypt=False;";
            // Создание соединения с базой данных
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Проверка существования пользователя с таким логином
                string checkUserQuery = $"SELECT COUNT(*) FROM register WHERE login_user = '{loginUser}'";
                SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection);

                try
                {
                    connection.Open();
                    int userCount = (int)checkUserCommand.ExecuteScalar();
                    if (userCount > 0)
                    {
                        MessageBox.Show("User with this login already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Прерываем выполнение метода, чтобы не продолжать регистрацию
                    }

                    // Если пользователь с таким логином не существует, регистрируем нового пользователя
                    string insertQuery = $"INSERT INTO register (login_user, password_user) VALUES ('{loginUser}', '{passUser}')";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Очистка полей логина и пароля
                        textBox_login.Text = "";
                        textBox_password.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Registration failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred during registration: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }



    }
}
