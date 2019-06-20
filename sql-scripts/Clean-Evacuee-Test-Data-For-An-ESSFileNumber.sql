--Will delete all regisration data for below ess file number
DECLARE @EssFileNumber INT = 0;

BEGIN TRANSACTION
-- Delete all addresses
DELETE FROM EvacueeRegistrationAddresses
FROM EvacueeRegistrationAddresses a
INNER JOIN
	EvacueeRegistrations d
	ON a.RegistrationId = d.EssFileNumber
WHERE 
    a.RegistrationId = @EssFileNumber;

-- Delete all people recorded
DELETE FROM ReferralEvacuees 
FROM
	ReferralEvacuees re
INNER JOIN
	EvacueeRegistrations d
	ON re.RegistrationId = d.EssFileNumber
WHERE 
    re.RegistrationId = @EssFileNumber;

DELETE FROM Evacuees
FROM 
	Evacuees e
INNER JOIN
	EvacueeRegistrations d
	ON e.RegistrationId = d.EssFileNumber
WHERE 
    e.RegistrationId = @EssFileNumber;

DELETE FROM	Referrals
FROM
	Referrals r
INNER JOIN
	EvacueeRegistrations d
	ON r.RegistrationId = d.EssFileNumber
WHERE 
    r.RegistrationId = @EssFileNumber;

DELETE FROM EvacueeRegistrations 
WHERE 
    EssFileNumber = @EssFileNumber;

ROLLBACK;
--COMMIT TRANSACTION