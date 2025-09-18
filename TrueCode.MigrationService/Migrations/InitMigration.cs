using FluentMigrator;

namespace TrueCode.MigrationService.Migrations;

[Migration(1)]
public class AddUserTableMigration : Migration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString(120).NotNullable().Unique()
            .WithColumn("Hash").AsString(256).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Users");
    }
}

[Migration(2)]
public class AddCurrencyTableMigration : Migration
{
    public override void Up()
    {
        Create.Table("Currencies")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(120).NotNullable().Indexed()
            .WithColumn("Rate").AsDecimal().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Currencies");
    }
}

[Migration(3)]
public class AddFillerServiceInfoDb : Migration
{
    public override void Up()
    {
        Create.Table("FillerServiceInfo")
            .WithColumn("Id").AsInt16().PrimaryKey().WithDefaultValue(1)
            .WithColumn("LastUpdated").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("FillerServiceInfo");
    }
}