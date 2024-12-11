using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class Question : BaseQuestion, IAuditable
    {
        public ICollection<UserQuestion> DerivedUserQuestions { get; set; }
        // Audit Fields
        public DateTime DateCreatedAudit { get; set; }
        public DateTime LastUpdatedAudit { get; set; }
    }
}
