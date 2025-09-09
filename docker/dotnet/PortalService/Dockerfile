ARG SOLUTION_IMAGE_NAME
ARG ASPNET_VERSION
FROM $SOLUTION_IMAGE_NAME as builder

ARG ASPNET_VERSION
FROM mcr.microsoft.com/dotnet/aspnet:$ASPNET_VERSION

WORKDIR /webapi

COPY --from=builder /src/Services/Portal/Freaks.Portal.WebApi/bin/Release/net9.0/publish/ .

ENTRYPOINT ["dotnet", "Freaks.Portal.WebApi.dll"]