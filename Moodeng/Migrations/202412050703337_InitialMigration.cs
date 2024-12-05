namespace Moodeng.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        ProductName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Picture = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CartId)
                .ForeignKey("dbo.AspNetUsers1", t => t.AspNetUser_Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers1",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles1",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers1", t => t.AspNetUser_Id)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins1",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(),
                        UserId = c.String(),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.LoginProvider)
                .ForeignKey("dbo.AspNetUsers1", t => t.AspNetUser_Id)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        MembershipId = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(),
                        RetailStoreId = c.Int(nullable: false),
                        MembershipDate = c.DateTime(),
                        MembershipStatus = c.String(),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MembershipId)
                .ForeignKey("dbo.AspNetUsers1", t => t.AspNetUser_Id)
                .ForeignKey("dbo.RetailStores", t => t.RetailStoreId, cascadeDelete: true)
                .Index(t => t.RetailStoreId)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.RetailStores",
                c => new
                    {
                        RetailStoreId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        StoreName = c.String(),
                        StoreAddress = c.String(),
                        StorePhone = c.String(),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RetailStoreId)
                .ForeignKey("dbo.AspNetUsers1", t => t.AspNetUser_Id)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        InventoryId = c.Int(nullable: false, identity: true),
                        RetailStoreId = c.Int(nullable: false),
                        ProductName = c.String(),
                        Category = c.String(),
                        Supplier = c.String(),
                        StockQuantity = c.Int(nullable: false),
                        LowStockThreshold = c.Int(),
                        LastUpdated = c.DateTime(),
                    })
                .PrimaryKey(t => t.InventoryId)
                .ForeignKey("dbo.RetailStores", t => t.RetailStoreId, cascadeDelete: true)
                .Index(t => t.RetailStoreId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        RetailStoreId = c.Int(nullable: false),
                        NotificationMessage = c.String(),
                        NotificationType = c.String(),
                        IsRead = c.Boolean(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.RetailStores", t => t.RetailStoreId, cascadeDelete: true)
                .Index(t => t.RetailStoreId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        CustomerId = c.String(),
                        RetailStoreId = c.Int(nullable: false),
                        OrderDate = c.DateTime(),
                        OrderStatus = c.String(),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.AspNetUsers1", t => t.AspNetUser_Id)
                .ForeignKey("dbo.RetailStores", t => t.RetailStoreId, cascadeDelete: true)
                .Index(t => t.RetailStoreId)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductName = c.String(),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderDetailId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.SalesReports",
                c => new
                    {
                        ReportId = c.Int(nullable: false, identity: true),
                        RetailStoreId = c.Int(nullable: false),
                        ReportDate = c.DateTime(),
                        TotalSales = c.Decimal(precision: 18, scale: 2),
                        TotalOrders = c.Int(),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReportId)
                .ForeignKey("dbo.RetailStores", t => t.RetailStoreId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers1", t => t.AspNetUser_Id)
                .Index(t => t.RetailStoreId)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.Wishlists",
                c => new
                    {
                        WishlistId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ProductId = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.WishlistId)
                .ForeignKey("dbo.AspNetUsers1", t => t.AspNetUser_Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.AspNetUser_Id);
            
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
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                "dbo.AspNetRoleAspNetUsers",
                c => new
                    {
                        AspNetRole_Id = c.String(nullable: false, maxLength: 128),
                        AspNetUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AspNetRole_Id, t.AspNetUser_Id })
                .ForeignKey("dbo.AspNetRoles1", t => t.AspNetRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers1", t => t.AspNetUser_Id, cascadeDelete: true)
                .Index(t => t.AspNetRole_Id)
                .Index(t => t.AspNetUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Carts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Wishlists", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Wishlists", "AspNetUser_Id", "dbo.AspNetUsers1");
            DropForeignKey("dbo.SalesReports", "AspNetUser_Id", "dbo.AspNetUsers1");
            DropForeignKey("dbo.SalesReports", "RetailStoreId", "dbo.RetailStores");
            DropForeignKey("dbo.Orders", "RetailStoreId", "dbo.RetailStores");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "AspNetUser_Id", "dbo.AspNetUsers1");
            DropForeignKey("dbo.Notifications", "RetailStoreId", "dbo.RetailStores");
            DropForeignKey("dbo.Memberships", "RetailStoreId", "dbo.RetailStores");
            DropForeignKey("dbo.Inventories", "RetailStoreId", "dbo.RetailStores");
            DropForeignKey("dbo.RetailStores", "AspNetUser_Id", "dbo.AspNetUsers1");
            DropForeignKey("dbo.Memberships", "AspNetUser_Id", "dbo.AspNetUsers1");
            DropForeignKey("dbo.Carts", "AspNetUser_Id", "dbo.AspNetUsers1");
            DropForeignKey("dbo.AspNetUserLogins1", "AspNetUser_Id", "dbo.AspNetUsers1");
            DropForeignKey("dbo.AspNetUserClaims1", "AspNetUser_Id", "dbo.AspNetUsers1");
            DropForeignKey("dbo.AspNetRoleAspNetUsers", "AspNetUser_Id", "dbo.AspNetUsers1");
            DropForeignKey("dbo.AspNetRoleAspNetUsers", "AspNetRole_Id", "dbo.AspNetRoles1");
            DropIndex("dbo.AspNetRoleAspNetUsers", new[] { "AspNetUser_Id" });
            DropIndex("dbo.AspNetRoleAspNetUsers", new[] { "AspNetRole_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Wishlists", new[] { "AspNetUser_Id" });
            DropIndex("dbo.Wishlists", new[] { "ProductId" });
            DropIndex("dbo.SalesReports", new[] { "AspNetUser_Id" });
            DropIndex("dbo.SalesReports", new[] { "RetailStoreId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "AspNetUser_Id" });
            DropIndex("dbo.Orders", new[] { "RetailStoreId" });
            DropIndex("dbo.Notifications", new[] { "RetailStoreId" });
            DropIndex("dbo.Inventories", new[] { "RetailStoreId" });
            DropIndex("dbo.RetailStores", new[] { "AspNetUser_Id" });
            DropIndex("dbo.Memberships", new[] { "AspNetUser_Id" });
            DropIndex("dbo.Memberships", new[] { "RetailStoreId" });
            DropIndex("dbo.AspNetUserLogins1", new[] { "AspNetUser_Id" });
            DropIndex("dbo.AspNetUserClaims1", new[] { "AspNetUser_Id" });
            DropIndex("dbo.Carts", new[] { "AspNetUser_Id" });
            DropIndex("dbo.Carts", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropTable("dbo.AspNetRoleAspNetUsers");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Wishlists");
            DropTable("dbo.SalesReports");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
            DropTable("dbo.Notifications");
            DropTable("dbo.Inventories");
            DropTable("dbo.RetailStores");
            DropTable("dbo.Memberships");
            DropTable("dbo.AspNetUserLogins1");
            DropTable("dbo.AspNetUserClaims1");
            DropTable("dbo.AspNetRoles1");
            DropTable("dbo.AspNetUsers1");
            DropTable("dbo.Carts");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
