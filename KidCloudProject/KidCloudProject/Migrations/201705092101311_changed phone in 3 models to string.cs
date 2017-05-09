namespace KidCloudProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedphonein3modelstostring : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Children",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Age = c.String(nullable: false),
                        HasAllergies = c.Boolean(nullable: false),
                        AllergiesDetails = c.String(),
                        TakesMedications = c.Boolean(nullable: false),
                        MedicationInstructions = c.String(),
                        HasSpecialFoodRequirements = c.Boolean(nullable: false),
                        FoodRequirementsInstructions = c.String(),
                        IsSpecialNeeds = c.Boolean(nullable: false),
                        SpecialNeedsType = c.String(),
                        SpecialNeedsRequirements = c.String(),
                        DayCare_Id = c.Int(),
                        ParentId_Id = c.Int(),
                        UserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DayCares", t => t.DayCare_Id)
                .ForeignKey("dbo.Parents", t => t.ParentId_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .Index(t => t.DayCare_Id)
                .Index(t => t.ParentId_Id)
                .Index(t => t.UserId_Id);
            
            CreateTable(
                "dbo.Parents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Age = c.Int(),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(nullable: false, maxLength: 5),
                        Phone = c.String(nullable: false, maxLength: 10),
                        Email = c.String(nullable: false),
                        NumberOfChildren = c.Int(nullable: false),
                        MoneyOwed = c.Decimal(precision: 18, scale: 2),
                        DayCareId_Id = c.Int(),
                        UserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DayCares", t => t.DayCareId_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .Index(t => t.DayCareId_Id)
                .Index(t => t.UserId_Id);
            
            CreateTable(
                "dbo.DayCares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OpenDate = c.DateTime(),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(nullable: false, maxLength: 5),
                        Phone = c.String(nullable: false, maxLength: 10),
                        Email = c.String(nullable: false),
                        AcceptChildrenUnderAgeTwo = c.Boolean(nullable: false),
                        AcceptDisabilites = c.Boolean(nullable: false),
                        MaxChildren = c.Int(nullable: false),
                        CurrentlyAcceptingApplicants = c.Boolean(nullable: false),
                        UserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .Index(t => t.UserId_Id);
            
            CreateTable(
                "dbo.DailyReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportDate = c.DateTime(nullable: false),
                        BathroomUse = c.String(nullable: false),
                        Meals = c.String(),
                        Sleep = c.String(),
                        ActivityReport = c.String(),
                        SuppliesNeeds = c.String(),
                        Mood = c.String(),
                        MiscellaneousNotes = c.String(),
                        ChildId_Id = c.Int(),
                        DayCare_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Children", t => t.ChildId_Id)
                .ForeignKey("dbo.DayCares", t => t.DayCare_Id)
                .Index(t => t.ChildId_Id)
                .Index(t => t.DayCare_Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Age = c.Int(),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(nullable: false, maxLength: 5),
                        Phone = c.String(nullable: false, maxLength: 10),
                        Email = c.String(nullable: false),
                        StartDate = c.DateTime(),
                        UserId_Id = c.String(maxLength: 128),
                        DayCare_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .ForeignKey("dbo.DayCares", t => t.DayCare_Id)
                .Index(t => t.UserId_Id)
                .Index(t => t.DayCare_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Children", "UserId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Children", "ParentId_Id", "dbo.Parents");
            DropForeignKey("dbo.Parents", "UserId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DayCares", "UserId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Parents", "DayCareId_Id", "dbo.DayCares");
            DropForeignKey("dbo.Employees", "DayCare_Id", "dbo.DayCares");
            DropForeignKey("dbo.Employees", "UserId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DailyReports", "DayCare_Id", "dbo.DayCares");
            DropForeignKey("dbo.DailyReports", "ChildId_Id", "dbo.Children");
            DropForeignKey("dbo.Children", "DayCare_Id", "dbo.DayCares");
            DropIndex("dbo.Employees", new[] { "DayCare_Id" });
            DropIndex("dbo.Employees", new[] { "UserId_Id" });
            DropIndex("dbo.DailyReports", new[] { "DayCare_Id" });
            DropIndex("dbo.DailyReports", new[] { "ChildId_Id" });
            DropIndex("dbo.DayCares", new[] { "UserId_Id" });
            DropIndex("dbo.Parents", new[] { "UserId_Id" });
            DropIndex("dbo.Parents", new[] { "DayCareId_Id" });
            DropIndex("dbo.Children", new[] { "UserId_Id" });
            DropIndex("dbo.Children", new[] { "ParentId_Id" });
            DropIndex("dbo.Children", new[] { "DayCare_Id" });
            DropTable("dbo.Employees");
            DropTable("dbo.DailyReports");
            DropTable("dbo.DayCares");
            DropTable("dbo.Parents");
            DropTable("dbo.Children");
        }
    }
}
