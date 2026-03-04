ARG DOTNET_SDK_VERSION=10.0-alpine3.22
FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_SDK_VERSION

WORKDIR /src

COPY . .

RUN dotnet restore --verbosity m ./Freaks.slnx

RUN dotnet publish --no-restore ./Freaks.slnx --configuration Release -p:Version=1.0.0 -maxcpucount:1
