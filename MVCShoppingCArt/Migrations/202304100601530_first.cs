namespace MVCShoppingCArt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        PId = c.Guid(nullable: false),
                        PName = c.String(),
                        PDescription = c.String(),
                        PPrice = c.String(),
                        PImage = c.String(),
                    })
                .PrimaryKey(t => t.PId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
        }
    }
}
