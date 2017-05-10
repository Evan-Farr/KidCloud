namespace KidCloudProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedChannelIdtoDayCaremodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DayCares", "ChannelId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DayCares", "ChannelId");
        }
    }
}
