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

[Migration(4)]
public class AddFavoriteCurrencies : Migration
{
    public override void Up()
    {
        Create.Table("FavoriteCurrencies")
            .WithColumn("Name").AsFixedLengthString(128).NotNullable()
            .WithColumn("Code").AsFixedLengthString(64).NotNullable();

        Create.PrimaryKey()
            .OnTable("FavoriteCurrencies")
            .Columns("Name", "Code");
    }

    public override void Down()
    {
        Delete.Table("FavoriteCurrencies");
    }
}

[Migration(5)]
public class AddIndexesFavoriteCurrencies : Migration
{
    public override void Up()
    {
        Create.Index("IX_FavoriteCurrencies_Name_Code")
            .OnTable("FavoriteCurrencies")
            .OnColumn("Name")
            .Ascending()
            .OnColumn("Code")
            .Ascending()
            .WithOptions().Unique();
    }

    public override void Down()
    {
        Delete.Index("IX_FavoriteCurrencies_Name_Code")
            .OnTable("FavoriteCurrencies");
    }
}

[Migration(6)]
public class AddInsertUserProcedure : Migration
{
    public override void Up()
    {
        Execute.Sql("CREATE OR REPLACE FUNCTION insert_user_safe(p_name TEXT, p_age INT)" +
                    "RETURNS INT AS $$BEGIN   " +
                    " BEGIN      " +
                    "  INSERT INTO users(name, age) VALUES (p_name, p_age);\n        RETURN 1; -- Успешно\n    EXCEPTION WHEN OTHERS THEN\n        RETURN 0; -- Ошибка\n    END;\nEND;\n$$ LANGUAGE plpgsql;");
    }

    public override void Down()
    {
        Delete.Index("IX_FavoriteCurrencies_Name_Code")
            .OnTable("FavoriteCurrencies");
    }
}