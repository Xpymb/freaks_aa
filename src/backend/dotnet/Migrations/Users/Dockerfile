ARG DOTNET_SDK_VERSION=9.0-alpine3.22
FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_SDK_VERSION AS build
WORKDIR /src

ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install -g dotnet-ef

ENV ASPNETCORE_ENVIRONMENT=Staging

COPY . .
RUN dotnet restore Shared/Users/Freaks.Users.Dal/Freaks.Users.Dal.csproj
RUN dotnet ef migrations bundle \
      --project Shared/Users/Freaks.Users.Dal/Freaks.Users.Dal.csproj \
      --context UserDbContext \
      --runtime linux-x64 --self-contained \
      --output /out/efbundle

FROM mcr.microsoft.com/dotnet/runtime-deps:$DOTNET_SDK_VERSION
WORKDIR /app
COPY --from=build /out/efbundle /app/efbundle
ENTRYPOINT ["/app/efbundle"]
