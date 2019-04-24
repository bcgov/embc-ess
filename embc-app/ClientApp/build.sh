#!/bin/bash

case $1 in
  --prep)
    echo
    echo "---> Building ESSPREP"
    npm run buildprep -- --prod
    ;;
  *)
    echo
    echo "---> Building EMBCESS"
    npm run buildprod -- --prod
    ;;
esac
