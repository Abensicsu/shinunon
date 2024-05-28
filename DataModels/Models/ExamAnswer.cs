using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class ExamAnswer
    {
        public int ExamAnswerId { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }

        public int? AnswerId { get; set; }
        [ForeignKey(nameof(AnswerId))]
        public Answer Answer { get; set; }
        public string TextAnswer { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public int ExamExecutionId { get; set; }
        [ForeignKey(nameof(ExamExecutionId))]
        public ExamExecution ExamExecution { get; set; }
    }
}
