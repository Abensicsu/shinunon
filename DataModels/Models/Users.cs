using DataModels.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
        public int UserId { get; set; }
        public string Password { get; set; }

        public UserSettings UserSettings { get; set; }
        public ICollection<ExamExecution> ExamExecutions { get; set; }

        public ICollection<UserQuestion> UserQuestions { get; set; }
    }
}
