using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecMan.Models
{
    public class Vote
    {
        public Guid VoteId { get; set; }
        public string UserId { get; set; }
        public int ResourceId { get; set; }
        public virtual Resource Resource { get; set; }
    }
}