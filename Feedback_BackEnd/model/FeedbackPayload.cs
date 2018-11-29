using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{
    public class GiveStarPayload
    {
        public int Rating { get; set; }
        public int Subscribe { get; set; }
        public int Ambigous { get; set; }
    }
}
