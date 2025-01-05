--USE zooefc;
--GO

--01-create-schema.sql
--create a schema for guest users, i.e. not logged in
CREATE SCHEMA gstusr;
GO

--create a schema for logged in user
CREATE SCHEMA usr;
GO

--02-create-gstusr-view.sql
--create a view that gives overview of the database content
CREATE OR ALTER VIEW gstusr.vwInfoDb AS
    SELECT 'Guest user database overview' as Title
GO


--03-create-supusr-sp.sql
CREATE OR ALTER PROC supusr.spDeleteAll
    @Seeded BIT = 1

    AS

    SET NOCOUNT ON;

    -- will delete here

    -- return new data status
    SELECT * FROM gstusr.vwInfoDb;

    --throw our own error
    --;THROW 999999, 'my own supusr.spDeleteAll Error directly from SQL Server', 1

    --show return code usage
    RETURN 0;  --indicating success
    --RETURN 1;  --indicating your own error code, in this case 1
GO

--04-create-users-azure.sql
--create 3 users we will late set credentials for these
DROP USER IF EXISTS  gstusrUser;
DROP USER IF EXISTS usrUser;
DROP USER IF EXISTS supusrUser;

CREATE USER gstusrUser WITH PASSWORD = N'pa$$Word1'; 
CREATE USER usrUser WITH PASSWORD = N'pa$$Word1'; 
CREATE USER supusrUser WITH PASSWORD = N'pa$$Word1'; 

ALTER ROLE db_datareader ADD MEMBER gstusrUser; 
ALTER ROLE db_datareader ADD MEMBER usrUser; 
ALTER ROLE db_datareader ADD MEMBER supusrUser; 
GO

--05-create-roles-credentials.sql
--create roles
CREATE ROLE zoosefcGstUsr;
CREATE ROLE zoosefcUsr;
CREATE ROLE zoosefcSupUsr;

--assign securables creadentials to the roles
GRANT SELECT, EXECUTE ON SCHEMA::gstusr to zoosefcGstUsr;
GRANT SELECT ON SCHEMA::supusr to zoosefcUsr;
GRANT SELECT, UPDATE, INSERT, DELETE, EXECUTE ON SCHEMA::supusr to zoosefcSupUsr;

--finally, add the users to the roles
ALTER ROLE zoosefcGstUsr ADD MEMBER gstusrUser;

ALTER ROLE zoosefcGstUsr ADD MEMBER usrUser;
ALTER ROLE zoosefcUsr ADD MEMBER usrUser;

ALTER ROLE zoosefcGstUsr ADD MEMBER supusrUser;
ALTER ROLE zoosefcUsr ADD MEMBER supusrUser;
ALTER ROLE zoosefcSupUsr ADD MEMBER supusrUser;
GO


