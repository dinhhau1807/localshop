﻿namespace localshop.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Product_Add_IsFeatured : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsFeatured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsFeatured");
        }
    }
}
