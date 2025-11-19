using LibraLibraryManagementSystem.Models; // To find the 'User' class
using LibraLibraryManagementSystem.Windows;
using Microsoft.Data.SqlClient; // Must be at the top of the file
using System;
using System.Collections.Generic; // To use Lists
using System.Configuration;    // Must be at the top of the file
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraLibraryManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        public Users()
        {

            // This is a temporary fix to clear the "ghost" items
            // from the cached XAML file before it's initialized.
            // We will remove this later, but it's needed to break the cache.
            if (dgAdmins != null)
            {
                dgAdmins.Items.Clear();
                dgAdmins.ItemsSource = null;
            }
            if (dgStudents != null)
            {
                dgStudents.Items.Clear();
                dgStudents.ItemsSource = null;
            }

            InitializeComponent();
            LoadUsersData(); // <-- This runs the function immediately
        }



        private void LoadUsersData()
        {
            // List to hold ALL users fetched from the database
            List<User> allUsers = new List<User>();

            try
            {
                // 1. Get the connection string from App.config
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                // 2. Define the SQL query: Select all necessary columns from the 'Users' table
                string sqlQuery = "SELECT [School ID], Role, Name, Email, [Contact No.], Password, [Grade & Section] FROM dbo.USERS";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        // 3. Execute the query and read the results row by row
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 4. Create a new User object and map the database columns to your C# model
                                allUsers.Add(new User
                                {
                                    SchoolID = reader.GetString(0), // <-- FIXED
                                    Role = reader.GetString(1),
                                    Name = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    ContactNo = reader.GetString(4),
                                    Password = reader.GetString(5),
                                    GradeSection = reader.IsDBNull(6) ? string.Empty : reader.GetString(6) // Added a safety check
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // If the database is blocked (firewall) or connection string is wrong, this runs.
                MessageBox.Show($"DATABASE ERROR: Failed to load user data. \nDetails: {ex.Message}", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Stop the function here
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show("Configuration Error: Check your App.config file for errors.", "System Error");
                return;
            }
            // 5. Filter the data and bind it to the XAML DataGrids

            // EXPLICITLY CLEAR THE "GHOST" ITEMS
            dgAdmins.Items.Clear();
            dgStudents.Items.Clear();

            // ADD THESE TWO LINES TO CLEAR THE GRIDS FIRST
            dgAdmins.ItemsSource = null;
            dgStudents.ItemsSource = null;

            // 5. Filter the data and bind it to the XAML DataGrids
            dgAdmins.ItemsSource = allUsers.Where(u => u.Role == "Admin").ToList();
            dgStudents.ItemsSource = allUsers.Where(u => u.Role != "Admin").ToList();
        }




        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            // 1. This creates a new instance of your pop-up window
            AddUserWindow addUserWindow = new AddUserWindow();

            // 2. This shows the window as a pop-up
            addUserWindow.ShowDialog();

            LoadUsersData(); // This line now runs, refreshing the tables
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            EditUserWindow editUserWindow = new EditUserWindow();
            editUserWindow.ShowDialog();

            LoadUsersData(); // This line now runs, refreshing the tables
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            DeleteUserWindow deleteUserWindow = new DeleteUserWindow();
            deleteUserWindow.ShowDialog();

            LoadUsersData(); // This line now runs, refreshing the tables
        }
    }
}
