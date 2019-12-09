# Getting runtime image and naming it as Phase base
FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base

# Creating and positiong over this path inside the runtime image
WORKDIR /app

# Getting building image and naming it as Phase build
FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build

# Creating and positioning over this path inside the building image
# C:\src\DrawManager.Api\ is the path inside the image
WORKDIR /src/DrawManager.Api

# Copiying .csproj file from host folder into image positioned folder
COPY DrawManager.Api.csproj ./

# Restoring inside the image
RUN dotnet restore "DrawManager.Api.csproj"

# Copying all files from host folder into image positioned folder
COPY /. ./

# Building inside the image
# Building result will be copied to C:\app\ folder
RUN dotnet build "DrawManager.Api.csproj" -c Release -o /app

# Naming build phase as publish phase
FROM build AS publish

# Publishing inside the image
# Publishing source will be into the C:\src\DrawManager.Api\bin\Release\netcoreapp2.1\ folder
# Publishing result will be copied into C:\dist\ folder
RUN dotnet publish "DrawManager.Api.csproj" -c Release -o /dist

# # Naming base phase as final phase
FROM base AS final

# Creating and positiong over this path inside the image
WORKDIR /app

# Copying publishing result from building image, phase publish, folder C:\dist\, to runtime image, phase final, into the folder C:\app\
COPY --from=publish /dist .

# Creating db folder for the correct .sqlite file creation.
# https://stackoverflow.com/questions/44232390/sqlite-error-14-unable-to-open-database-file-with-ef-core-code-first
WORKDIR /app/db

# Going up to /app folder again
WORKDIR ..

# Execute
ENTRYPOINT ["dotnet", "DrawManager.Api.dll"]