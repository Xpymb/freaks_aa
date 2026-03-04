ARG DOTNET_SDK_VERSION=10.0-alpine3.22
FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_SDK_VERSION AS build
WORKDIR /src

ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install -g dotnet-ef

ENV ASPNETCORE_ENVIRONMENT=Staging

COPY . .
RUN dotnet restore Services/Portal/Dal/Freaks.Portal.Dal.Persistence/Freaks.Portal.Dal.Persistence.csproj
RUN dotnet build Services/Portal/Dal/Freaks.Portal.Dal.Persistence/Freaks.Portal.Dal.Persistence.csproj --no-restore
RUN dotnet ef migrations bundle \
      --project Services/Portal/Dal/Freaks.Portal.Dal.Persistence/Freaks.Portal.Dal.Persistence.csproj \
      --context PortalDbContext \
      --output /out/efbundle

FROM mcr.microsoft.com/dotnet/runtime:$DOTNET_SDK_VERSION
WORKDIR /app
COPY --from=build /out/efbundle /app/efbundle
ENTRYPOINT ["/app/efbundle"]
