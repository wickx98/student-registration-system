using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Simple_Login_FORM
{
    public partial class LoginForm : Form
    {
        //connection string with your real SQL Server setup
        SqlConnection con = new SqlConnection(@"Data Source=localhost;Initial Catalog=Student;Integrated Security=True");

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            UsernameBox.Focus();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm regForm = new RegisterForm();
            regForm.Show();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM LoginForm WHERE username = @username AND password = @password";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", UsernameBox.Text.Trim());
                cmd.Parameters.AddWithValue("@password", PasswordBox.Text.Trim());

                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Login successful! Now redirecting to student registration.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();

                    // Go to student registration form
                    RegisterForm regForm = new RegisterForm();
                    regForm.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Clear input fields and focus
                    UsernameBox.Clear();
                    PasswordBox.Clear();
                    UsernameBox.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }



        // 🔘 Clear Button
        private void button1_Click(object sender, EventArgs e)
        {
            UsernameBox.Clear();
            PasswordBox.Clear();
            UsernameBox.Focus();
        }

        // 🔘 Exit Button
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
