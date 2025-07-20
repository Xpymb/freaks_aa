ARG DOTNET_SDK_VERSION
FROM mcr.microsoft.com/dotnet/aspnet:$DOTNET_SDK_VERSION

WORKDIR /src

COPY ./../../src/backend/dotnet .

RUN dotnet restore --verbosity m ./../../src/backend/dotnet/Freaks.sln

RUN dotnet publish --no-restore ./../../src/backend/dotnet/Freaks.sln --configuration Release -p:Version=1.0.0