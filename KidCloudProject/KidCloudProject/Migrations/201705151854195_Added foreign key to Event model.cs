namespace KidCloudProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedforeignkeytoEventmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventType", c => c.Int());
            AddColumn("dbo.Events", "UserId_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Events", "UserId_Id");
            AddForeignKey("dbo.Events", "UserId_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "UserId_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "UserId_Id" });
            DropColumn("dbo.Events", "UserId_Id");
            DropColumn("dbo.Events", "EventType");
        }
    }
}
