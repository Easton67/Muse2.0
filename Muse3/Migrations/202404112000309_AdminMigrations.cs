namespace Muse3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        SongID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ImageFilePath = c.String(),
                        Photo = c.Binary(),
                        PhotoMimeType = c.String(),
                        Mp3FilePath = c.String(),
                        YearReleased = c.Int(nullable: false),
                        Lyrics = c.String(),
                        Explicit = c.Boolean(nullable: false),
                        Genre = c.String(),
                        Plays = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        Artist = c.String(),
                        Album = c.String(),
                        DateUploaded = c.DateTime(),
                        DateAdded = c.DateTime(nullable: false),
                        isLiked = c.Boolean(nullable: false),
                        isPublic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SongID);
        }
        
        public override void Down()
        {
            DropTable("dbo.Songs");
        }
    }
}
