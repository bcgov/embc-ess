FROM mcr.microsoft.com/dotnet/core/sdk:2.1

# install vsdbg debugger into our container
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl unzip \
    && rm -rf /var/lib/apt/lists/* \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

# required inside Docker, otherwise file-change events may not trigger
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:5000

# make sure we skip the Angular front-end build inside Docker
ENV SKIP_ANGULAR_BUILD=true

WORKDIR /app

# copy build dependencies
COPY embc-interfaces/BCeID/. embc-interfaces/BCeID/

WORKDIR /app/embc-app

# copy everything else and build app
COPY embc-app/. .

ENTRYPOINT dotnet watch run
