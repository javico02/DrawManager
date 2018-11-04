# Runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app

# Building image
FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src

# Restoring
COPY src/DrawManager.Api/DrawManager.Api.csproj DrawManager.Api/
RUN dotnet restore "DrawManager.Api/DrawManager.Api.csproj"

# Building
COPY src/DrawManager.Api/. DrawManager.Api/
RUN dotnet build "DrawManager.Api/DrawManager.Api.csproj" -c Release -o /app

# Publishing
FROM build AS publish
RUN dotnet publish "DrawManager.Api/DrawManager.Api.csproj" -c Release -o /dist

# Copying to runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /dist .

# Creating db folder for the correct .sqlite file creation.
# https://stackoverflow.com/questions/44232390/sqlite-error-14-unable-to-open-database-file-with-ef-core-code-first
WORKDIR /app/db

# Going up to /app folder again
WORKDIR ..

# Execute
ENTRYPOINT ["dotnet", "DrawManager.Api.dll"]