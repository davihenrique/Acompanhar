#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
# Program.cs escuta na porta definida pela variavel PORT
ENV PORT=8081
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AcompanharApp/AcompanharApp.csproj", "AcompanharApp/"]
RUN dotnet restore "AcompanharApp/AcompanharApp.csproj"
COPY . .
WORKDIR "/src/AcompanharApp"
RUN dotnet build "AcompanharApp.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AcompanharApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcompanharApp.dll"]
