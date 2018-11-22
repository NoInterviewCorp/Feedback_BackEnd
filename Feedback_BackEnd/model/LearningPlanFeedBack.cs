using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{

    public class LearningPlanFeedBack
    {
        public int LearningPlanFeedBackId { get; set; }
        public int LearningPlanId { get; set; }
        public int UserId { get; set; }
        public int Star { get; set; }
        public int subscribe { get; set; }

    }
}
