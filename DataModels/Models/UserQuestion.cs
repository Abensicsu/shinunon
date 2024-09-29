using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class UserQuestion : BaseQuestion
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public int? OriginalQuestionId { get; set; }
        public Question Question { get; set; }

        // Inherits Answers collection from BaseQuestion
    }
}
