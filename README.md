# Onion Architecture In ASP.NET Core With CQRS

https://craftbakery.dev/make-your-own-custom-netcore-template/

# Install template

dotnet new -i Ecommerce.Template\

# Uninstall template

dotnet new -u Ecommerce.Template\

# Create a project using the template

dotnet new onion-clean -n Ecommerce -au "ToanLe" -d "The ecommerce project for business" -y 2024

# Run docker sql server as command below

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=sql@pa22w0rd" -p 1433:1433 -d --name=sqlserver mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04

# Create policy.csv file in Ecommerce/Ecommerce.WebApp.Server/wwwroot/policy.csv

Add default row as below
p, SuperAdmin, users, list
p, SuperAdmin, users, create
p, SuperAdmin, roleclaims, list
p, SuperAdmin, roleclaims, create
p, SuperAdmin, roleclaims, show
p, SuperAdmin, roleclaims, edit
p, SuperAdmin, roleclaims, delete

# Run dotnet run in Ecommerce.WebApp.Server

# Access https://localhost:5173/ on browser

# Login with username:superadmin@gmail.com and password:123Pa$$word!
