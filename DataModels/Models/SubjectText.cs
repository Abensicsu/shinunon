using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class SubjectText
    {
        public int SubjectId { get; set; }
        public string? SubjectTextContent { get; set; }

        public Subject Subject { get; set; }
    }
}
