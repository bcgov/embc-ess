#!/bin/bash
API_URL="${1:-http://localhost:3000}"
# API_URL="${1:-http://pdfprint-develop-jhnamn.pathfinder.gov.bc.ca}"

curl_time() {
    curl -w "\
   namelookup:  %{time_namelookup}s\n\
      connect:  %{time_connect}s\n\
   appconnect:  %{time_appconnect}s\n\
  pretransfer:  %{time_pretransfer}s\n\
     redirect:  %{time_redirect}s\n\
starttransfer:  %{time_starttransfer}s\n\
-------------------------\n\
        total:  %{time_total}s\n" "$@"
}

pushd "$( dirname "${BASH_SOURCE[0]}" )"
curl_time -s -X POST -d @sample.html -OJ -H "Expect:" -H "Content-Type: text/html" "${API_URL}/pdf?filename=referral_sample.pdf&v=2"
popd

