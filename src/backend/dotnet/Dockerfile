ARG DOTNET_SDK_VERSION=10.0-alpine3.22
FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_SDK_VERSION

WORKDIR /src

COPY . .

RUN dotnet restore --verbosity m ./Freaks.sln

RUN dotnet publish --no-restore ./Freaks.sln --configuration Release -p:Version=1.0.0