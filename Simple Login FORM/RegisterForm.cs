using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Simple_Login_FORM
{
    public partial class RegisterForm : Form
    {
        private readonly SqlConnection _connection = new SqlConnection(@"Data Source=localhost;Initial Catalog=Student;Integrated Security=True");

        public RegisterForm()
        {
            InitializeComponent();
            LoadRegistrationNumbers();
            regNoComboBox.SelectedIndexChanged += regNoComboBox_SelectedIndexChanged;
        }

        // Load Reg Numbers into ComboBox
        private void LoadRegistrationNumbers()
        {
            regNoComboBox.Items.Clear();

            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT regNo FROM Registration", _connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        regNoComboBox.Items.Add(reader["regNo"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading registration numbers: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        // Clear form fields
        private void ClearFields()
        {
            regNoComboBox.Text = "";
            firstNameBox.Clear();
            lastNameBox.Clear();
            addressBox.Clear();
            emailBox.Clear();
            mobileBox.Clear();
            homeBox.Clear();
            parentBox.Clear();
            nicBox.Clear();
            contactBox.Clear();
            datePicker.Value = DateTime.Now;
            maleRadio.Checked = false;
            femaleRadio.Checked = false;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            Hide();
            new LoginForm().Show();
        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            Hide();
            new LoginForm().Show();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private SqlCommand BuildSqlCommand(string query)
        {
            SqlCommand cmd = new SqlCommand(query, _connection);

            cmd.Parameters.AddWithValue("@regNo", int.Parse(regNoComboBox.Text));
            cmd.Parameters.AddWithValue("@firstName", firstNameBox.Text);
            cmd.Parameters.AddWithValue("@lastName", lastNameBox.Text);
            cmd.Parameters.AddWithValue("@dob", datePicker.Value);
            cmd.Parameters.AddWithValue("@gender", maleRadio.Checked ? "Male" : "Female");
            cmd.Parameters.AddWithValue("@address", addressBox.Text);
            cmd.Parameters.AddWithValue("@email", emailBox.Text);
            cmd.Parameters.AddWithValue("@mobile", int.Parse(mobileBox.Text));
            cmd.Parameters.AddWithValue("@home", int.Parse(homeBox.Text));
            cmd.Parameters.AddWithValue("@parent", parentBox.Text);
            cmd.Parameters.AddWithValue("@nic", nicBox.Text);
            cmd.Parameters.AddWithValue("@contact", int.Parse(contactBox.Text));

            return cmd;
        }

        private void ExecuteDatabaseCommand(SqlCommand cmd, string successMessage)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                MessageBox.Show(rowsAffected > 0 ? successMessage : "Operation failed.", rowsAffected > 0 ? "Success" : "Failure");

                if (rowsAffected > 0)
                {
                    ClearFields();
                    LoadRegistrationNumbers();  // Refresh combo box
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string insertQuery = @"INSERT INTO Registration 
                (regNo, firstName, lastName, dateOfBirth, gender, address, email, mobilePhone, homePhone, parentName, nic, contactNo) 
                VALUES (@regNo, @firstName, @lastName, @dob, @gender, @address, @email, @mobile, @home, @parent, @nic, @contact)";

            SqlCommand cmd = BuildSqlCommand(insertQuery);
            ExecuteDatabaseCommand(cmd, "Student Registered Successfully!");
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(regNoComboBox.Text))
            {
                MessageBox.Show("Please select a Registration Number to update.");
                return;
            }

            string updateQuery = @"UPDATE Registration SET 
                firstName = @firstName,
                lastName = @lastName,
                dateOfBirth = @dob,
                gender = @gender,
                address = @address,
                email = @email,
                mobilePhone = @mobile,
                homePhone = @home,
                parentName = @parent,
                nic = @nic,
                contactNo = @contact
                WHERE regNo = @regNo";

            SqlCommand cmd = BuildSqlCommand(updateQuery);
            ExecuteDatabaseCommand(cmd, "Student details updated successfully.");
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(regNoComboBox.Text))
            {
                MessageBox.Show("Please select a Registration Number to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM Registration WHERE regNo = @regNo", _connection);
                cmd.Parameters.AddWithValue("@regNo", int.Parse(regNoComboBox.Text));

                int rowsAffected = cmd.ExecuteNonQuery();

                MessageBox.Show(rowsAffected > 0 ? "Student record deleted successfully." : "No record found.", rowsAffected > 0 ? "Deleted" : "Error");

                if (rowsAffected > 0)
                {
                    ClearFields();
                    LoadRegistrationNumbers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        private void regNoComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(regNoComboBox.Text))
                return;

            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Registration WHERE regNo = @regNo", _connection);
                cmd.Parameters.AddWithValue("@regNo", int.Parse(regNoComboBox.Text));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        firstNameBox.Text = reader["firstName"].ToString();
                        lastNameBox.Text = reader["lastName"].ToString();
                        datePicker.Value = Convert.ToDateTime(reader["dateOfBirth"]);
                        addressBox.Text = reader["address"].ToString();
                        emailBox.Text = reader["email"].ToString();
                        mobileBox.Text = reader["mobilePhone"].ToString();
                        homeBox.Text = reader["homePhone"].ToString();
                        parentBox.Text = reader["parentName"].ToString();
                        nicBox.Text = reader["nic"].ToString();
                        contactBox.Text = reader["contactNo"].ToString();

                        string gender = reader["gender"].ToString();
                        maleRadio.Checked = gender == "Male";
                        femaleRadio.Checked = gender == "Female";
                    }
                    else
                    {
                        MessageBox.Show("No student found with this Registration Number.", "Not Found");
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student details: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            // You can leave this empty or add code if needed
        }
    }
}
