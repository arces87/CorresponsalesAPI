FROM nginx:alpine

COPY FBSMovilCBWebApi.WebApi.csproj ./

RUN dotnet restore

WORKDIR /usr/share/nginx/html
COPY dist/ .