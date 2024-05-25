namespace QLLaCoffee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class name : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShiftReports", "FirstTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.ShiftReports", "LastTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShiftReports", "LastTime");
            DropColumn("dbo.ShiftReports", "FirstTime");
        }
    }
}
