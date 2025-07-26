ARG DOCKER_REGISTRY
ARG APP_VERSION
ARG ASPNET_VERSION
FROM $DOCKER_REGISTRY/solution-build-artifacts:$APP_VERSION as builder

ARG ASPNET_VERSION
FROM mcr.microsoft.com/dotnet/aspnet:$ASPNET_VERSION

WORKDIR /webapi

COPY --from=builder /src/Services/Portal/Freaks.Portal.WebApi/bin/Release/net9.0/publish/ .

ENTRYPOINT ["dotnet", "Freaks.Portal.WebApi.dll"]