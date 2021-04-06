namespace ZumaneGas_OperationalSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LatestMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "Order_Date", c => c.DateTime(nullable: true));

    //        AddColumn("dbo.People", "LocationTmp", c => c.Int(nullable: false));
    //        Sql(@"
    //UPDATE dbp.People
    //SET LocationTmp =
    //    CASE Location
    //        WHEN 'London' THEN 1
    //        WHEN 'Edinburgh' THEN 2
    //        WHEN 'Cardiff' THEN 3
    //        ELSE 0
    //    END
    //");
    //        DropColumn("dbo.People", "Location");
    //        RenameColumn("dbo.People", "LocationTmp", "Location");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Order_Date", c => c.DateTime());
        }
    }
}
