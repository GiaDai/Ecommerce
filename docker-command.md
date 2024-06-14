dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context IdentityContext

dotnet ef dbcontext list
dotnet ef migrations add InitialCreate --context ApplicationDbContext
dotnet ef migrations add InitialCreate --context IdentityContext

docker build -f Ecommerce/Ecommerce.WebApp.Server/Dockerfile -t onion.clean:v.0.1 .

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

docker run -d --name rabbitmq --network ecommerce -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=ecommerce -e RABBITMQ_DEFAULT_PASS=123456789 -e RABBITMQ_MEMORY_HIGH_WATERMARK=0.7 -e RABBITMQ_DISK_FREE_LIMIT=500MB -e RABBITMQ_CONSUMER_TIMEOUT=300000 --cpus="2" --memory="2g" rabbitmq:3-management

docker run -d -p 56379:6379 -h redis -e REDIS_PASSWORD=redis --network ecommerce --name redis --restart always redis /bin/sh -c 'redis-server --appendonly yes --requirepass ${REDIS_PASSWORD}'

git rm --cached Ecommerce/Ecommerce.WebApp.Client/Ecommerce.WebApp.Client.esproj
git rm --cached ./Ecommerce/Ecommerce.WebApp.Client/Ecommerce.WebApp.Client.esproj
