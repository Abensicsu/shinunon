using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace DataModels.Models
{
    public class ForumComment
    {
        public int ForumCommentId { get; set; }
        public string Comment { get; set; }

        public int ForumQuestionId { get; set; }

        [ForeignKey(nameof(ForumQuestionId))]
        public ForumQuestion ForumQuestion { get; set; }

        public int? ParentCommentId { get; set; }
        [ForeignKey(nameof(ParentCommentId))]
        public ForumComment ParentComment { get; set; }


        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
