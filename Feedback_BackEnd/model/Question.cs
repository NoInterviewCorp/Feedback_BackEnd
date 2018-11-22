using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string technology { get; set; }
        public int Total_Ambiguity { get; set; }
    }
}
