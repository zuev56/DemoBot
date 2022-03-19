1. PS> dotnet ef migrations add InitialDemoBotContext --context DemoBotContext --output-dir "Data\Migrations"
2. PS> dotnet ef database update --context DemoBotContext


// ADD Microsoft.EntityFrameworkCore.Design
// THEN dotnet tool update --global dotnet-ef --version 6.0.0
//   OR dotnet tool install --global dotnet-ef  (dotnet tool update --global dotnet-ef)








