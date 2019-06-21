-- Enter the GUID root for a range of Incident Tasks and their associated records

DECLARE @IncidentTaskId varchar(36) = '' --A34592F5-C12D-4F61-0CF9-08D6B3D424B5

BEGIN TRANSACTION

DELETE FROM EvacueeRegistrationAddresses
FROM EvacueeRegistrationAddresses a
INNER JOIN
	EvacueeRegistrations d
	ON a.RegistrationId = d.EssFileNumber
WHERE 
    d.IncidentTaskId = @IncidentTaskId;

-- Delete all people recorded
DELETE FROM ReferralEvacuees 
FROM
	ReferralEvacuees re
INNER JOIN
	EvacueeRegistrations d
	ON re.RegistrationId = d.EssFileNumber
WHERE 
    d.IncidentTaskId = @IncidentTaskId;

DELETE FROM Evacuees
FROM 
	Evacuees e
INNER JOIN
	EvacueeRegistrations d
	ON e.RegistrationId = d.EssFileNumber
WHERE 
    d.IncidentTaskId = @IncidentTaskId;

DELETE FROM	Referrals
FROM
	Referrals r
INNER JOIN
	EvacueeRegistrations d
	ON r.RegistrationId = d.EssFileNumber
WHERE 
    d.IncidentTaskId = @IncidentTaskId;

DELETE FROM EvacueeRegistrations 
WHERE 
    IncidentTaskId = @IncidentTaskId;

DELETE FROM IncidentTasks
FROM IncidentTasks i
WHERE
	Id = @IncidentTaskId

ROLLBACK;
--COMMIT TRANSACTION