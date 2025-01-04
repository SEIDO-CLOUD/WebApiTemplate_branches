USE zooefc;
GO

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


--04-create-users.sql
--Create 3 logins
IF SUSER_ID (N'gstusr') IS NOT NULL
DROP LOGIN gstusr;

IF SUSER_ID (N'usr') IS NOT NULL
DROP LOGIN usr;

IF SUSER_ID (N'supusr') IS NOT NULL
DROP LOGIN supusr;

CREATE LOGIN gstusr WITH PASSWORD=N'pa$$Word1', 
    DEFAULT_DATABASE=zooefc, DEFAULT_LANGUAGE=us_english, 
    CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;

CREATE LOGIN usr WITH PASSWORD=N'pa$$Word1', 
DEFAULT_DATABASE=zooefc, DEFAULT_LANGUAGE=us_english, 
CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;

CREATE LOGIN supusr WITH PASSWORD=N'pa$$Word1', 
DEFAULT_DATABASE=zooefc, DEFAULT_LANGUAGE=us_english, 
CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;


--create 3 users from the logins, we will late set credentials for these
DROP USER IF EXISTS  gstusrUser;
DROP USER IF EXISTS usrUser;
DROP USER IF EXISTS supusrUser;

CREATE USER gstusrUser FROM LOGIN gstusr;
CREATE USER usrUser FROM LOGIN usr;
CREATE USER supusrUser FROM LOGIN supusr;

--05-create-roles-credentials.sql
--create roles
CREATE ROLE zooefcGstUsr;
CREATE ROLE zooefcUsr;
CREATE ROLE zooefcSupUsr;

--assign securables creadentials to the roles
GRANT SELECT, EXECUTE ON SCHEMA::gstusr to zooefcGstUsr;
GRANT SELECT ON SCHEMA::supusr to zooefcUsr;
GRANT SELECT, UPDATE, INSERT, DELETE, EXECUTE ON SCHEMA::supusr to zooefcSupUsr;

--finally, add the users to the roles
ALTER ROLE zooefcGstUsr ADD MEMBER gstusrUser;

ALTER ROLE zooefcGstUsr ADD MEMBER usrUser;
ALTER ROLE zooefcUsr ADD MEMBER usrUser;

ALTER ROLE zooefcGstUsr ADD MEMBER supusrUser;
ALTER ROLE zooefcUsr ADD MEMBER supusrUser;
ALTER ROLE zooefcSupUsr ADD MEMBER supusrUser;
GO




