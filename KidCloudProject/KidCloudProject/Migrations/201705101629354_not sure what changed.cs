namespace KidCloudProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notsurewhatchanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DayCares", "DateEstablished", c => c.DateTime());
            DropColumn("dbo.Parents", "NumberOfChildren");
            DropColumn("dbo.DayCares", "OpenDate");
            DropColumn("dbo.DayCares", "AcceptDisabilites");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DayCares", "AcceptDisabilites", c => c.Boolean(nullable: false));
            AddColumn("dbo.DayCares", "OpenDate", c => c.DateTime());
            AddColumn("dbo.Parents", "NumberOfChildren", c => c.Int(nullable: false));
            DropColumn("dbo.DayCares", "DateEstablished");
        }
    }
}
