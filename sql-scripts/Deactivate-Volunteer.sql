-- Will deactivate a Volunteer for the ID below
DECLARE @VolunteerId INT = 0;

BEGIN TRANSACTION

UPDATE Volunteers
SET
    Active = 0
WHERE
    Id = @VolunteerId

ROLLBACK
--COMMIT TRANSACTION