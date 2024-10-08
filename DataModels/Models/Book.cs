﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string HebrewBookName { get; set; }
        public BookTypeEnum BookType { get; set; }
        public bool HasSubSubject { get; set; }
        public int BookOrder { get; set; }
        public ICollection<Subject> Subjects { get; set; }
    }

    public enum BookTypeEnum
    {
        Chumash,
        Neviim,
        Ketuvim,
        Gemara,
        Mishna
    }
}
