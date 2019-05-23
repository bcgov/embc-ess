--Will delete all regisration data 
BEGIN TRANSACTION
-- Delete all addresses
DELETE FROM EvacueeRegistrationAddresses
FROM EvacueeRegistrationAddresses a
INNER JOIN
	EvacueeRegistrations d
	ON a.RegistrationId = d.EssFileNumber

-- Delete all people recorded
DELETE FROM ReferralEvacuees 
FROM
	ReferralEvacuees re
INNER JOIN
	EvacueeRegistrations d
	ON re.RegistrationId = d.EssFileNumber

DELETE FROM Evacuees
FROM 
	Evacuees e
INNER JOIN
	EvacueeRegistrations d
	ON e.RegistrationId = d.EssFileNumber

DELETE FROM	Referrals
FROM
	Referrals r
INNER JOIN
	EvacueeRegistrations d
	ON r.RegistrationId = d.EssFileNumber

DELETE FROM EvacueeRegistrations 

--ROLLBACK;
COMMIT TRANSACTION