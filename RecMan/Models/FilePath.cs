namespace RecMan.Models
{
    using System.ComponentModel.DataAnnotations;
    public class FilePath
    {
        public int FilePathId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public int ResourceID { get; set; }
        public virtual Resource Resource { get; set; }
    }
}