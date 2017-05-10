namespace KidCloudProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeddaycaretodailyReportandemployee : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.DailyReports", name: "DayCare_Id", newName: "DayCareId_Id");
            RenameIndex(table: "dbo.DailyReports", name: "IX_DayCare_Id", newName: "IX_DayCareId_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.DailyReports", name: "IX_DayCareId_Id", newName: "IX_DayCare_Id");
            RenameColumn(table: "dbo.DailyReports", name: "DayCareId_Id", newName: "DayCare_Id");
        }
    }
}
