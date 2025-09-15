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