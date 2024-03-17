using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int? Ordinal { get; set; } //like perek / daf

        public ICollection<Question> Questions { get; set; }

        public ICollection<ExamExecution> ExamExecutions { get; set; }
    }
}
