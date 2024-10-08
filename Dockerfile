# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyAspNetProject.csproj", "./"]
RUN dotnet restore "./MyAspNetProject.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MyAspNetProject.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "MyAspNetProject.csproj" -c Release -o /app/publish

# Use the runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyAspNetProject.dll"]
