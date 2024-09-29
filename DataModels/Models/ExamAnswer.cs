using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public enum AnswerCorrectnessLevelEnum
    {
        Correct,
        PartiallyCorrect,
        Wrong
    }

    public class ExamAnswer
    {
        public int ExamAnswerId { get; set; }
        public string? TextAnswer { get; set; }
        public TimeSpan TimeSpent { get; set; }

        public AnswerCorrectnessLevelEnum? AnswerCorrectnessLevel { get; set; }

        public int? AnswerId { get; set; }
        [ForeignKey(nameof(AnswerId))]
        public Answer Answer { get; set; }

        public int? BaseQuestionId { get; set; }
        [ForeignKey(nameof(BaseQuestionId))]
        public BaseQuestion BaseQuestion { get; set; }

        public int ExamExecutionId { get; set; }
        [ForeignKey(nameof(ExamExecutionId))]
        public ExamExecution ExamExecution { get; set; }
    }
}
