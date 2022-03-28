@ECHO off
cls

ECHO Generating deploy
ECHO.

cd FBSConsolaCBWebApi.WebApi
dotnet build -c Release
dotnet publish -c Release

cd ..

cd FBSMovilCBWebApi.WebApi
dotnet build -c Release
dotnet publish -c Release

ECHO Fineshed
pause > nul