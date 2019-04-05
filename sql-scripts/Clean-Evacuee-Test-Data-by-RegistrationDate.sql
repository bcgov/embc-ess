--Will delete all regisration data prior to the set date below
DECLARE @RegistrationDate DATETIME = '2019-04-03' --yyyy-mm-dd

DECLARE @RegistrationsToDrop TABLE(RegistrationId uniqueidentifier)
DECLARE @HeadsOfHouseholds TABLE(PeopleId uniqueidentifier)
DECLARE @PeopleToDrop TABLE(PeopleId uniqueidentifier)
DECLARE @AddressesToDrop TABLE(AddressId uniqueidentifier)

--Obtain the registrations
INSERT INTO @registrationsToDrop
SELECT r.Id
FROM
	Registrations r
WHERE
 -- assuming we are deleting registrations prior to a date
	(r.SelfRegisteredDate IS NOT NULL AND r.SelfRegisteredDate <  @RegistrationDate)
OR
	(r.RegistrationCompletionDate IS NOT NULL AND r.RegistrationCompletionDate < @RegistrationDate)

--Obtain the Head of House Hold
INSERT INTO @HeadsOfHouseholds
SELECT p.Id
FROM
	People p
INNER JOIN
	Registrations r
	ON p.Id = r.HeadOfHouseholdId
INNER JOIN
	@registrationsToDrop d
	ON r.Id = d.RegistrationId

--Obtain all people with matching head of household
INSERT INTO @PeopleToDrop
SELECT p.Id
FROM 
	People p
INNER JOIN
	@HeadsOfHouseholds h
	ON p.HeadOfHouseholdId = h.PeopleId
UNION
SELECT h.PeopleId
FROM
	@HeadsOfHouseholds h

--Obtain all Primary/Mailing Residences Address
INSERT INTO @AddressesToDrop 
SELECT a.Id
FROM
	Addresses A
INNER JOIN
	People p
	ON a.Id = p.PrimaryResidenceId OR a.Id = p.MailingAddressId
INNER JOIN
	@PeopleToDrop d
	ON p.Id = d.PeopleId

BEGIN TRANSACTION
-- Delete to drop registrations
DELETE FROM Registrations
FROM Registrations r
INNER JOIN
	@RegistrationsToDrop d
	ON r.Id = d.RegistrationId

-- Delete all people recorded
DELETE FROM People
FROM 
	People p
INNER JOIN
	@PeopleToDrop d
	ON p.Id = d.PeopleId

-- Delete all addresses
DELETE FROM Addresses 
FROM 
	Addresses a
INNER JOIN
	@AddressesToDrop d
	ON a.Id = d.AddressId

--ROLLBACK;
COMMIT TRANSACTION