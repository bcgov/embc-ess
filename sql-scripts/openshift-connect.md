# OpenShift Connection

To apply these scripts one needs a point to run the scripts. You will need a MS SQL client. We will reference Azure Data Studio but realistically all of this could be done with another Microsoft SQL Client.

Creating a port to map to:

From OpenShift web interface `Copy Login Command` from the button in the top right corner. 
Paste it into bash and hit enter to log in. 
Switch your environment in bash to the one you intend to modify using the command `oc project jhnamn-foo`. 
Get pod ids using `oc get pods`. Find the pod name that looks similar to `mssql-00-foo` and save it for the next step.
Create a local port to connect your client to using the command `oc port-forward $PODNAME $LOCALPORTNUMBER:1433`. Port 1433 is the port that MS SQL uses by default you can map to other ports so that you can have multiple configurations that don't overlap in your SQL client. The command should look like `oc port-forward mssql-00-foo 1433:1433`.
Open your MS SQL client (Azure Data Studio) and add a  new connection.

|Key|Value|
|--|--|
|Connection Type|Microsoft SQL Server|
|Server|127.0.0.1 or 127.0.0.1,$LOCALPORTNUMBER|
|Port|1433 or ''|
|Authentication type|SQL Login|
|Username|sa|
|Password|found in the secrets in openshift|
