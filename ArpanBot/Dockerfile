FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app



# Copy csproj and restore as distinct layers
COPY *.csproj ./

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80
ENV ASPNETCORE_URLS=http://*:80

# Get from Azure
ARG DISCORD_API_KEY_ARG

# Set environment variables
ENV DISCORD_API_KEY=$DISCORD_API_KEY_ARG

# install image manipulation packages
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*

ENTRYPOINT ["dotnet", "ArpanBot.dll"]
