#!/bin/bash
# Postgresql automated backup script
# See README.md for documentation on this script

while true; do

#FINAL_BACKUP_DIR=$BACKUP_DIR"`date +\%Y-\%m-\%d`/"
#DBFILE=$FINAL_BACKUP_DIR"$MSSQL_DATABASE`date +\%Y-\%m-\%d-%H-%M`"
#echo "Making backup directory in $FINAL_BACKUP_DIR"
 
#if ! mkdir -p $FINAL_BACKUP_DIR; then
#	echo "Cannot create backup directory in $FINAL_BACKUP_DIR." 1>&2
#	exit 1;
#fi;

DB_ADMIN_PASSWORD=$DB_ADMIN_PASSWORD
export DB_ADMIN_PASSWORD

sqlcmd -s $MSSQL_DATABASE -U sa -P $DB_ADMIN_PASSWORD -d master -q "BACKUP DATABASE ess-develop TO DISK='/var/opt/mssql/data/var/opt/mssql/data/ess-develop-zip-test.bak'"

#if ! /opt/rh/rh-postgresql94/root/usr/bin/pg_dump -Fp -h "$DATABASE_SERVICE_NAME" -U "sa" "$MSSQL_DATABASE" | gzip > $DBFILE.sql.gz.in_progress; then
#	echo "[!!ERROR!!] Failed to backup database $MSSQL_DATABASE" 
#else
#	mv $DBFILE.sql.gz.in_progress $DBFILE.sql.gz
#	echo "Database backup written to $DBFILE.sql.gz"
	
	# first cull backups older than 10 days.  (SB-331)
#	find $BACKUP_DIR* -type d -ctime +31 | xargs rm -rf
#fi;

# 24 hrs
sleep 1d

done