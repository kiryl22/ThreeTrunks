namespace ThreeTrunks.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        IsGallery = c.Boolean(nullable: false),
                        IsBasket = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CategoryUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FilePath = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        FolowUrl = c.String(),
                        IsCarouselImage = c.Boolean(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ImageCategories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SettingKey = c.String(),
                        SettingValue = c.String(),
                        SettingType = c.Int(nullable: false),
                        SettingCaption = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "CategoryId", "dbo.ImageCategories");
            DropIndex("dbo.Images", new[] { "CategoryId" });
            DropTable("dbo.Users");
            DropTable("dbo.Settings");
            DropTable("dbo.Images");
            DropTable("dbo.ImageCategories");
        }
    }
}
