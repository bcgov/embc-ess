# PDF Service

The purpose of the PDF microservice is to generate PDF documents based on templates and JSON format data.  No database connection is used to generate the PDF.

## Development

Strategy: To run the development environment you will need docker desktop installed. The docker container will run locally and watch the `dist` folder. So to make changes in the `dist` folder we need to re-transpile the typescript in the project into javascript in the dist folder. To do that, we set our local version of node to watch for changes in the typescript files. That means, you will need a local version of node and a docker container running node. The docker container is built in the same way as the production version that will eventually run in OpenShift.

## Testing

TODO:

### Curl

TODO:

### Triggering the PDF export

TODO:
