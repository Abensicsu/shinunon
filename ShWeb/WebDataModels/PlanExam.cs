using DataModels.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShWeb.WebDataModels
{
    public enum ExamFrequency
    {
        Daily,
        Weekly,
        Monthly
    }


    public class PlanExam
    {
        public int PlanExamId { get; set; }

        public ExamFrequency ExamFrequency { get; set; }

        //ordinal part - Which ordinal in every frequency
        public int OrdinalPerFrequency { get; set; }

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
