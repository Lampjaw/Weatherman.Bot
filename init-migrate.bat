pushd %~dp0\src\Weatherman.Data
set ASPNETCORE_ENVIRONMENT=Development
dotnet ef migrations add weathermandbcontext.release.1 -v ^
    -c Weatherman.Data.WeathermanDbContext ^
    -o ./Migrations
popd