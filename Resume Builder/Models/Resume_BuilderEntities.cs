using System.Data.Entity;

namespace Resume_Builder.Models
{
    public class Resume_BuilderEntities : DbContext
    {
        public Resume_BuilderEntities()
            : base("ResumeEntities")
        { }

        public DbSet<Resume_Builder.Models.Resume> Resume { get; set; }
    }
}