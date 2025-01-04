To create the publish the app to azure

1. Open your user secrets and make sure you have the connection string to your groups Azure SQL Server.
   E.g, to SQL Server for the group Tanzanite is should look like
   "SQLServer-zooefc-azure-sysadmin": "Server=tcp:SYS6-Tanzanite-sqlserver-6755B002D1BB.database.windows.net,1433;Initial Catalog=SYS6-Tanzanite-db-6755B002D1BB;Persist Security Info=False;User ID=martin;Password=tegdek-1nyjZe-worzij;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;",
   "SQLServer-zooefc-azure-gstusr": "Server=tcp:SYS6-Tanzanite-sqlserver-6755B002D1BB.database.windows.net,1433;Initial Catalog=SYS6-Tanzanite-db-6755B002D1BB;Persist Security Info=False;User ID=gstusrUser;Password=pa$$Word1;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;",
   "SQLServer-zooefc-azure-usr": "Server=tcp:SYS6-Tanzanite-sqlserver-6755B002D1BB.database.windows.net,1433;Initial Catalog=SYS6-Tanzanite-db-6755B002D1BB;Persist Security Info=False;User User ID=usrUser;Password=pa$$Word1;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;",
   "SQLServer-zooefc-azure-supusr": "Server=tcp:SYS6-Tanzanite-sqlserver-6755B002D1BB.database.windows.net,1433;Initial Catalog=SYS6-Tanzanite-db-6755B002D1BB;Persist Security Info=False;User ID=supusrUser;Password=pa$$Word1;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;"

2. In Configuration.csproj make sure you have the AzureProjectSettings tag set to 
    <AzureProjectSettings>/Users/Martin/Development/scripts/azure/az-projects/newton-tanzanite</AzureProjectSettings>

3. Update your groups Azure KeyVault with the content of your user secrets
   With Terminal in folder .scripts 
   ./az-kv-update.sh

4. Make sure that below two keys in appsettings.json in the folders AppWebApi and DbContext are set to following:
      "UseAzureKeyVault": true
      "UseDataSetWithTag": "zooefc.localhost.docker"
   This ensures you will use user secrets and docker on your local development computer.

5. You can now run the AppWebApi using you local docker SQL Server
   Run AppWebApi with or without debugger
   Without debugger: Open a Terminal in folder AppGoodFriendsWebApi run: 
   dotnet run -lp http

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
   
6. Change UseDataSetWithTag to in appsettings.json in the folders AppWebApi and DbContext are set to following:
      "UseDataSetWithTag": "zooefc.azure"
   
7. With Terminal in folder .scripts 
   ./database-rebuild-all.sh azure
   Ensure no errors from build, migration or database update

8. From Azure Data Studio you can now connect to the database
   Use connection string from user secrets: sysadmin
   connection string corresponding to Tag
   "zooefc.azure"

4. Use Azure Data Studio to execute SQL script DbContext/SqlScripts/initDatabaseAzure.sql on the database zooefc

5. Run AppGoodFriendsWebApi with or without debugger
   Without debugger: Open a Terminal in folder AppGoodFriendsWebApi run: 
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
   
6. Use endpoint Admin/Seed to seed the database, Admin/RemoveSeed to remove the seed
   Verify connections and setup with endpoint Admin/Info
   Verify database seed with endpoint Guest/Info


