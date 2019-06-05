# Dockerfile for a database backup pod.
FROM openshift/base-centos7
copy backup.sh /

RUN chmod -R a+rwx /backup.sh

CMD /backup.sh

# Install Microsoft SQL Server Command Line Tools (sqlcmd/bcp)
FROM mcr.microsoft.com/mssql-tools