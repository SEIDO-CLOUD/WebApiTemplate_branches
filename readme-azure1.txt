This text shows the steps to run AppWebApi locally but using Azure KeyVault and Azure SQL.
First Azure KeyVault is engaged and the Azure SQL Server
This is needed before publishing the app to Azure.


Ensure ConnectionStrings and Project seetings
---------------------------------------------------
1. Open your user secrets and make sure you have the connection string to your groups Azure SQL Server.
   E.g, to SQL Server for the group Tanzanite is should look like
        "SQLServer-zooefc-azure-sysadmin": "Server=tcp:SYS6-Garnet-sqlserver-6755B002D1BB.database.windows.net,1433;Initial Catalog=SYS6-Garnet-db-6755B002D1BB;Persist Security Info=False;User ID=martin;Password=tegdek-1nyjZe-worzij;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;",
        "SQLServer-zooefc-azure-gstusr": "Server=tcp:SYS6-Garnet-sqlserver-6755B002D1BB.database.windows.net,1433;Initial Catalog=SYS6-Garnet-db-6755B002D1BB;Persist Security Info=False;User ID=gstusrUser;Password=pa$$Word1;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;",
        "SQLServer-zooefc-azure-usr": "Server=tcp:SYS6-Garnet-sqlserver-6755B002D1BB.database.windows.net,1433;Initial Catalog=SYS6-Garnet-db-6755B002D1BB;Persist Security Info=False;User ID=usrUser;Password=pa$$Word1;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;",
        "SQLServer-zooefc-azure-supusr": "Server=tcp:SYS6-Garnet-sqlserver-6755B002D1BB.database.windows.net,1433;Initial Catalog=SYS6-Garnet-db-6755B002D1BB;Persist Security Info=False;User ID=supusrUser;Password=pa$$Word1;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;",

2. In Configuration.csproj make sure you have the AzureProjectSettings tag set to 
    <AzureProjectSettings>/Users/Martin/Development/scripts/azure/az-projects/newton-garnet</AzureProjectSettings>


Update Azure KeyVault
---------------------
3. Update your groups Azure KeyVault with the content of your user secrets
   With Terminal in folder .scripts run
   ./az-kv-update.sh


Run AppWebApi locally using local SQL Server and Azure KeyVault
-----------------------------------------------------------------
4. Make sure that below two keys in appsettings.json in the folders AppWebApi and DbContext are set to following:
      "UseAzureKeyVault": true
      "DefaultDataUser": "sysadmin"
      "UseDataSetWithTag": "zooefc.localhost.docker"
   This ensures you will use user secrets and docker on your local development computer.

5. You can now run the AppWebApi using you local docker SQL Server
   Run AppWebApi with or without debugger
   Without debugger: Open a Terminal in folder AppWebApi run: 
   dotnet run -lp https

   Verify connections and setup with endpoint Admin/Info. Output should be
   {
      "appEnvironment": "Development",
      "secretSource": "Azure: Tanzanite",
      "dataConnectionTag": "zooefc.localhost.docker",
      "defaultDataUser": "sysadmin",
      "migrationDataUser": "sysadmin",
      "dataConnectionServer": 0,
      "dataConnectionServerString": "SQLServer"
   }

   Verify database seed with endpoint Guest/Info. You will see the overview of the local database content
   

Build Azure Database
--------------------
6. From Azure Data Studio you can now connect to the database
   Use connection string from user secrets: sysadmin
   connection string corresponding to Tag
   "zooefc.azure"
   NOTE: You may have to open the firewalls on the SQL Server on Azure
   
7. Use Azure Data Studio to execute SQL script DbContext/SqlScripts/clearDatabaseAzure.sql on the database zooefc
   Ignore any errors

8. Change UseDataSetWithTag to in appsettings.json in the folders AppWebApi and DbContext are set to following:
      "UseDataSetWithTag": "zooefc.azure"
   
9. With Terminal in folder .scripts run
   ./database-rebuild-all.sh azure
   Ensure no errors from build, migration or database update

10. Use Azure Data Studio to execute SQL script DbContext/SqlScripts/initDatabaseAzure.sql on the database zooefc



Run AppWebApi locally using Azure SQL Server and Azure KeyVault
-----------------------------------------------------------------
11. Run AppWebApi with or without debugger
   Without debugger: Open a Terminal in folder AppWebApi run: 
   dotnet run -lp https 

   open url: https://localhost:7066/swagger

   Verify connections and setup with endpoint Admin/Info. Output should be
   {
      "appEnvironment": "Development",
      "secretSource": "Azure: Tanzanite",
      "dataConnectionTag": "zooefc.azure",
      "defaultDataUser": "sysadmin",
      "migrationDataUser": "sysadmin",
      "dataConnectionServer": 0,
      "dataConnectionServerString": "SQLServer"
   }

   Verify database seed with endpoint Guest/Info. You will see the overview of the local database content

12. Use endpoint Admin/SeedUsers to seed users into the the database

13. Terminate WebApi
   Change one key in appsettings.json in the folder AppWebApi to following:
      "DefaultDataUser": "gstusr"
   This ensures your database connection, unless logged in, will be gstusr
   
14. Run AppWebApi with or without debugger
    Use endpoint Guest/LoginUser to login as sysadmin1
{
  "userNameOrEmail": "sysadmin1",
  "password": "sysadmin1"
}

16. Authorize using Swagger Authorize button and paste in the encryptedToken recieved after login.
    NOTE!!: Copy and paste the encryptedToken WITHIN the quotation, i.e. WITHOUT the first and last quotation mark "

16. Use endpoint Admin/Seed to seed the database, Admin/RemoveSeed to remove the seed
   Verify database seed with endpoint Guest/Info

17. As sysadmin you can now use and play with all endpoints
