namespace Resume_Builder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedResumeDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resumes", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resumes", "Description");
        }
    }
}
