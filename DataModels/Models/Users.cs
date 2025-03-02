using DataModels.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class Role : IdentityRole<int>
    {

    }
    public class User : IdentityUser<int>
    {
        //public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserSettings UserSettings { get; set; }
        public ICollection<ExamExecution> ExamExecutions { get; set; }

        public ICollection<UserQuestion> UserQuestions { get; set; }
    }

    public class RegisterRequest
    {
        //[Required(ErrorMessage = "שם המשתמש הוא שדה חובה")]
        public string UserFullName { get; set; }

        //[Required(ErrorMessage = "האימייל הוא שדה חובה")]
        //[EmailAddress(ErrorMessage = "נא להזין כתובת אימייל תקינה.")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "הסיסמה היא שדה חובה")]
        //[MinLength(6, ErrorMessage = "הסיסמה חייבת להיות באורך של לפחות 8 תווים.")]
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
