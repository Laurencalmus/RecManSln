using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResourceManagement.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RecMan.DAL
{
    public class RecManContext : DbContext
    {
        public RecManContext() : base("ResourceManagementContext")
        {
        }

        public DbSet<Models.Resource> Resources { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<Models.FilePath> FilePaths { get; set; }
        public DbSet<Models.Comment> Comments { get; set; }

    }
}