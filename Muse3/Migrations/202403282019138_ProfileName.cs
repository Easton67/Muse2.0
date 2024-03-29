namespace Muse3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProfileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ProfileName");
        }
    }
}
