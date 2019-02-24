using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapi.Models
{
    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string timezone { get; set; }
        public string phone { get; set; }
        public string about { get; set; }
        public string image { get; set; }
        
    }
}