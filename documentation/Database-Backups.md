# Performing Manual Database Backups and Restores

## Backup Database
1. Connect to Database Pod
2. Open Data management tool to the table such as Sql Server Management Studio (SSMS) or Azure Data Studio (ADS)
3. On SSMS   ```Right-Click on Database and select **Tasks** -> **Backup Database** 
    ![smss-backup](./resources/ssms-backup.png) 
    ```Ensure **Database** is selected as a <u>Backup component:</u>
    ```Select <u>Destination</u> as a <u>Back up to:</u> **Disk** and click **Add**
    ![smss-backup](./resources/ssms-backup-2.png) 
    ```In the popup, enter the <i>file name</i>, then click **Ok**```Then click **Ok** on the main <i>Backup Up Database</i> window
    <br />   
    On ADS
    ```Right-Click on Database and select **Backup**
    ![smss-backup](./resources/ads-backup.png) 
    ```Enter a <i>Backup Name</i>
    ```Remove the current backup file by selecting and clicking "-", then click "+" to enter a new specific backup file
    ```Click **Backup** button
    ![smss-backup](./resources/ads-backup-2.png) 

## Restore Database
1. Connect to Database Pod
2. Open Data management tool to the table such as Sql Server Management Server (SSMS) or Azure Data Studio (ADS)
3. On SSMS
    ```Right click on Database and select **Tasks** -> **Restore** -> **Database**
    ![smss-backup](./resources/ssms-restore.png) 
    ```On the **General** page select **Device**, then click '...' button
    ```Click the **Add** button and enter a file name
    ![smss-backup](./resources/ssms-restore-2.png) 
    ```Next click the **Options** Page
    ```Ensure that **Overwrite the existing database** is selected <u>Restore Options</u>
    ```As well, under <u>Server Connections</u>, select **Close existing connections to the database**
    ```Click **Ok** and then **Ok** on the the main restore window
    ![smss-backup](./resources/ssms-restore-3.png) 

   On ADS
        ```Right-Click on Database and select **Restore**
        ![smss-backup](./resources/ads-restore.png) 
        ```Ensure <i>Backup file</i> is selected under **Source**
        ```Click the "..." button to select the backup file
        ```Next click the **Options** Page
        ```Ensure that **Overwrite the existing database** is selected under <u>Restore Options</u>
        ```As well, under <u>Server Connections</u>, select **Close existing connections to the database**
        ```Click the **Restore** button
        ![smss-backup](./resources/ads-restore-2.png) 
