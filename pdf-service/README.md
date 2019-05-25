# PDF Service

The purpose of the PDF microservice is to generate PDF documents based on templates and JSON format data.  No database connection is used to generate the PDF.

## Development

Strategy: To run the development environment you will need docker desktop installed. The docker container will run locally and watch the `dist` folder. So to make changes in the `dist` folder we need to re-transpile the typescript in the project into javascript in the dist folder. To do that, we set our local version of node to watch for changes in the typescript files. That means, you will need a local version of node and a docker container running node. The docker container is built in the same way as the production version that will eventually run in OpenShift.

Begin development by installing nodejs and docker desktop. In the root of the project folder perform an `npm install` to build your node modules folder which stops VS Code from complaining about missing dependencies and enables linting and hinting while you write your typescript.

Launch the docker container that watches for changes in the `dist` folder with the command `npm run docker-debug`. This will compose a docker container that begins watching. The docker output is an important shell window to keep open because it will show you when there is a major problem with the code in the container.

In another shell start watching your typescript for changes using `npm run watch`. It will watch for your changes and output fresh js files into the dist folder.

## Testing

TODO:

### Curl

TODO:

### Triggering the PDF export

TODO:
