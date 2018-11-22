using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{
    public class ResourceFeedBack
    {

        public int ResourceId { get; set; }
        public int UserId { get; set; }
        public int Star { get; set; }

    }
}
