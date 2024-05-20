dotnet ef migrations add InitialMigration -s Core.API -p Core.Infrastructure -v
dotnet ef database update -s Core.API -p Core.Infrastructure -v

dotnet ef database update 0 -s Core.API -p Core.Infrastructure -v
dotnet ef  migrations remove -s Core.API -p Core.Infrastructure -v