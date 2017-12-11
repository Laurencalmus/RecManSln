using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecMan.Models
{
    public class CommentCreateViewModel
    {
        public int ID { get; set; }
        public String Message { get; set; }
        public String UserId { get; set; }
        public String UserName { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? DateTimeEdit { get; set; }
        public int ResourceID { get; set; }
        public virtual Resource Resource { get; set; }

    }
}