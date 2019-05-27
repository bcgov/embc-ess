#!/bin/bash

#remove all untagged images and stopped containers
docker ps -aqf status=exited | xargs docker rm
docker images -qf dangling=true | xargs docker rmi
