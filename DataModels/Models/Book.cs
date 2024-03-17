using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public enum BookTypeEnum
    {
        Chumash,
        Neviim,
        Ketuvim,
        Gemara,
        Mishna
    }

    public class Book
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public BookTypeEnum BookType { get; set; }
        public bool HasSubSubject { get; set; } //True if has sub-subject
        public int? BookOrder { get; set; } //Order in subject list
        public int SubjectNum { get; set; } //Amount of sub-subjects
    }
}
