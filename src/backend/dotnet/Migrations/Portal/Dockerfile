ARG DOTNET_SDK_VERSION
# сборка бандла
FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_SDK_VERSION AS build
WORKDIR /src
COPY ./../../ .
RUN dotnet tool restore && \
    dotnet ef migrations bundle \
      --project Services/Portal/Dal/Freaks.Portal.Dal.Persistence/Freaks.Portal.Dal.Persistence.csproj \
      --startup-project Services/Portal/Freaks.Portal.WebApi/Freaks.Portal.WebApi.csproj \
      --context PortalDbContext \
      --runtime linux-x64 --self-contained \
      --output /out/efbundle

# лёгкий рантайм для self-contained бинаря
FROM mcr.microsoft.com/dotnet/runtime-deps:$DOTNET_SDK_VERSION
WORKDIR /app
COPY --from=build /out/efbundle /app/efbundle
ENTRYPOINT ["/app/efbundle"]