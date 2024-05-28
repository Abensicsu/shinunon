using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public int? SubSubjectCount { get; set; } 
        public ICollection<Question> Questions { get; set; }

        public ICollection<ExamExecution> ExamExecutions { get; set; }

        public int BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
    }
}
