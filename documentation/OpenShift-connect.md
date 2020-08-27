# OpenShift Connection

To apply scripts in the /sql-scripts/ directory you need a database access point to run the scripts. The general idea is to port-forward the ms-sql port locally so that you can access the DB with a MS SQL client. This guide was made with Azure Data Studio in mind but realistically all of it could be done with another Microsoft SQL Client.

## Creating a port to map to

- From OpenShift web interface `Copy Login Command` from the button in the top right corner. 
- Paste the login command into bash and hit enter to log in. 
- Switch your openshift environment in bash to the one you intend to modify using the command `oc project jhnamn-foo`. 
- Get pod ids using `oc get pods`. Find the pod name that looks similar to `mssql-00-foo` and save it for the next step.
- Create a local port to connect your client to using the command:

    ```oc port-forward $PODNAME $LOCALPORTNUMBER:1433```

    Port `1433` is the port that MS SQL uses by default. You can map your local port to other more distinctive ports so that you can have multiple configurations that don't overlap in your SQL client. The command should look like `oc port-forward mssql-00-foo 1433:1433`.

## Configure your MS SQL client

Open your MS SQL client (Azure Data Studio) and add a  new connection. MSSQL takes the port number as `127.0.0.1,1433` with a comma seperating the port number.

| Key | Value |
|--|--|
| Connection Type | Microsoft SQL Server |
| Server | 127.0.0.1 |
| Port | 1433 |
| Authentication type | SQL Login |
| Username | sa |
| Password | found in the secrets in openshift |
