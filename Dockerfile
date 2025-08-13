#Build stage → compiles and publishes your ASP.NET Core app.

# ----- Build Stage -----
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./ 
RUN dotnet publish -c Release -o /app/publish


#Run stage → takes the compiled output and actually runs it inside a lightweight runtime container.
# ----- Runtime Stage -----
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Copy only the published output from build stage
COPY --from=build /app/publish .

# Expose port
EXPOSE 5000

# Set environment variables for Kestrel
ENV ASPNETCORE_URLS=http://+:5000

# Run the application
ENTRYPOINT ["dotnet", "WebApplication1.dll"]