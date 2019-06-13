# db-backup
## Sql Server Backup Container Cron Job

This is a very simple container that is deployed via cron job to execute a backup for a database on a MS Sql Server container.

### MS SQL Server Backup Storage
MS SQL Server must have backups provisioned to the host server and not directly mapped to a nfs.  Thus a `/backups/` path on a volume must be added as storage on the MS SQL Server container assigned to a `nfs-backup` as described below.

### Backup Storage Volume
The recommended storage class for the backup volume is `nfs-backup`.  This class of storage **cannot** be auto-provisioned through the use of a deployment template.  The `PersistentVolumeClaim` declared in the supplied deployment template for the *backup volume* will purposely fail to properly provision and wire an `nfs-backup` volume if published before you manually provision your `nfs-backup` claim.

When using `nfs-backup` you will need to provision your claims **before** you publish your deployment configuration, through either the [service catalog](https://github.com/BCDevOps/provision-nfs-apb#provision-via-gui-catalog) using the [BC Gov NFS Storage](https://github.com/BCDevOps/provision-nfs-apb/blob/master/docs/usage-gui.md) wizard, or by using the [svcat cli](https://github.com/BCDevOps/provision-nfs-apb#provision-via-svcat-cli).

You'll note the name of the resulting storage claim has a random component to it (example, `bk-devex-von-bc-tob-test-xjrmkhsnshay`).  This name needs to be injected into the default value of the `BACKUP_VOLUME_NAME` parameter of the template **before** publishing the deployment configuration in order for the storage to be correctly mounted to the `/backups/` directory of the container.

`nfs-backup` storageClass is a lower tier of storage and not considered highly available.  `read: don't use this for live application storage`.  The storageClass **IS** covered by the default enterprise backup policies, and can be directly referenced for restores using the PVC name when opening a restore ticket with 7700.

`nfs-backup` PVCs **cannot** be used for restore/verification.  The permissions on the underlying volume do not allow the PostgreSql server to host it's configuration and data files on a directory backed by this class of storage.

Ensure you review and plan your storage requirements before provisioning.

More information on provisioning `nfs-backup` storage here; [provision-nfs-apb](https://github.com/BCDevOps/provision-nfs-apb)

#### NFS Storage Backup and Retention Policy
NFS backed storage is covered by the following backup and retention policies:
- Backup
  - Daily: Incremental
  - Monthly: Full
- Retention
  - 90 days


## backup.sh
The `backup.sh` shell script is comprised of an ubuntu and a MS Sql Server Tools layer.  It 
essentially will execute a `BACKUP` transact sql command on the database assigned to 
the `MSSQL_DATABASE` and `DATABASE_SERVICE_URI` environment variables.   The 
environment variable `BACKUP_DIR` needs to be set to the same path as the MS Sql Server 
container's backup path `/backups/` as the shell script will create a folder with the current 
date to save the backup files.

The shell script is able to do full backups and log only backups and this is set by the environment 
variable `BACKUP_LOG_ONLY`.

If you wish to deploy and test the container you can set the environment variables directly
using the [db-backup.deploy.json](db-backup.deploy.json).  Before the deployment occurs ensure 
that the project has a build in the `Tools` project, which can be configured by creating
the image using the OpenShift build configuration [db-backup.build.json](db-backup.build.json).

The cron job yaml will set these environment variables when it deploys the image, make
desired settings in the below decsribed yaml.

| Name | Default (if not set) | Purpose |
| ---- | -------------------- | ------- |
| BACKUP_DIR | /backups/ | The directory under which backups will be stored.  The deployment configuration mounts the persistent volume claim to this location when first deployed. |
| DAILY_BACKUPS | 31 | this value is used with the daily backup strategy to set the number of backups to retain before pruning. |
| BACKUP_LOG_ONLY | false | Determines wether a full database backup or a log only database occurs.  'false' will trigger a full database backup. |
| DATABASE_SERVICE_URI  | mssql.host.domain (no default) | The name of the service hostname/uri for the database target. |
| MSSQL_DATABASE  | database-name (no default) | The name of the database target; the name of the database you want to backup. |
| DB_ADMIN_PASSWORD  | *wired to a secret* | The password for the database hosted by the `mssql.host.domain` Postgres server. The deployment configuration makes the assumption you have your database credentials stored in secrets (which you should), and the key for the username is `database-admin-password`.  The name of the secret must be provided as the Database's container secret name to the deployment configuration template. |

## Cron Job
The best use of the db-backup container is to create a 
[OpenShift Cron Job](https://docs.openshift.com/container-platform/3.9/dev_guide/cron_jobs.html) 
via the configuration yaml file [backup-cronjob-full-database.yaml](backup-cronjob-full-database.yaml) or 
[backup-cronjob-log-only.yaml](backup-cronjob-log-only.yaml), which creates a full database backup `ess-db-backup` 
or a log only `ess-db-backup-log-only` backup cron job respectively.  These can be 
viewed in the OpenShift cluster console for each environment project.  The configuration 
also creates corresponding `Config Maps` for each of the Cron Jobs with the names
`ess-db-backup-config` and `ess-db-backup-log-only-config`

When the scheduled cron job executes in pulls the latest tagged image from the tools 
project with the name of 'db-backup' and creates a container, which runs the [backup.sh](backup.sh)
shell script with Environment Variables set to the parameter values in the configuration 
yaml and from the Config map in creates when the yaml deploys.

The full database backup is defaulted to run daily at 13:00 utc and the log only is defaulted to run
once an hour on the hour.  You can see this value set in the parameter `SCHEDULE` of either config file
where the values are set as `0 13 * * *` or `0 * * * *` using `crontab` syntax as explained at 
[https://en.wikipedia.org/wiki/Cron](https://en.wikipedia.org/wiki/Cron).  

### backup-cronjob-[type].yaml Parameters
| Name | Default (if not set) | Purpose |
| ---- | -------------------- | ------- |
| JOB_NAME | ess-db-backup-log-only or ess-db-backup | Name of the Scheduled Job to Create. |
| JOB_PERSISTENT_STORAGE_NAME | not set,<br />example, `bk-devex-von-bc-tob-test-xjrmkhsnshay` | Backup Persistent Storage Name of pre-created PVC to use for the backup |
| SCHEDULE | `0 13 * * *` or<br /> `0 * * * *` | Cron Schedule to Execute the Job (in UTC) |
| SOURCE_IMAGE_NAME | db-backup | The name of the image to use for this resource. |
| IMAGE_NAMESPACE | jhnamn-tools | The namespace of the OpenShift project containing the imagestream for the application. |
| TAG_NAME | latest | Environment TAG Name |
| DATABASE_NAME | database name (no default) | The name of the database. |
| DATABASE_SERVICE_NAME | mssql | Database Deployment Name |
| DATABASE_SERVICE_URI  | mssql.host.domain (no default) | The name of the service hostname/uri for the database target. |
| BACKUP_DIR | /backups/ | The directory under which backups will be stored.  The deployment configuration mounts the persistent volume claim to this location when first deployed. |
| DAILY_BACKUPS | 31 | this value is used with the daily backup strategy to set the number of backups to retain before pruning. |
| BACKUP_LOG_ONLY | false | Determines wether a full database backup or a log only database occurs.  'false' will trigger a full database backup. |
