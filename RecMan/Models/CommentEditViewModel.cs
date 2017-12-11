using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecMan.Models
{
    public class CommentEditViewModel
    {
        public int ID { get; set; }
        public String Message { get; set; }
        public String UserId { get; set; }
        public String UserName { get; set; }
        public DateTime? DateTimeEdit { get; set; }
        public int ResourceID { get; set; }
        public virtual Resource Resource { get; set; }
    }
}