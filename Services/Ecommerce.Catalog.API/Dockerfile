#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Ecommerce.Catalog.API/Ecommerce.Catalog.API.csproj", "Services/Ecommerce.Catalog.API/"]
COPY ["Shared/Ecommerce.Shared/Ecommerce.Shared.csproj", "Shared/Ecommerce.Shared/"]
RUN dotnet restore "Services/Ecommerce.Catalog.API/Ecommerce.Catalog.API.csproj"
COPY . .
WORKDIR "/src/Services/Ecommerce.Catalog.API"
RUN dotnet build "Ecommerce.Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Catalog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Catalog.API.dll"]