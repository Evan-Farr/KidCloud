namespace KidCloudProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedDayCareIdtoEventModelandrescaffoldedControllerandView : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "DayCareId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "DayCareId");
        }
    }
}
