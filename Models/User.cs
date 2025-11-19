using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraLibraryManagementSystem.Models
{
    public class User
    {
        // Properties must match the columns in your database tables
        public string SchoolID { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; //admin or standard
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty; // Fixed!
        public string Password { get; set; } = string.Empty;

        // This property is specifically for Students
        public string GradeSection { get; set; } = string.Empty;
    }
}