#!/bin/bash
# mssql automated backup script
# See README.md for documentation on this script

FINAL_BACKUP_DIR=$BACKUP_DIR"`date +\%Y-\%m-\%d`/"
DBFILE=$FINAL_BACKUP_DIR"$MSSQL_DATABASE`date +\%Y-\%m-\%d-%H-%M`.bak"
echo "Making backup directory in $FINAL_BACKUP_DIR"
 
if ! mkdir -p $FINAL_BACKUP_DIR; then
	echo "Cannot create backup directory in $FINAL_BACKUP_DIR." 1>&2
	exit 1;
fi;

DB_ADMIN_PASSWORD=$DB_ADMIN_PASSWORD
export DB_ADMIN_PASSWORD

echo "Backing up $MSSQL_DATABASE on $DATABASE_SERVICE_URI"
sqlcmd -S $DATABASE_SERVICE_URI -U sa -P $DB_ADMIN_PASSWORD -d master -Q "BACKUP DATABASE $MSSQL_DATABASE TO DISK='$DBFILE'"

if test -f "$DBFILE" ; then
	echo "Database backup written to $DBFILE"

	echo "Will now gzip $DBFILE"
	gzip $DBFILE

	echo "Database backup written compressed to $DBFILE.gz"

	if test -f "$DBFILE"; then
		rm -d $DBFILE
	fi;

	# first cull backups older. 
	echo "Culling backups older than $DAILY_BACKUPS"
	find $BACKUP_DIR* -type d -ctime +$DAILY_BACKUPS | xargs rm -rf
	exit 0;
else
	echo "[!!ERROR!!] Failed to backup database $MSSQL_DATABASE"
	exit 1;
fi;

