namespace Muse3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Message = c.String(),
                        UserID = c.Int(nullable: false),
                        SongID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewID)
                .ForeignKey("dbo.Songs", t => t.SongID, cascadeDelete: true)
                .Index(t => t.SongID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "SongID", "dbo.Songs");
            DropIndex("dbo.Reviews", new[] { "SongID" });
            DropTable("dbo.Reviews");
        }
    }
}
