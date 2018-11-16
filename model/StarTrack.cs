using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace feedBack
{
    class StarTrack1
{
    [Key]
    public string StarTrackId { get; set; }
   // public string UserId {get; set;}
    //public List<UserId1> UserIdTracker{get;set;}
    public int Star {get; set;}
    public string LearningPlanFeedBackId { get; set; }
    public string ResourceFeedBackId { get; set; }
     


}

}

