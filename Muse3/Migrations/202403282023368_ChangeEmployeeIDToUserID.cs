namespace Muse3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEmployeeIDToUserID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserID", c => c.Int());
            DropColumn("dbo.AspNetUsers", "EmployeeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "EmployeeID", c => c.Int());
            DropColumn("dbo.AspNetUsers", "UserID");
        }
    }
}
