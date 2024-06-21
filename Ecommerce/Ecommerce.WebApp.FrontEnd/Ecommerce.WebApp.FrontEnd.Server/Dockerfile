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
COPY ["Ecommerce/Ecommerce.WebApp.FrontEnd/ecommerce.webapp.frontend.client/nuget.config", "Ecommerce/Ecommerce.WebApp.FrontEnd/ecommerce.webapp.frontend.client/"]
COPY ["Ecommerce/Ecommerce.WebApp.FrontEnd/Ecommerce.WebApp.FrontEnd.Server/Ecommerce.WebApp.FrontEnd.Server.csproj", "Ecommerce/Ecommerce.WebApp.FrontEnd/Ecommerce.WebApp.FrontEnd.Server/"]
COPY ["Ecommerce/Ecommerce.WebApp.FrontEnd/ecommerce.webapp.frontend.client/ecommerce.webapp.frontend.client.esproj", "Ecommerce/Ecommerce.WebApp.FrontEnd/ecommerce.webapp.frontend.client/"]
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash -
RUN apt install -y nodejs

RUN dotnet restore "./Ecommerce/Ecommerce.WebApp.FrontEnd/Ecommerce.WebApp.FrontEnd.Server/Ecommerce.WebApp.FrontEnd.Server.csproj"
COPY . .
WORKDIR "/src/Ecommerce/Ecommerce.WebApp.FrontEnd/Ecommerce.WebApp.FrontEnd.Server"
RUN dotnet build "./Ecommerce.WebApp.FrontEnd.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ecommerce.WebApp.FrontEnd.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.WebApp.FrontEnd.Server.dll"]