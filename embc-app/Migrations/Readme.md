# Database migrations

## Model Changes

Open embc-app solution file in Microsoft Blend for Visual Studio.
Make your change to the model in the `Models/Db` folder
Open PMC (Package Manager Console)
Add a migration with the appropriate name for the model change. This generates a file in the 
    Add-Migration FooMigration
This generates a file to be applied to the database in the Migrations folder under `20190101125108_FooMigration.cs`.
Test the migration on your local database to be sure that it applies correctly. Reviewing the `.cs` file is a good idea.

# Setting up a local_db to test migrations

In Visual Studio you need to open the Visual Studio installer to get the right packages to install. "Get Tools and Features" from the "Tools" menu. Choose the `SQL Server Express 2016 LocalDB` package and install it. Be sure to have dotnet core 2.1 installed as well.

We need to add environment variables to the solution so that we can connect the project to the LocalDB. In solution explorer right click embc-app and access properties. Under the debug tab add these environment variables.
| Name | Value |
|--|--|
| DB_USER | ess |
| DB_DATABASE | ess-from-dev |
| DB_FULL_REFRESH | false |
| DB_PASSWORD | password |
| DB_ADMIN_PASSWORD | password |

