#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/WebApi.MusicBrainz/WebApi.MusicBrainz.csproj", "src/WebApi.MusicBrainz/"]
COPY ["src/Core.Common/Core.Common.csproj", "src/Core.Common/"]
COPY ["src/Core.MusicBrainz/Core.MusicBrainz.csproj", "src/Core.MusicBrainz/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/WebApi.MusicBrainz/WebApi.MusicBrainz.csproj"
COPY . .
WORKDIR "/src/src/WebApi.MusicBrainz"
RUN dotnet build "WebApi.MusicBrainz.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.MusicBrainz.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.MusicBrainz.dll"]