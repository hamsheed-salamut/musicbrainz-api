FROM mcr.microsoft.com/dotnet/sdk:6.0.401-alpine3.16 AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

# copy everything else and build
COPY ./sql/MusicBrainz.SqlServer.Migrations ./MusicBrainz.SqlServer.Migrations
WORKDIR /MusicBrainz.SqlServer.Migrations/
RUN dotnet restore MusicBrainz.SqlServer.Migrations.csproj
RUN dotnet publish -c $BUILDCONFIG -o out /p:Version=$VERSION

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0.9-alpine3.16
WORKDIR /app
COPY --from=build /MusicBrainz.SqlServer.Migrations/out ./

# should run deployment and exit
ENTRYPOINT ["dotnet", "MusicBrainz.SqlServer.Migrations.dll"]