namespace KidCloudProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdjunctiontableforparentsandchildren : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Children", "ParentId_Id", "dbo.Parents");
            DropIndex("dbo.Children", new[] { "ParentId_Id" });
            RenameColumn(table: "dbo.Employees", name: "DayCare_Id", newName: "DayCareId_Id");
            RenameIndex(table: "dbo.Employees", name: "IX_DayCare_Id", newName: "IX_DayCareId_Id");
            CreateTable(
                "dbo.ParentChilds",
                c => new
                    {
                        Parent_Id = c.Int(nullable: false),
                        Child_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Parent_Id, t.Child_Id })
                .ForeignKey("dbo.Parents", t => t.Parent_Id, cascadeDelete: true)
                .ForeignKey("dbo.Children", t => t.Child_Id, cascadeDelete: true)
                .Index(t => t.Parent_Id)
                .Index(t => t.Child_Id);
            
            AddColumn("dbo.Children", "HasSpecialNeeds", c => c.Boolean(nullable: false));
            DropColumn("dbo.Children", "IsSpecialNeeds");
            DropColumn("dbo.Children", "ParentId_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Children", "ParentId_Id", c => c.Int());
            AddColumn("dbo.Children", "IsSpecialNeeds", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.ParentChilds", "Child_Id", "dbo.Children");
            DropForeignKey("dbo.ParentChilds", "Parent_Id", "dbo.Parents");
            DropIndex("dbo.ParentChilds", new[] { "Child_Id" });
            DropIndex("dbo.ParentChilds", new[] { "Parent_Id" });
            DropColumn("dbo.Children", "HasSpecialNeeds");
            DropTable("dbo.ParentChilds");
            RenameIndex(table: "dbo.Employees", name: "IX_DayCareId_Id", newName: "IX_DayCare_Id");
            RenameColumn(table: "dbo.Employees", name: "DayCareId_Id", newName: "DayCare_Id");
            CreateIndex("dbo.Children", "ParentId_Id");
            AddForeignKey("dbo.Children", "ParentId_Id", "dbo.Parents", "Id");
        }
    }
}
