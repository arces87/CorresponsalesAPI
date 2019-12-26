FROM mcr.microsoft.com/mssql/server:2017-latest

# Install Unzip
RUN apt-get update \
    && apt-get install unzip -y

# Install SQLPackage for Linux and make it executable
#RUN wget -progress=bar:force -q -O sqlpackage.zip https://go.microsoft.com/fwlink/?linkid=2108814 \
#    && unzip -qq sqlpackage.zip -d /opt/sqlpackage \
#    && chmod +x /opt/sqlpackage/sqlpackage

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app
COPY ./FBSConsolaCBBaseDatos/. /usr/src/app
RUN ls /usr/src/app

# Grant permissions for the import-data script to be executable
RUN chmod +x /usr/src/app/import-data.sh


CMD /bin/bash ./entrypoint.sh