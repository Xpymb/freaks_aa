ARG SOLUTION_IMAGE_NAME=ghcr.io/xpymb/freaks_aa/solution-build-artifacts:1.0.0
ARG ASPNET_VERSION=10.0-alpine3.22
FROM $SOLUTION_IMAGE_NAME AS builder

ARG ASPNET_VERSION
FROM mcr.microsoft.com/dotnet/aspnet:$ASPNET_VERSION

WORKDIR /webapi

COPY --from=builder /src/Services/Portal/Freaks.Portal.WebApi/bin/Release/net10.0/publish/ .

ENTRYPOINT ["dotnet", "Freaks.Portal.WebApi.dll"]