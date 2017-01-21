namespace CardIndex.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Street = c.String(maxLength: 50),
                        ZipCode = c.String(maxLength: 10),
                        City = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contractor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Surname = c.String(maxLength: 30),
                        Email = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(maxLength: 15),
                        DateOfBirth = c.DateTime(),
                        AddressId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.AddressId, cascadeDelete: true)
                .Index(t => t.AddressId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contractor", "AddressId", "dbo.Address");
            DropIndex("dbo.Contractor", new[] { "AddressId" });
            DropTable("dbo.Contractor");
            DropTable("dbo.Address");
        }
    }
}
