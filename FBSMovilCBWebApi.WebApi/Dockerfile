FROM mcr.microsoft.com/dotnet/core/sdk:latest AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . ./app
RUN dotnet restore ./app/FBSMovilCBWebApi.WebApi/FBSMovilCBWebApi.WebApi.csproj

# Copy everything else and build
RUN dotnet publish ./app/FBSMovilCBWebApi.WebApi/FBSMovilCBWebApi.WebApi.csproj -c Release -o /app/out
# RUN dotnet ef database update --project FBSMovilCBWebApi.WebApi/

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:latest
WORKDIR /app
COPY --from=build-env /app/out .
#ENTRYPOINT ["dotnet", "--launch-profile", "Production", "FBSMovilCBWebApi.WebApi.dll"]
ENTRYPOINT ["dotnet","FBSMovilCBWebApi.WebApi.dll"]

