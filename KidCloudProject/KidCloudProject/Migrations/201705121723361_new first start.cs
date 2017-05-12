namespace KidCloudProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newfirststart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Status = c.String(nullable: false),
                        Parent_Id = c.Int(),
                        DayCare_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parents", t => t.Parent_Id)
                .ForeignKey("dbo.DayCares", t => t.DayCare_Id)
                .Index(t => t.Parent_Id)
                .Index(t => t.DayCare_Id);
            
            CreateTable(
                "dbo.DayCares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateEstablished = c.DateTime(),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(nullable: false, maxLength: 5),
                        Phone = c.String(nullable: false, maxLength: 10),
                        Email = c.String(nullable: false),
                        AcceptChildrenUnderAgeTwo = c.Boolean(nullable: false),
                        MaxChildren = c.Int(nullable: false),
                        CurrentlyAcceptingApplicants = c.Boolean(nullable: false),
                        ChannelId = c.String(),
                        DailyRatePerChild = c.Double(nullable: false),
                        UserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .Index(t => t.UserId_Id);
            
            CreateTable(
                "dbo.Children",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Age = c.String(nullable: false),
                        HalfDay = c.Boolean(nullable: false),
                        FullDay = c.Boolean(nullable: false),
                        HasAllergies = c.Boolean(nullable: false),
                        AllergiesDetails = c.String(),
                        TakesMedications = c.Boolean(nullable: false),
                        MedicationInstructions = c.String(),
                        HasSpecialFoodRequirements = c.Boolean(nullable: false),
                        FoodRequirementsInstructions = c.String(),
                        HasSpecialNeeds = c.Boolean(nullable: false),
                        SpecialNeedsType = c.String(),
                        SpecialNeedsRequirements = c.String(),
                        DayCare_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DayCares", t => t.DayCare_Id)
                .Index(t => t.DayCare_Id);
            
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
                        MoneyOwed = c.Decimal(precision: 18, scale: 2),
                        DayCare_Id = c.Int(),
                        UserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DayCares", t => t.DayCare_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .Index(t => t.DayCare_Id)
                .Index(t => t.UserId_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                        DayCareId_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Children", t => t.ChildId_Id)
                .ForeignKey("dbo.DayCares", t => t.DayCareId_Id)
                .Index(t => t.ChildId_Id)
                .Index(t => t.DayCareId_Id);
            
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
                        DayCareId_Id = c.Int(),
                        UserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DayCares", t => t.DayCareId_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .Index(t => t.DayCareId_Id)
                .Index(t => t.UserId_Id);
            
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
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.DirectMessageChannels", "SenderId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DirectMessageChannels", "ReciverId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DayCares", "UserId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Applications", "DayCare_Id", "dbo.DayCares");
            DropForeignKey("dbo.Employees", "UserId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Employees", "DayCareId_Id", "dbo.DayCares");
            DropForeignKey("dbo.DailyReports", "DayCareId_Id", "dbo.DayCares");
            DropForeignKey("dbo.DailyReports", "ChildId_Id", "dbo.Children");
            DropForeignKey("dbo.Children", "DayCare_Id", "dbo.DayCares");
            DropForeignKey("dbo.Parents", "UserId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Parents", "DayCare_Id", "dbo.DayCares");
            DropForeignKey("dbo.ParentChilds", "Child_Id", "dbo.Children");
            DropForeignKey("dbo.ParentChilds", "Parent_Id", "dbo.Parents");
            DropForeignKey("dbo.Applications", "Parent_Id", "dbo.Parents");
            DropIndex("dbo.ParentChilds", new[] { "Child_Id" });
            DropIndex("dbo.ParentChilds", new[] { "Parent_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.DirectMessageChannels", new[] { "SenderId_Id" });
            DropIndex("dbo.DirectMessageChannels", new[] { "ReciverId_Id" });
            DropIndex("dbo.Employees", new[] { "UserId_Id" });
            DropIndex("dbo.Employees", new[] { "DayCareId_Id" });
            DropIndex("dbo.DailyReports", new[] { "DayCareId_Id" });
            DropIndex("dbo.DailyReports", new[] { "ChildId_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Parents", new[] { "UserId_Id" });
            DropIndex("dbo.Parents", new[] { "DayCare_Id" });
            DropIndex("dbo.Children", new[] { "DayCare_Id" });
            DropIndex("dbo.DayCares", new[] { "UserId_Id" });
            DropIndex("dbo.Applications", new[] { "DayCare_Id" });
            DropIndex("dbo.Applications", new[] { "Parent_Id" });
            DropTable("dbo.ParentChilds");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.DirectMessageChannels");
            DropTable("dbo.Employees");
            DropTable("dbo.DailyReports");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Parents");
            DropTable("dbo.Children");
            DropTable("dbo.DayCares");
            DropTable("dbo.Applications");
        }
    }
}
