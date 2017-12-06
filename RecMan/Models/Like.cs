using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecMan.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public string UserId { get; set; }
        public Boolean Liked { get; set; }
        public int ResourceId { get; set; }
        public virtual Resource Resource { get; set; }
    }
}