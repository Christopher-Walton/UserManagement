CREATE TABLE IndividualConsignee
(
	Id int PRIMARY KEY IDENTITY(1,1),
	FirstName varchar(50) NOT NULL,
	LastName varchar(50) NOT NULL,
	TRN varchar(12) UNIQUE NOT NULL,
	[Address] varchar(125) NOT NULL,
	CustomerCode varchar(5)
)

CREATE TABLE CompanyConsignee
(
	Id int PRIMARY KEY IDENTITY(1,1),
	CompanyName varchar(100) NOT NULL,
	CompanyTRN varchar(12) UNIQUE NOT NULL,
	CompanyAddress varchar(125) NOT NULL,
	CompanyPhoneNumber varchar(12) NOT NULL,
	CompanyEmail varchar(50) NOT NULL,
	CustomerCode varchar(5)
)

CREATE TABLE CompanyRepresentative
(
	CompanyConsigneeId int NOT NULL FOREIGN KEY REFERENCES CompanyConsignee(Id),
	UserId nvarchar(128) NOT NULL FOREIGN KEY REFERENCES AspNetUsers(Id)
)

-- Add Composite Primary Key To
ALTER TABLE CompanyRepresentative
	ADD CONSTRAINT pk_CompanyRepresentative PRIMARY KEY (CompanyConsigneeId,UserId)
GO

CREATE TABLE IndividualRepresentative
(
	IndividualConsigneeId int NOT NULL FOREIGN KEY REFERENCES IndividualConsignee(Id),
	UserId nvarchar(128) NOT NULL FOREIGN KEY REFERENCES AspNetUsers(Id)
)

-- Add Composite Primary Key To
ALTER TABLE IndividualRepresentative
	ADD CONSTRAINT pk_IndividualRepresentative PRIMARY KEY (IndividualConsigneeId,UserId)
GO


CREATE TABLE [dbo].[AuthenticationInformation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Token] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[UserIdentityDetails] [varchar](max) NOT NULL
)