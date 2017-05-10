namespace KidCloudProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedlistofpendingapplicationstodaycare : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Parents", "DayCareId_Id", "dbo.DayCares");
            AddColumn("dbo.Parents", "DayCare_Id", c => c.Int());
            AddColumn("dbo.Parents", "DayCare_Id1", c => c.Int());
            CreateIndex("dbo.Parents", "DayCare_Id");
            CreateIndex("dbo.Parents", "DayCare_Id1");
            AddForeignKey("dbo.Parents", "DayCare_Id1", "dbo.DayCares", "Id");
            AddForeignKey("dbo.Parents", "DayCare_Id", "dbo.DayCares", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Parents", "DayCare_Id", "dbo.DayCares");
            DropForeignKey("dbo.Parents", "DayCare_Id1", "dbo.DayCares");
            DropIndex("dbo.Parents", new[] { "DayCare_Id1" });
            DropIndex("dbo.Parents", new[] { "DayCare_Id" });
            DropColumn("dbo.Parents", "DayCare_Id1");
            DropColumn("dbo.Parents", "DayCare_Id");
            AddForeignKey("dbo.Parents", "DayCareId_Id", "dbo.DayCares", "Id");
        }
    }
}
