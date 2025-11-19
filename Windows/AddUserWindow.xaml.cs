using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace LibraLibraryManagementSystem.Windows // <--- Check this!
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // 1. Get values
            string schoolID = txtSchoolID.Text;
            string name = txtName.Text;
            string email = txtEmail.Text;
            string contactNo = txtContactNo.Text;
            string gradeSection = txtGradeSection.Text;
            string password = txtPassword.Password;
            string userType = rbStandard.IsChecked == true ? "Standard" : "Admin";

            // 2. Validation
            if (string.IsNullOrWhiteSpace(schoolID) || string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 3. Database Insert (Using CORRECT column names with spaces)
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            // Note: We added [ ] around names with spaces like [School ID]
            string sqlQuery = "INSERT INTO dbo.USERS ([School ID], Role, Username, Name, Email, [Contact No.], Password, [Grade & Section]) " +
                              "VALUES (@ID, @Role, @Username, @Name, @Email, @Contact, @Pass, @Grade);";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", schoolID);
                        command.Parameters.AddWithValue("@Role", userType);
                        command.Parameters.AddWithValue("@Username", name); // Using Name as Username for now
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Contact", contactNo);
                        command.Parameters.AddWithValue("@Pass", password);
                        command.Parameters.AddWithValue("@Grade", gradeSection);

                        command.ExecuteNonQuery();
                        MessageBox.Show($"User '{name}' added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
