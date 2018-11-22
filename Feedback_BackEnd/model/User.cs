using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace feedBack
{
    public class User
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
    }
}
