namespace localshop.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Status_Edit_Product : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "ShortDesciption", c => c.String());
            AddColumn("dbo.Products", "LongDescription", c => c.String());
            AddColumn("dbo.Products", "StatusId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Products", "StatusId");
            AddForeignKey("dbo.Products", "StatusId", "dbo.Status", "Id");
            DropColumn("dbo.Products", "Description");
            DropColumn("dbo.Products", "Detail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Detail", c => c.String());
            AddColumn("dbo.Products", "Description", c => c.String());
            DropForeignKey("dbo.Products", "StatusId", "dbo.Status");
            DropIndex("dbo.Products", new[] { "StatusId" });
            DropColumn("dbo.Products", "StatusId");
            DropColumn("dbo.Products", "LongDescription");
            DropColumn("dbo.Products", "ShortDesciption");
            DropTable("dbo.Status");
        }
    }
}
