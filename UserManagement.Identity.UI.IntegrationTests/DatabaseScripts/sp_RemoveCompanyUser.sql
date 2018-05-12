CREATE PROCEDURE CleanUpCompanyAccountPostTest

@Email nvarchar(256)

AS

DECLARE @UserId nvarchar(128)
DECLARE @CompanyConsigneeId nvarchar(128)

BEGIN
	
	SELECT @UserId = Id
	FROM AspNetUsers
	WHERE Email = @Email

	DELETE
	FROM  AspNetUserRoles
	WHERE UserId = @UserId

	SELECT @CompanyConsigneeId = CompanyConsigneeId
	FROM CompanyRepresentative
	WHERE UserId = @UserId

	DELETE
	FROM  CompanyRepresentative
	WHERE CompanyConsigneeId = @CompanyConsigneeId

	DELETE
	FROM  CompanyConsignee
	WHERE Id = @CompanyConsigneeId

	DELETE
	FROM AspNetUsers
	WHERE Id = @UserId
END