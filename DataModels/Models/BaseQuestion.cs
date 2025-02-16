using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public enum QuestionTypeEnum
    {
        SelectQuestion,
        OpenQuestion
    }

    public abstract class BaseQuestion : IAuditable
    {
        public int BaseQuestionId { get; set; }
        public string QuestionText { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }

        public string DiscriminatorRF { get; set; }

        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; }
        public string Hint { get; set; }
        public ICollection<Answer> Answers { get; set; }

        public DateTime DateCreatedAudit { get; set; }
        public DateTime LastUpdatedAudit { get; set; }
    }
}
