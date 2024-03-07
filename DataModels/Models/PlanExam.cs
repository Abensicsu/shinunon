using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Models
{
    public enum ExamFrequencyEnum
    {
        Daily,
        Weekly,
        Monthly
    }

    public class PlanExam
    {
        public int PlanExamId { get; set; }

        public ExamFrequencyEnum ExamFrequency { get; set; }

        public int SubjectNum { get; set; }

        public int QuestionsAmount { get; set; }
        public int FromSubjectId { get; set; }
        [ForeignKey(nameof(FromSubjectId))]
        public Subject FromSubject { get; set; }

        public int ToSubjectId { get; set; }
        [ForeignKey(nameof(ToSubjectId))]
        public Subject ToSubject { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public ICollection<ExamExecution> ExamExecutions { get; set; }
    }
}
