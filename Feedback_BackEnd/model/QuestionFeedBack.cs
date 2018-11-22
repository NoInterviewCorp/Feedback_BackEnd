using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{
    public class QuestionFeedBack
    {

        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public int Ambiguity { get; set; }

    }
}
