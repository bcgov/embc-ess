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
The `backup.sh` shell script is comprised of an ubuntu and a MS Sql Server Tools layer.  It essentially will execute a `BACKUP` transact sql command on the database assigned to the 

## Cron Job Yaml
