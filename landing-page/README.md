# EMBC ESS Landing page

## Structure

/html - the folder to contain all html and resource of the landing page

/conf.d - nginx configuration files

## Build

A build definifion in Pathfinder tools project uses the official nginx image and the current folder to create an image in 'embcess-landing-page' imagestream.

## Deployment

The build will create :latest tag that triggers automatic deployment to Pathfinder rtest project (embcess-landing-page service).

To deploy to production project (embcess-landing-page service), a manual tag :prod needs to be added by using the command

`oc tag embcess-landing-page:latest embcess-landing-page:prod`

Changing the :prod tag will automatically deploy the production environment.
