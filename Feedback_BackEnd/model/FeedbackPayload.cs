using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{
    public class GiveStarPayload
    {
        public int Rating{get;set;}
        public bool Subscribe{get;set;}
    }
}
