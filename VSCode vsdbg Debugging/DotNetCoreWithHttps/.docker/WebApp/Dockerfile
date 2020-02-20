# First stage build, responsible for
# building the application
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
ARG buildconfig
ARG pathtoproject

# Make the directories
# to copy over the files
RUN mkdir -p /app

# Copy the directories and files
WORKDIR /app
COPY . .

# Publish the projects
WORKDIR /app/${pathtoproject}
RUN dotnet restore
RUN if [ "${buildconfig}" = "Debug" ]; then \
        dotnet publish -o /publish -c Debug --no-restore; \
    else \
        dotnet publish -o /publish -c Release --no-restore; \
    fi

# Self signed certificate for HTTPS support. See:
#   https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1
# Make the directory to store the self-generated certificate
ARG autoselfsigncert
ARG autoselfsigncertpassword
RUN mkdir -p /https
RUN if [ "${autoselfsigncert}" != "" ]; then \
        dotnet dev-certs https -ep /https/aspnetapp.pfx -p "${autoselfsigncertpassword}"; \
    fi

# Second stage build, responsible for
# running the application. More lightweight
# compared to the first stage build.
# Uses the runtime image, opposed to the SDK image.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ARG buildconfig
ARG csprojbasename
ARG entrypointfile
ARG launchapp
ENV APPLICATION_DDL_FILE=${csprojbasename}.dll
ENV LAUNCH_APP=${launchapp}
WORKDIR /publish
# Copy from the previous image the
# published files to the current
# image to run the application
COPY --from=build-env /publish .
# Install vsdbg tool for debugging
RUN if [ "${buildconfig}" = "Debug" ]; then \
        # Updates apt-get package manager
        apt-get update && \
        apt-get install -y --no-install-recommends apt-utils && \
        # Installs curl and unzip so vsdbg can be downloaded
        # and installed. The procps package gives the 'ps' command
        # to list processes. This is used when our debugger
        # scans the availables processes to attach itselve into.
        apt-get install curl unzip procps -y && \
        curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /publish/vsdbg; \
     else \
        echo "*Whistling*"; \
    fi

# Self signed certificate for HTTPS support. See:
#   https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1
# Conditionally set the environment variables which configures
# the app to use the HTTPS & the certificate.
ARG usehttpsendpoint
ARG autoselfsigncert
ARG autoselfsigncertpassword
ARG NEW_VALUE=${usehttpsendpoint:+"https://+;http://+"}
ENV ASPNETCORE_URLS=${NEW_VALUE:-${ASPNETCORE_URLS}}
ARG NEW_VALUE=${autoselfsigncert:+${autoselfsigncertpassword}}
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=${NEW_VALUE:-${ASPNETCORE_Kestrel__Certificates__Default__Password}}
ARG NEW_VALUE=${autoselfsigncert:+"/https/aspnetapp.pfx"}
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=${NEW_VALUE:-${ASPNETCORE_Kestrel__Certificates__Default__Path}}
COPY --from=build-env /https /https
EXPOSE 443

# Set entrypoint so that
# when containers are up,
# the web server is started
COPY ${entrypointfile} /publish/docker-entrypoint.sh
RUN chmod +x /publish/docker-entrypoint.sh
ENTRYPOINT ["/publish/docker-entrypoint.sh"]