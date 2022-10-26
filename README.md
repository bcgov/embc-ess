# EMBC ESS

Emergency Management BC - Emergency Support Services Modernization

ERA portal - This repository contains the code base for self registration and emergency response assistants functions

[![Lifecycle:Retired](https://img.shields.io/badge/Lifecycle-Retired-d45500)](https://github.com/bcgov/repomountie/blob/master/doc/lifecycle-badges.md)

Technology Stack
-----------------

| Layer   | Technology |
| ------- | ------------ |
| Presentation | Angular 7 |
| API and Business Logic | C# - Dotnet Core 2.1 |
| Web Server | Kestrel |
| Data | SQL Server 2017 |
| Runtime | OpenShift |
| Authentication | BC.Gov SSO services |

Knowledge base
-----------------------
Throughout the project there are readme documents. Those specific documents can be browsed from the [knowledgebase.](documentation/index.md)

Installation
------------
This application is meant to be deployed to RedHat OpenShift version 3.11. Full instructions to deploy to OpenShift are in the `openshift` directory.

Developer Prerequisites
-----------------------

**Public Application**

- Visual Studio 2017+ with .net core 2.1 SDK or above
- Sql Server Local DB 2016+
- Node.js version 10 LTS
- Angular 7 CLI


**DevOps**

- RedHat OpenShift tools
- Docker
- A familiarity with Jenkins

## Project Architecture

![architecture-diagram](architecture-diagram.png)

## Files in this repository

```
project
+-- .s2i
|   +-- bin                     (Source to image (s2i) scripts for OpenShift)
+-- embc-app
|  +-- embc-app.sln             (solution file for Visual Studio 2017+)
|  +-- embc-app.csproj          (main application project)
+-- embc-interfaces
|  +-- BCeID 
|     +-- BCeID.csproj          (proxy to consume BCeID web services)
+-- embc-unit-tests
|  +-- embc-unit-tests.csproj
+-- landing-page
|  +-- html                     (html for ESS landing page ess.gov.bc.ca)
+-- openshift
|  +-- other-templates
|  |  +-- sql-server             (sql server templates)
|  +-- templates
|  |  +-- embcess                (main app templates)
|  |  +-- pdf-service            (pdf rendering service templates)
+-- pdf-service                  (pdf rendering service code)
+-- sql-scripts                  (scripts for backend ops on the data)
+-- sql-server                   (SQL Server docker scripts) 
```

Backend Unit Tests
-----------------------
Run `dotnet test` from embc-app folder

Backend local machine debugging
-------------------------------

1. Create or edit embc-app/Properties/launchSettings.json to contain the required environment variables:

```json
{
    "profiles": {
        "embc-app": {
            "commandName": "Project",
            "launchBrowser": false,
            "environmentVariables": {
                "DB_DATABASE": "ESS_Dev",
                "DB_FULL_REFRESH": "true",
                "PDF_SERVICE_NAME": "https://pdfservice-dev.pathfinder.gov.bc.ca",
                "ASPNETCORE_ENVIRONMENT": "Development",
                "CSP_ENABLED": "true",
                "SITEMINDER_LOGOUT_URL": "https://logontest.gov.bc.ca/clp-cgi/logoff.cgi",
                "BASE_URI": "http://localhost:49200",
                "auth:oidc:ClientId": "embcess-app",
                "auth:oidc:ClientSecret": "[client secert from sso-dev.pathfinder.gov.bc.ca]",
                "auth:oidc:MetaDataAddress": "https://sso-dev.pathfinder.gov.bc.ca/auth/realms/udb1ycga/.well-known/openid-configuration",
                "auth:jwt:Audience": "embcess-app",
                "auth:jwt:MetaDataAddress": "https://sso-dev.pathfinder.gov.bc.ca/auth/realms/udb1ycga/.well-known/openid-configuration",
                "auth:jwt:TokenValidationParameters:ValidateLifetime": "false",
                "AUTH_MODE": "KC"
            },
            "applicationUrl": "http://0.0.0.0:49200"
        }
    }
}

```
2. from Visual Studio, run embc-app.csproj
3. run `ng serve' from embc-app/ClientApp folder
4. open a browser to http://localhost:49200
5. to test the API directly, it is recommended to use Postman and use its oauth2 support to obtain the required JWT in order to be authenticated

**Defaults for development environment**
- Angular requests to localhost:4200
- A new database will be recreated every time the project is executed, to prevent this, set `DB_FULL_REFRESH` to `false`
- The default connection string is set to use `(localdb)\MSSQLLocalDB` with integrated authentication, to change the connection string, set `DATABASE_SERVICE_NAME` to the server name.


Environment Variables
---------------------

Before running the API locally, you must set some environment variables:

| Name | Value |
| ---- | ----- |
| BASE_PATH |/embcess|
| BASE_URI |http://localhost|
| APP_ENVIRONMENT_TITLE     |   Banner title (shown on all environments except PROD).   |
| DATABASE_SERVICE_NAME     |   Database service URL                                    |
| DB_DATABASE               |   <database_name>                                         |
| DB_ADMIN_PASSWORD         |   SA (admin) password                                     |
| DB_USER                   |   DB connection credentials                               |
| DB_PASSWORD               |   DB connection credentials                               |
| SMTP_HOST                 |   smtp.youremailserver.com                                |
| SMTP_DEFAULT_SENDER       |   no-reply@youremailserver.com                            |
| PDF_SERVICE_NAME          |   PDF microservice URL                                    |
|AUTH_MODE                  |   KC for KeyCloak  or SM for SiteMinder                   |
|auth:oidc:MetaDataAddress  |   KeyCloak client meta data URL                           | 
|auth:oidc:ClientId         |   KeyCloak client ID                                      |
|auth:oidc:ClientSecret     |   KeyCloak client secert                                  |
|auth:oidc:MetaDataAddress  |   KeyCloak client metadata URL                            | 
|auth:jwt:Audience          |   KeyCloak client ID                                      |
|auth:jwt:MetaDataAddress   |   KeyCloak client metadata URL                            |
|auth:jwt:TokenValidationParameters:ValidateLifetime | false to reuse tokens which expired |
|INTERNAL_NETWORK_ADDRESS   | network subnet address for trusted reverse proxies, must be in the form of IPv6 |
|KEY_RING_DIRECTORY| path to a shared folder for key sharing between running instances|


Authentication
--------------
## Users

Provincial admins can log in using their IDIR accounts

Volunteers and local authority admins can log in using their BCeID accounts


### Environments

| Environment | URL                                    |
| ----------- | -------------------------------------- |
| DEV         | https://embcess-r1-dev.pathfinder.gov.bc.ca  |
| DEV landing page | http://dev.ess.gov.bc.ca |
| TEST        | https://embcess-r1-test.pathfinder.gov.bc.ca |
| TRAINING    | https://embcess-r1-training.pathfinder.gov.bc.ca |
| PROD        | https://embcess-r1.pathfinder.gov.bc.ca |
| PROD landing page | http://ess.gov.bc.ca |


Contribution
------------

Please report any [issues](https://github.com/bcgov/embc-ess/issues).

[Pull requests](https://github.com/bcgov/embc-ess/pulls) are always welcome.

If you would like to contribute, please see our [contributing](CONTRIBUTING.md) guidelines.

Please note that this project is released with a [Contributor Code of Conduct](CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.

License
-------

    Copyright 2019 Province of British Columbia
    
    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at 
    
       http://www.apache.org/licenses/LICENSE-2.0
    
    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.

Maintenance
-----------

This repository is maintained by [BC Attorney General]( https://www2.gov.bc.ca/gov/content/governments/organizational-structure/ministries-organizations/ministries/justice-attorney-general ).
