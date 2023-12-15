
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSe 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/Shared/Ecommerce.Shared/Ecommerce.Shared.csproj", "src/Shared/Ecommerce.Shared/"]
COPY ["src/Services/Ecommerce.Catalog.API/Ecommerce.Catalog.API.csproj", "src/Services/Ecommerce.Catalog.API/"]


RUN dotnet restore "src/Services/Ecommerce.Catalog.API/Ecommerce.Catalog.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Ecommerce.Catalog.API"
RUN dotnet build "Ecommerce.Catalog.API.csproj" -c Release -o /app/build --no-cache

FROM build AS publish
RUN dotnet publish "Ecommerce.Catalog.API.csproj" -c Release -o /app/publish --no-cache

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Catalog.API.dll","--server.urls","http://+:80;https://+:443"]