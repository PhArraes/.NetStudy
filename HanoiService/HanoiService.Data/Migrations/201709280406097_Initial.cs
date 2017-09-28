namespace HanoiService.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HanoiLogs",
                c => new
                    {
                        LogID = c.Int(nullable: false, identity: true),
                        DiscsNumber = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.LogID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HanoiLogs");
        }
    }
}
