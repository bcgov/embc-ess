# JAG EMBC-ESS

Emergency Management BC - Emergency Social Services Modernization

## Run SQL Server container images with Docker

The following instructions provide details on how to deploy the project using [Docker compose](https://docs.docker.com/compose/install/#install-compose). 

All application services are exposed to the host so they may be easily accessed individually for development and testing purposes.

:warning: WARNING - This method of deployment is intended for <u>local development only</u>. It is **NOT** intended to be supported in production level deployments where security, availability, resilience, and data integrity are important.

#### Prerequisites

- Docker Engine 1.8+ on any supported Linux distribution or Docker for Mac/Windows. 
  - For more information, see [Install Docker](https://docs.docker.com/engine/installation/).
- Minimum of 2 GB of disk space.
- Minimum of 2 GB of RAM.
- [System requirements for SQL Server on Linux](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup?view=sql-server-linux-ver15#system).

## Starting the database container

```
./start.sh
```

## Stopping the database container

```
./stop.sh
```

## Change the SA password

The **SA** account is a system administrator on the SQL Server instance that gets created during setup. After creating your SQL Server container, the `SA_PASSWORD` environment variable you specified is discoverable by running `echo $SA_PASSWORD` in the container. 

For security purposes, change your SA password.

```sh
docker exec -it embc-app-database /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U SA -P '<YourStrong!Passw0rd>' \
   -Q 'ALTER LOGIN SA WITH PASSWORD="<YourNewStrong!Passw0rd>"'
```

:bulb: NOTE - The default **SA** password for the local **embc-app-database** container can be found within the `docker-compose.yml` file.

## Troubleshooting

To verify which containers are running:

```sh
# List the currently running containers.
docker ps

# List all containers.
docker ps -a
```

You should see output similar to the following screenshot:

![Docker ps command output](https://docs.microsoft.com/en-us/sql/linux/media/sql-server-linux-setup-docker/docker-ps-command.png?view=sql-server-linux-ver15)

If the **STATUS** column shows a status of **Up**, then SQL Server is running in the container and listening on the port specified in the **PORTS** column. 

If the **STATUS** column for your SQL Server container shows **Exited**, see the [Troubleshooting section of the configuration guide (Microsoft website)](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-configure-docker?view=sql-server-linux-ver15#troubleshooting).

To launch an interactive shell on the database container:

```sh
docker exec -it embc-app-database bash
```

> This will launch a bash shell that is running within the container, allowing you to inspect the internal files and folders within the container.

## Connect to SQL Server

The following steps use the SQL Server command-line tool, **sqlcmd**, inside the container to connect to SQL Server.

To launch an interactive shell on the database container:

```sh
docker exec -it embc-app-database bash
```

Once inside the container, connect locally with sqlcmd. Sqlcmd is not in the path by default, so you have to specify the full path.

```sh
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P '<YourNewStrong!Passw0rd>'
```

If successful, you should get to a **sqlcmd** command prompt: `1>`.

## Connect from outside the container

The following steps use **sqlcmd** outside of your container to connect to SQL Server running in the container. These steps assume that you already have the [SQL Server command-line tools](<https://docs.microsoft.com/en-us/sql/tools/sqlcmd-utility?view=sql-server-2017>) installed outside of your container. 

:bulb: NOTE - The same principles apply when using other tools, but the process of connecting is unique to each tool.

1. Find the IP address for the machine that hosts your container. On Linux, use **ifconfig** or **ip addr**. On Windows, use **ipconfig**.

2. Run **sqlcmd** specifying the IP address and the port mapped to port 1433 in your container. In this example, that is the same port, 1433, on the host machine. If you specified a different mapped port on the host machine, you would use it here.

   ```sh
   sqlcmd -S localhost -U SA -P '<YourNewStrong!Passw0rd>'
   ```

3. Run Transact-SQL commands. When finished, type `QUIT`.

