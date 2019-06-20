--Will delete all regisration data prior created prior to the set date below using the CreatedDateTime field as the
DECLARE @RegistrationDate DATETIME = '2019-04-03' --yyyy-mm-dd

BEGIN TRANSACTION
-- Delete all addresses
DELETE FROM EvacueeRegistrationAddresses
FROM EvacueeRegistrationAddresses a
INNER JOIN
	EvacueeRegistrations d
	ON a.RegistrationId = d.EssFileNumber
WHERE 
    a.CreatedDateTime < @RegistrationDate;

-- Delete all people recorded
DELETE FROM ReferralEvacuees 
FROM
	ReferralEvacuees re
INNER JOIN
	EvacueeRegistrations d
	ON re.RegistrationId = d.EssFileNumber
WHERE 
    d.CreatedDateTime < @RegistrationDate;;

DELETE FROM Evacuees
FROM 
	Evacuees e
INNER JOIN
	EvacueeRegistrations d
	ON e.RegistrationId = d.EssFileNumber
WHERE 
    e.CreatedDateTime < @RegistrationDate;;

DELETE FROM	Referrals
FROM
	Referrals r
INNER JOIN
	EvacueeRegistrations d
	ON r.RegistrationId = d.EssFileNumber
WHERE 
    r.CreatedDateTime < @RegistrationDate;

DELETE FROM EvacueeRegistrations 
WHERE 
    CreatedDateTime < @RegistrationDate;

ROLLBACK;
--COMMIT TRANSACTION