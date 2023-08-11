namespace MyEvernote.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 150),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUserName = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 70),
                        Text = c.String(nullable: false, maxLength: 2000),
                        IsDraft = c.Boolean(nullable: false),
                        LikeCount = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUserName = c.String(nullable: false, maxLength: 30),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.EvernoteUsers", t => t.Owner_Id)
                .Index(t => t.CategoryId)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 300),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUserName = c.String(nullable: false, maxLength: 30),
                        Note_Id = c.Int(),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notes", t => t.Note_Id)
                .ForeignKey("dbo.EvernoteUsers", t => t.Owner_Id)
                .Index(t => t.Note_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.EvernoteUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        SurName = c.String(maxLength: 25),
                        UserName = c.String(maxLength: 25),
                        Email = c.String(nullable: false, maxLength: 70),
                        Password = c.String(nullable: false, maxLength: 25),
                        ProfileImageFileName = c.String(maxLength: 75),
                        IsActive = c.Boolean(nullable: false),
                        ActivateGuid = c.Guid(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedUserName = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LikedUser_Id = c.Int(),
                        Note_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EvernoteUsers", t => t.LikedUser_Id)
                .ForeignKey("dbo.Notes", t => t.Note_Id)
                .Index(t => t.LikedUser_Id)
                .Index(t => t.Note_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "Owner_Id", "dbo.EvernoteUsers");
            DropForeignKey("dbo.Likes", "Note_Id", "dbo.Notes");
            DropForeignKey("dbo.Likes", "LikedUser_Id", "dbo.EvernoteUsers");
            DropForeignKey("dbo.Comments", "Owner_Id", "dbo.EvernoteUsers");
            DropForeignKey("dbo.Comments", "Note_Id", "dbo.Notes");
            DropForeignKey("dbo.Notes", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Likes", new[] { "Note_Id" });
            DropIndex("dbo.Likes", new[] { "LikedUser_Id" });
            DropIndex("dbo.Comments", new[] { "Owner_Id" });
            DropIndex("dbo.Comments", new[] { "Note_Id" });
            DropIndex("dbo.Notes", new[] { "Owner_Id" });
            DropIndex("dbo.Notes", new[] { "CategoryId" });
            DropTable("dbo.Likes");
            DropTable("dbo.EvernoteUsers");
            DropTable("dbo.Comments");
            DropTable("dbo.Notes");
            DropTable("dbo.Categories");
        }
    }
}
