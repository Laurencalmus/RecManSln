using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecMan.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public String Message { get; set; }
        public int UserId { get; set; }
        public String UserName { get; set; }
        public int ResourceId { get; set; }
        public virtual Resource Resource { get; set; }
    }
}