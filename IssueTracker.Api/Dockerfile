FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5237

ENV ASPNETCORE_URLS=http://+:5237

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["IssueTracker.Api/IssueTracker.csproj", "IssueTracker.Api/"]
RUN dotnet restore "IssueTracker.Api/IssueTracker.csproj"
COPY . .
WORKDIR "/src/IssueTracker.Api"
RUN dotnet build "IssueTracker.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "IssueTracker.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IssueTracker.dll"]
