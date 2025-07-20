ARG DOCKER_REPOSITORY
ARG APP_VERSION
ARG ASPNET_VERSION
FROM $DOCKER_REPOSITORY/dotnet-solution-build:$APP_VERSION as builder

ARG ASPNET_VERSION
FROM mcr.microsoft.com/dotnet/aspnet:$ASPNET_VERSION

WORKDIR /webapi

COPY --from=builder /src/Freaks.Portal.WebApi/Source/bin/Release/net9.0/publish/ .

ENTRYPOINT ["dotnet", "Freaks.Portal.WebApi.dll"]