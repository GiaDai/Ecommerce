dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context IdentityContext

dotnet ef dbcontext list
dotnet ef migrations add InitialCreate --context ApplicationDbContext
dotnet ef migrations add InitialCreate --context IdentityContext

docker build -f Ecommerce/Ecommerce.WebApp.Server/Dockerfile -t be.ecommerce:v.0.1 .

dotnet ef migrations add InitialIdentiy --context IdentityContext -o ../Ecommerce.Infrastructure.Identity/Migrations
dotnet ef migrations add InitialProductTable --context ApplicationDbContext

docker network create -d bridge ecommerce
docker network connect ecommerce sqlserver

docker-compose -f docker-compose-postgres.yml up

git config core.ignorecase true
git config --global core.ignorecase true

p, SuperAdmin, roleclaims, list
p, SuperAdmin, roleclaims, create
p, SuperAdmin, roleclaims, show
p, SuperAdmin, roleclaims, edit
p, SuperAdmin, roleclaims, delete

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=sql@pa22w0rd" -p 1433:1433 -d --name=sqlserver mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04

git rm --cached Ecommerce/Ecommerce.WebApp.Client/Ecommerce.WebApp.Client.esproj
git rm --cached ./Ecommerce/Ecommerce.WebApp.Client/Ecommerce.WebApp.Client.esproj
