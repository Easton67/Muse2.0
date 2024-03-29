namespace Muse3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "GivenName", c => c.String());
            AddColumn("dbo.AspNetUsers", "FamilyName", c => c.String());
            AddColumn("dbo.AspNetUsers", "EmployeeID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "ImageFilePath", c => c.String());
            AddColumn("dbo.AspNetUsers", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "MinutesListened", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MinutesListened");
            DropColumn("dbo.AspNetUsers", "Active");
            DropColumn("dbo.AspNetUsers", "ImageFilePath");
            DropColumn("dbo.AspNetUsers", "EmployeeID");
            DropColumn("dbo.AspNetUsers", "FamilyName");
            DropColumn("dbo.AspNetUsers", "GivenName");
        }
    }
}
