To create the AppGoodFriendsWebApi

1. Make sure that belwo two keys in appsettings.json in the folders AppWebApi and DbContext are set to following:
      "UseAzureKeyVault": false
      "UseDataSetWithTag": "zooefc.localhost.docker"
   This ensures you will use user secrets and docker on your local development computer.

2. With Terminal in folder .scripts 
   ./database-rebuild-all.sh local
   Ensure no errors from build, migration or database update

3. From Azure Data Studio you can now connect to the database
   Use connection string from user secrets:
   connection string corresponding to Tag
   "zooefc.localhost.docker"

4. Use Azure Data Studio to execute SQL script DbContext/SqlScripts/initDatabase.sql on the database zooefc

5. Run AppGoodFriendsWebApi with or without debugger
   Without debugger: Open a Terminal in folder AppGoodFriendsWebApi run: 
   dotnet run -lp https 

   open url: https://localhost:7066/swagger
   
6. Use endpoint Admin/Seed to seed the database, Admin/RemoveSeed to remove the seed
   Verify connections and setup with endpoint Admin/Info
   Verify database seed with endpoint Guest/Info


