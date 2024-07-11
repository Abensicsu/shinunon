using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public enum ExamTypeEnum
    {
        InitiatedExam,
        RepeatExam,
        ReviewExam
    }

    public class ExamExecution
    {
        public int ExamExecutionId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public int QuestionsAnswered { get; set; }

        public ExamTypeEnum ExamType { get; set; }
        public int? ExamRepeatNumber { get; set; }  //How many times has the exam been done before?
        public bool? IsReviewed { get; set; }
        public int? QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public Question CurrentQuestion { get; set; }

        public int FromSubjectId { get; set; }
        [ForeignKey(nameof(FromSubjectId))]
        public Subject FromSubject { get; set; }

        public int ToSubjectId { get; set; }
        [ForeignKey(nameof(ToSubjectId))]
        public Subject ToSubject { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int? PlanExamId { get; set; }
        [ForeignKey(nameof(PlanExamId))]
        public PlanExam? PlanExam { get; set; }

        public ICollection<ExamAnswer> ExamAnswers { get; set; }
    }
}
