CREATE PROCEDURE CleanUpIndividualAccountPostTest 

@Email nvarchar(256)

AS

DECLARE @UserId nvarchar(128)
DECLARE @IndividualConsigneeId nvarchar(128)

BEGIN
	
	SELECT @UserId = Id
	FROM AspNetUsers
	WHERE Email = @Email

	DELETE
	FROM  AspNetUserRoles
	WHERE UserId = @UserId

	SELECT @IndividualConsigneeId = IndividualConsigneeId
	FROM IndividualRepresentative
	WHERE UserId = @UserId

	DELETE
	FROM  IndividualRepresentative
	WHERE IndividualConsigneeId = @IndividualConsigneeId

	DELETE
	FROM  IndividualConsignee
	WHERE Id = @IndividualConsigneeId

	DELETE
	FROM AspNetUsers
	WHERE Id = @UserId
END
