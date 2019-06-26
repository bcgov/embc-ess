# SQL Scripts

All the following scripts are used to manage data within the MS SQL Database for embc-ess.

They all follow a structure of having a variable at the top, which filters the target data.  All are within a transaction with the default final statement set as `ROLLBACK` with `COMMIT TRANSACTION` commented out.  Just a precaution and a quick way to test the script before executing permanently.

<u>IMPORTANT NOTE: Backup up the target database first as a precaution</u>, which you can read how to do at [Database-Backups](../documentation/Database-Backups.md)

### To Deactivate a Volunteer

#### Deactivate-Volunteer.sql

The script `Deactivate-Volunteer.sql` will set the Volunteer.Active field to 0 with the matching Id entered in the `VoluteerId` variable in the script:


## Test Data Cleaning Scripts

### Delete all Test Data for Evacuee Registrations
##### Clean-Evacuee-Test-Data.sql

The script `Clean-Evacuee-Test-Data.sql` will remove all the data from the following set of tables


| Tables Cleaned |
|----------- |
| Evacuee Registrations |
| Evacuees |
| EvacueeRegistrationAddresses |
| Referrals |
| ReferralEvacuees |

### Delete all Test Data before a Date
#### Clean-Evacuee-Test-Data-by-RegistrationDate.sql

The script `Clean-Evacuee-Test-Data-by-RegistrationDate.sql` will remove all the data from the following set of tables
created before date entered for the variable:<br/>
`DECLARE @RegistrationDate DATETIME = '2019-04-03' --yyyy-mm-dd`

| Tables Cleaned |
|----------- |
| Evacuee Registrations |
| Evacuees |
| EvacueeRegistrationAddresses |
| Referrals |
| ReferralEvacuees |

### Delete all test data for an ESS File Number
#### Clean-Evacuee-Test-Data-For-An-ESSFileNumber.sql

The script `Clean-Evacuee-Test-Data-For-An-ESSFileNumber.sql` will remove all the data from the following set of tables
for the records associated with the ESS File Number entered in the script variable:<br/>
`DECLARE @EssFileNumber INT = 0;  -- Replace 0 with a valid ESS File Number`

| Tables Cleaned |
|----------- |
| Evacuee Registrations |
| Evacuees |
| EvacueeRegistrationAddresses |
| Referrals |
| ReferralEvacuees |

### Delete all Incident Tasks and associated data
#### Clean-IncidentTask-Test-Data-by-GUID-Root.sql

The script `Clean-IncidentTask-Test-Data-by-GUID-Root.sql` is used to remove an Incident Task and associated data from the tables below for
the GUID Id entered for `IncidentTaskId`:<br />
`DECLARE @IncidentTaskId varchar(36) = '' --A34592F5-C12D-4F61-0CF9-08D6B3D424B5`

| Tables Cleaned |
|----------- |
| IncidentTasks |
| Evacuee Registrations |
| Evacuees |
| EvacueeRegistrationAddresses |
| Referrals |
| ReferralEvacuees |

### Delete all Organizations and associated data
#### Clean-Organization-Test-Data-by-GUID-Root.sql

The script `Clean-Organization-Test-Data-by-GUID-Root.sql` will delete an Organization and all associated Volunteer records for the Organization Id entered in the variable `OrganizationId` in the script: <br />

`DECLARE @OrganizationId varchar(36) = '' --010D1F87-7566-40CE-F3F7-08D6B6EB9A3B`

| Tables Cleaned |
|----------- |
| Organizations |
| Volunteers |