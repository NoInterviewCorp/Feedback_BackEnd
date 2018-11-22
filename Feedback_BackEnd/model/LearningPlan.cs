using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{
    public class LearningPlan
    {

        public int LearningPlanId { get; set; }
        public string technology { get; set; }
        public float AvgRating { get; set; }
        public int Subscriber { get; set; }

    }
}
