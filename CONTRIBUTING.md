# Contributing to embc-ess project

To contribute to this project development, you'll need to fork it under your account and submit a PR to the development team.

## development

Submit PRs to `r1develop` branch.

## Patches to production

Submit PRs to `master` branch AND to `r1develop` branch. Ensure both can  be merged to their respective branchs.

# DevOps Process

## Branching strategy

Production branch is [master](https://github.com/bcgov/embc-ess/tree/master)

Development branch and default branch for PRs is [r1develop](https://github.com/bcgov/embc-ess/tree/r1develop)

[develop](https://github.com/bcgov/embc-ess/tree/develop) branch is currently not being used

Forks should submit PRs only to `r1develop` branch. The development team will review and if accepted, will merge the changes into this branch.
Once merged to r1develop, the CI/CD pipeline (based on Openshift Jenkins) will deploy to the dev-r1 environment for testing.

According to release planning done by the development team in coordination with BC government's EMBC team, a stable r1develop branch will be merged into `master`.
A new build will be triggered and deployed to the r1-test environment for EMBC team's review and for regression testing.

Once testing is complete, the already tested image will be tagged for training, and then production for automated deployment to training and production respectively.

## CI/CD pipelines

There are 2 CI/CD pipelines that generate docker images for the project:

### embcess-r1develop

For early stage deployment and testing, used for active development. A merge to `r1develop` will automatically trigger a build, test and deployment to the dev environment.
The new docker images will be pushed to `embcess-r1-develop` image stream and tagged 'latest'.

Image stream: embcess-r1-develop  
Tags: Latest

### embcess-master

Used for deployments to higher environments, release of features and patches. A merge to `master` will automatically trigger build, test and deployment to the test environment.
The new docker images will be pushed to `embcess-r1` image stream and tagged 'test'.

Image stream: embcess-r1  
Tags: Test, Test-Previous, Training, Training-Previous, Prod, Prod-Previous

### embcess-master-training

Manually triggering the training pipeline will tag as `train` the latest `test` labeled image and will automatically trigger deployment to the training environment.

### embcess-master-prod

Manually triggering the training pipeline will tag as `prod` the latest `test` labeled image and will automatically trigger deployment to the training environment.

# Development Standards

## .Net
TBD

## Angular

TBD

## OpenShift

TBD


