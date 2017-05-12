namespace KidCloudProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDirectMessageChannel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DirectMessageChannels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChannelId = c.String(),
                        ReciverId_Id = c.String(maxLength: 128),
                        SenderId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ReciverId_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId_Id)
                .Index(t => t.ReciverId_Id)
                .Index(t => t.SenderId_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DirectMessageChannels", "SenderId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DirectMessageChannels", "ReciverId_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DirectMessageChannels", new[] { "SenderId_Id" });
            DropIndex("dbo.DirectMessageChannels", new[] { "ReciverId_Id" });
            DropTable("dbo.DirectMessageChannels");
        }
    }
}
