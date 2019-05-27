# PDF Service

The purpose of the PDF microservice is to generate PDF documents based on templates and JSON format data.  No database connection is used to generate the PDF. Send your file with embedded content and get the pdf back.

## Development

Strategy: To run the development environment you will need docker desktop installed. The docker container will run locally and watch the `dist` folder. So to make changes in the `dist` folder we need to re-transpile the typescript in the project into javascript in the dist folder. To do that, we set our local version of node to watch for changes in the typescript files. That means, you will need a local version of node and a docker container running node. The docker container is built in the same way as the production version that will eventually run in OpenShift.

Begin development by installing nodejs and docker desktop. In the root of the project folder perform an `npm install` to build your node modules folder which stops VS Code from complaining about missing dependencies and enables linting and hinting while you write your typescript.

Launch the docker container that watches for changes in the `dist` folder with the command `npm run docker-debug`. This will compose a docker container that begins watching. The docker output is an important shell window to keep open because it will show you when there is a major problem with the code in the container. The microservice will be available on `localhost:3000`. You will test against this port.

In another shell start watching your typescript for changes using `npm run watch`. It will watch for your changes and output fresh js files into the dist folder.

## Testing

In the test folder there is a shell script that can be run `test.sh`. This requires bash and curl to use. Upon running the script, it posts sample.html to the printer service running in docker and it outputs to `sample.pdf` in the test directory so you can see the results of the print.

### Curl

For information on curl look at the man pages or examine the curl command in the `test.sh` file.

### Triggering the PDF export

There are two basic routes in the microservice. 

`localhost:3000/health` is a keep-alive `GET` endpoint for OpenShift. If this stops responding OpenShift will restart the pod.

`localhost:3000/pdf` is a `POST` endpoint that gets a `text/html` file (with inline embedded content) and it returns a pdf.
