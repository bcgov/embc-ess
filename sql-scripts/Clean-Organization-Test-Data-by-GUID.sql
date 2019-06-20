-- Enter the GUID root for of an Organization and their associated records

DECLARE @OrganizationId varchar(36) = '' --010D1F87-7566-40CE-F3F7-08D6B6EB9A3B

BEGIN TRANSACTION

-- Delete all people recorded
DELETE FROM 
	Volunteers 
WHERE
	OrganizationId = @OrganizationId

DELETE FROM Organizations
FROM Organizations o
WHERE 
	o.Id = @OrganizationId

ROLLBACK;
--COMMIT TRANSACTION