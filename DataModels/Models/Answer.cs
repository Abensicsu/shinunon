﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrectAnswer { get; set; }

        public int BaseQuestionId { get; set; }
        [ForeignKey(nameof(BaseQuestionId))]
        public BaseQuestion BaseQuestion { get; set; }
    }
}
