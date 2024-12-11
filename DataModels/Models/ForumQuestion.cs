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


    public class ForumQuestion : IAuditable
    {
        public int ForumQuestionId { get; set; }
        public ForumQuestionType ForumQuestionType { get; set; } //types comment\subject\discussion
        public string ForumQuestionText { get; set; }

        public string ForumQuestionDescription { get; set; }

        public int? SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; }

        public ICollection<ForumComment> Comments { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int ViewsCount { get; set; } = 0;
        public DateTime CreateDate { get; set; }

        public DateTime DateCreatedAudit { get; set; }
        public DateTime LastUpdatedAudit { get; set; }
    }
}
