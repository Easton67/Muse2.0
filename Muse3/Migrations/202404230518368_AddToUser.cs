namespace Muse3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Photo", c => c.Binary());
            AddColumn("dbo.AspNetUsers", "PhotoMimeType", c => c.String());
            AddColumn("dbo.AspNetUsers", "isPublic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "isPublic");
            DropColumn("dbo.AspNetUsers", "PhotoMimeType");
            DropColumn("dbo.AspNetUsers", "Photo");
        }
    }
}
