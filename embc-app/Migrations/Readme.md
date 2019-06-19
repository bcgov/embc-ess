# Database migrations

## Model Changes

Open embc-app solution file in Microsoft Blend for Visual Studio.
Make your change to the model in the `Models/Db` folder
Open PMC (NuGet Package Manager Console)
Add a migration with the appropriate name for the model change. 
    `Add-Migration FooMigration`
This generates a file to be applied to the database in the Migrations folder under `20190101125108_FooMigration.cs`.
Test the migration on your local database to be sure that it applies correctly. Reviewing the `.cs` file is a good idea.

Test the migration on the PMC against your local database with the command `Update-Database`. This works because LocalDB is the default database set in `DatabaseTools.cs`.

# Setting up a local_db to test migrations (Windows)

In Visual Studio we need to open the Visual Studio installer to get the right packages to install. "Get Tools and Features" from the "Tools" menu. Choose the `SQL Server Express 2016 LocalDB` package and install it. Be sure to have dotnet core 2.1 installed as well.

When the project runs it should connect to SQL Server Express and seed things like the country information from the seeder files. Run the project with the `F5` key.

You can check the data in the database with Azure Data Studio or a similar GUI based Microsoft SQL client. 

## First login

|Property|Value|
|--|--|
|Connection type| Microsoft Sql Server |
|Server| (localdb)\mssqllocaldb |
|Authentication type| Windows Authentication |

Localdb is a place on your HDD. It runs as a file based sql server similar to sqlite. It is an ad hoc process so it doesn't get its own port number.

You should be able to see the countries in one of the table that was generated.

**Set the SA user**
If you are connecting to the real database the SA user is needed in OpenShift.

You need the SA user when using the sql server container and not localdb. Win auth is not a valid option in OpenShift. SA is the Admin and is required to create the DB and schema in SQL server. A reg user only has read/write permissions and no DDL.
