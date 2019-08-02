FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy and restore as distinct layers
COPY *.sln ./
COPY ./src/Weatherman.App/*.csproj ./src/Weatherman.App/
COPY ./src/Weatherman.Data/*.csproj ./src/Weatherman.Data/
COPY ./src/Weatherman.Discord/*.csproj ./src/Weatherman.Discord/
COPY ./src/Weatherman.Domain/*.csproj ./src/Weatherman.Domain/
COPY ./src/Weatherman.WebUI/*.csproj ./src/Weatherman.WebUI/

RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2

# Copy the app
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

# Start the app
ENTRYPOINT dotnet Weatherman.Discord.dll