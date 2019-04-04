-- Enter the GUID root for a range of Organization and their associated records

DECLARE @OrganizationIdRoot varchar(16) = '08D6B6EB9A3B' --010D1F87-7566-40CE-F3F7-08D6B6EB9A3B

DECLARE @OrganizationsToDrop TABLE(OrganizationId uniqueidentifier)
DECLARE @VolunteersToDrop TABLE(PeopleId uniqueidentifier)
DECLARE @RegistrationsToDrop TABLE(RegistrationId uniqueidentifier)
DECLARE @HeadsOfHouseholds TABLE(PeopleId uniqueidentifier)
DECLARE @PeopleToDrop TABLE(PeopleId uniqueidentifier)
DECLARE @AddressesToDrop TABLE(AddressId uniqueidentifier)

--Obtain the Organizations
INSERT INTO @OrganizationsToDrop
SELECT o.Id
FROM
	Organizations o
WHERE
	o.Id LIKE '%' + @OrganizationIdRoot

INSERT INTO @VolunteersToDrop
SELECT i.Id
FROM
	People i
INNER JOIN
	@OrganizationsToDrop o
	ON i.OrganizationId = o.OrganizationId

--Obtain the registrations
INSERT INTO @RegistrationsToDrop
SELECT r.Id
FROM
	Registrations r
INNER JOIN
	@VolunteersToDrop i
	ON r.CompletedById = i.PeopleId

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

DELETE FROM People
FROM People p
INNER JOIN
	@VolunteersToDrop d
	ON p.Id = d.PeopleId

DELETE FROM Organizations
FROM Organizations o
INNER JOIN @OrganizationsToDrop d
	ON o.Id = d.OrganizationId

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

ROLLBACK;
--COMMIT TRANSACTION