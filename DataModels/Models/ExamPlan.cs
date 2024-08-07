using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Models
{
    public enum ExamFrequencyEnum
    {
        Daily,
        Weekly,
        Monthly
    }

    public class ExamPlan
    {
        public int ExamPlanId { get; set; }
        public ExamFrequencyEnum ExamFrequency { get; set; }

        //How many per ExamFrequency, can choose both.
        public int SubjectNum { get; set; } //per subject
        public int SubSubjectNum { get; set; } //per sub-subject

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
