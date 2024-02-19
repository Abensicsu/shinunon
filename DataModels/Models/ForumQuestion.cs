using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public enum ForumQuestionType
    {
        Comment,
        Subject,
        Discussion
    }

    public class ForumQuestion
    {
        public int ForumQuestionId { get; set; }
        public ForumQuestionType ForumQuestionType { get; set; } //Choose comment\subject\discussion
        public string Question { get; set; }

        public int? SubjectId { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; }

        public ICollection<ForumComment> Comments { get; set; }


        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
