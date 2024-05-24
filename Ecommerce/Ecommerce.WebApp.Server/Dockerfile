#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ecommerce/Ecommerce.WebApp.Client/nuget.config", "Ecommerce/Ecommerce.WebApp.Client/"]
COPY ["Ecommerce/Ecommerce.WebApp.Server/Ecommerce.WebApp.Server.csproj", "Ecommerce/Ecommerce.WebApp.Server/"]
COPY ["Ecommerce/Ecommerce.Infrastructure.Identity/Ecommerce.Infrastructure.Identity.csproj", "Ecommerce/Ecommerce.Infrastructure.Identity/"]
COPY ["Ecommerce/Ecommerce.Infrastructure.Shared/Ecommerce.Infrastructure.Shared.csproj", "Ecommerce/Ecommerce.Infrastructure.Shared/"]
COPY ["Ecommerce/Ecommerce.Application/Ecommerce.Application.csproj", "Ecommerce/Ecommerce.Application/"]
COPY ["Ecommerce/Ecommerce.Domain/Ecommerce.Domain.csproj", "Ecommerce/Ecommerce.Domain/"]
COPY ["Ecommerce/Ecommerce.Infrastructure.Persistence/Ecommerce.Infrastructure.Persistence.csproj", "Ecommerce/Ecommerce.Infrastructure.Persistence/"]
COPY ["Ecommerce/Ecommerce.WebApp.Client/Ecommerce.WebApp.Client.esproj", "Ecommerce/Ecommerce.WebApp.Client/"]
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash -
RUN apt install -y nodejs
RUN dotnet restore "./Ecommerce/Ecommerce.WebApp.Server/Ecommerce.WebApp.Server.csproj"
COPY . .
WORKDIR "/src/Ecommerce/Ecommerce.WebApp.Server"
RUN dotnet build "./Ecommerce.WebApp.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ecommerce.WebApp.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.WebApp.Server.dll"]