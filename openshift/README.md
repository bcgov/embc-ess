JAG EMBCESS
=================

## Running in OpenShift

This project uses the scripts found in [openshift-project-tools](https://github.com/BCDevOps/openshift-project-tools) to setup and maintain OpenShift environments (both local and hosted).  Refer to the [OpenShift Scripts](https://github.com/BCDevOps/openshift-project-tools/blob/master/bin/README.md) documentation for details.

**These scripts are designed to be run on the command line (using Git Bash for example) in the root `openshift` directory of your project's source code.**

### Special notes for Non BC Government Production OpenShift deployments

If you are not using the BC Government Pathfinder OpenShift instance, you may need to import image streams for RHEL Dotnet Core.  A file "imagestreams.json" has been provided in the "other-templates" folder that contains a definition for these image streams.

This does not apply if you are doing local testing with OpenShift Origin or Minishift; if you are using OpenShift Origin or Minishift, follow the instructions below for the use of Centos as a source image on a local build.

## Running in a Local OpenShift Cluster

At times running in a local cluster is a little different than running in the production cluster.

Differences can include:
* Resource settings.
* Available image runtimes.
* Source repositories (such as your development repo).
* Etc.

To target a different repo and branch, create a `settings.local.sh` file in your project's local `openshift` directory and override the GIT parameters, for example;
```
export GIT_URI="https://github.com/bcgov/jag-embcess.git"
export GIT_REF="openshift-updates"
```

Then run the following command from the project's local `openshift` directory:
```
genParams.sh -l
```

**Git Bash Note:  Ensure that you do not have a linux "oc" binary on your path if using Git Bash on a Windows PC to run the scripts.  A windows "oc.exe" binary will work fine.

This will generate local settings files for all of the builds, deployments, and Jenkins pipelines.
The settings in these files will be specific to your local configuration and will be applied when you run the `genBuilds.sh` or `genDepls.sh` scripts with the `-l` switch.

### Important Local Configuration 

Before you deploy your local build configurations ...

The application uses .Net 2.0 s2i images for the builds.  In the pathfinder environment these components utilize the `dotnet-21-rhel7` image which is available at registry.access.redhat.com/dotnet/dotnet-21-rhel7.  

You can use the following command to import this image into your OpenShift project:

`oc import-image dotnet-21-rhel7 --from registry.access.redhat.com/dotnet/dotnet-21-rhel7:latest --confirm`

### Preparing for local deployment

1. Install the oc cli.  
2. Start an OpenShift cluster.  If you are using Docker you can use the `oc-cluster-up.sh` scripts from [openshift-project-tools](https://github.com/BCDevOps/openshift-project-tools).
3. Run `generateLocalProjects.sh` to create the projects in your local cluster.
4. Run `initOSProjects.sh` to update the deployment permissions on the projects.
5. To fix the routes in your local deployment environments use the `updateRoutes.sh` script.

From here on out the commands used to deploy and management the application configurations are basically the same for a local cluster as they are for the Pathfinder environment.

## Deploying your project

All of the commands listed in the following sections must be run from the root `openshift` directory of your project's source code.

### Before you begin ...

If you are updating an existing environment you will need to be conscious of retaining access to the existing data in the given environment.  User accounts, database names, and database credentials can all be affected.  The processes affecting them should be reviewed and understood before proceeding.

For example, the process of deploying and managing database credentials has changed.  The process has moved to using shared secretes that are mounted as environment variables, where previously they were provisioned as discrete environment variables in each component's environment.  Further the new deployment process, by default, will create a random set of credentials for each deployment or update (a new set every time you run `genDepls.sh`).  Being that the credentials are shared, there is a single source and place that needs to be updated.  You simply need to ensure the credentials are updated to the values expected by the pre-configured environment if needed.

### Initialization

If you are working with a new set of OpenShift projects, or you have run a `oc delete all --all` to start over, run the `initOSProjects.sh` script, this will repair the cluster file system services (in the Pathfinder environment), and ensure the deployment environments have the correct permissions to deploy images from the tools project.

### Generating the Builds, Images and Pipelines in the Tools Project

Run;
```
genBuilds.sh
```
, and follow the instructions.

Note that the script will stop mid-way through. Ensure builds are complete in the tools project. Also, embcess may hang without error. This is likely due to insufficient resources in your local.

IMPORTANT: Sometimes embcess will fail while trying to build its image. The error will say that npm install failed. This is due to npm timing out. To fix this resources must be increased. Try increasing requested CPU to 2 cores and RAM to 2 GB.

All of the builds should start automatically as their dependencies are available, starting with builds with only docker image and source dependencies.

The process of deploying the Jenkins pipelines will automatically provision a Jenkins instance if one does not already exist.  This makes it easy to start fresh; you can simply delete the existing instance along with it's associated PVC, and fresh instances will be provisioned.


### Generate the Deployment Configurations and Deploy the Components

Run;
```
genDepls.sh -e <environmentName/>
```
, and follow the instructions to deploy the application components to the desired environments.


### Wire up your Jenkins Pipelines

When the Jenkins Pipelines were provisioned when your ran `genBuilds.sh` web-hook URLs and secrets were generated automatically.  To trigger the pipelines via GIT commits, register the URL(s) for the pipelines with GIT.

Copy and paste the pipeline's web-hook url into the Payload URL of the GIT web-hook (it comes complete with the secret).

Set the content type to **application/json**

Select **Just the push event**

Check **Active**

Click **Add webhook**

