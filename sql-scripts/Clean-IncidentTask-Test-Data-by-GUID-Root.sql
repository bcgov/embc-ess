-- Enter the GUID root for a range of Incident Tasks and their associated records

DECLARE @IncidentTaskRoot varchar(16) = '08D6B3D424B5' --A34592F5-C12D-4F61-0CF9-08D6B3D424B5

DECLARE @IncidentsToDrop TABLE(IncidentTaskId uniqueidentifier)
DECLARE @RegistrationsToDrop TABLE(RegistrationId uniqueidentifier)
DECLARE @HeadsOfHouseholds TABLE(PeopleId uniqueidentifier)
DECLARE @PeopleToDrop TABLE(PeopleId uniqueidentifier)
DECLARE @AddressesToDrop TABLE(AddressId uniqueidentifier)

--Obtain the IncidentTasks
INSERT INTO @IncidentsToDrop
SELECT i.Id
FROM
	IncidentTasks i
WHERE
	i.Id LIKE '%' + @IncidentTaskRoot

--Obtain the registrations
INSERT INTO @RegistrationsToDrop
SELECT r.Id
FROM
	Registrations r
INNER JOIN
	@IncidentsToDrop i
	ON r.IncidentTaskId = i.IncidentTaskId

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

DELETE FROM IncidentTasks
FROM IncidentTasks i
INNER JOIN
	@IncidentsToDrop d
	ON i.Id = d.IncidentTaskId

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