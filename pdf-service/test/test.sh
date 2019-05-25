#!/bin/bash
pushd "$( dirname "${BASH_SOURCE[0]}" )"
curl -s -X POST -d @sample.html -o sample.pdf -H "Expect:" -H "Content-Type: text/html" http://localhost:3000/pdf?filename=referral_sample.pdf
popd

