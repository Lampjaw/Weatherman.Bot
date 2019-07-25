pushd %~dp0\src\Weatherman.Data
set ASPNETCORE_ENVIRONMENT=Development
dotnet ef database update -v ^
    -c Weatherman.Data.WeathermanDbContext
popd