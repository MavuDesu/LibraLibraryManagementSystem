using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace LibraLibraryManagementSystem.Windows
{
    /// <summary>
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        public EditUserWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string schoolID = txtSchoolID.Text;
            string name = txtName.Text;
            string email = txtEmail.Text;
            string contactNo = txtContactNo.Text;
            string gradeSection = txtGradeSection.Text;
            string password = txtPassword.Password;
            string userType = rbStandard.IsChecked == true ? "Standard" : "Admin";

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Name and Email are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            // Update Query with CORRECT column names
            string sqlQuery = "UPDATE dbo.USERS SET Role = @Role, Name = @Name, Email = @Email, " +
                              "[Contact No.] = @Contact, Password = @Pass, [Grade & Section] = @Grade " +
                              "WHERE [School ID] = @ID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Role", userType);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Contact", contactNo);
                        command.Parameters.AddWithValue("@Pass", password);
                        command.Parameters.AddWithValue("@Grade", gradeSection);
                        command.Parameters.AddWithValue("@ID", schoolID);

                        int rows = command.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("User updated successfully!", "Success");
                            this.Close();
                        }
                        else MessageBox.Show("Update failed. User not found.");
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
