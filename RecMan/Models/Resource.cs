using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace RecMan.Models
{
    public enum EflLevel
    {
        [Description("Beginner")]
        Beginner = 0,
        [Description("Elementary")]
        Elementary = 1,
        [Description("Pre-Intermediate")]
        PreIntermediate = 2,
        [Description("Intermediate")]
        Intermediate = 3,
        [Description("Upper-Intermediate")]
        UpperIntermediate = 4,
        [Description("Advanced")]
        Advanced = 5,
        [Description("Proficient")]
        Proficient = 6
    }

    public enum LanguageFocus
    {
        [Description("Grammar")]
        Grammar = 0,
        [Description("Vocabulary")]
        Vocabulary = 1,
        [Description("Reading")]
        Reading = 2,
        [Description("Listening")]
        Listening = 3,
        [Description("Speaking")]
        Speaking = 4,
        [Description("Writing")]
        Writing = 5,
        [Description("Pronunciation")]
        Pronunciation = 6,
        [Description("Other")]
        Other = 7,
    }
    public class Resource
    {
        public int ResourceID
        { get; set; }
        public String Title { get; set; }
        public String Source { get; set; }
        public EflLevel Level { get; set; }
        public LanguageFocus Focus { get; set; }
        public String Topic { get; set; }
        public String Content { get; set; }
        public int VoteCount { get; set; }

        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<FilePath> FilePaths { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }


    }
}
