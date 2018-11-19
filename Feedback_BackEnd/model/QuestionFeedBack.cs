using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{

    class QuestionFeedBack1
    {

        [Key]
        public string QuestionFeedBackId { get; set; }
      //  public List<UserId1> UserIdTracker{get;set;}
        public int QuestionId{get;set;}

        public int Ambiguity {get; set;} 
    }

}