using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string RDescription { get; set; }
        public float AvgRating { get; set; }
    }
}
