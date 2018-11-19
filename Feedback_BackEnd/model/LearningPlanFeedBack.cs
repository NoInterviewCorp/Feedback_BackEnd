using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{

    class LearningPlanFeedBack1
    {
        [Key]
        public string LearningPlanFeedBackId { get; set; }
        // chane camel case into pascal case
       // public List<UserId1> UserIdTracker{get;set;}
        public string LearningPlanId{get;set;}
        public double AverageStar { get; set;}
      //  public List<StarTrack> StarTracker {get;set;}

        // public bool subscribe {get; set;}

        //public List<SubscribeTrack> SubscribeTracker {get;set;}
        public int totalsubscriber{get;set;}

    }


}

