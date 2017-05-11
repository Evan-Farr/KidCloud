namespace KidCloudProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedhalfandfulldayoptiononchildanddailyrateondaycare : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Children", "UserId_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Children", new[] { "UserId_Id" });
            AddColumn("dbo.Children", "HalfDay", c => c.Boolean(nullable: false));
            AddColumn("dbo.Children", "FullDay", c => c.Boolean(nullable: false));
            AddColumn("dbo.DayCares", "DailyRatePerChild", c => c.Double(nullable: false));
            DropColumn("dbo.Children", "UserId_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Children", "UserId_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.DayCares", "DailyRatePerChild");
            DropColumn("dbo.Children", "FullDay");
            DropColumn("dbo.Children", "HalfDay");
            CreateIndex("dbo.Children", "UserId_Id");
            AddForeignKey("dbo.Children", "UserId_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
