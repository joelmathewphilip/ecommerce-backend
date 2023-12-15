
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSe 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/Shared/Ecommerce.Shared/Ecommerce.Shared.csproj", "src/Shared/Ecommerce.Shared/"]
COPY ["src/Services/Ecommerce.Account.API/Ecommerce.Account.API.csproj", "src/Services/Ecommerce.Account.API/"]


RUN dotnet restore "src/Services/Ecommerce.Account.API/Ecommerce.Account.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Ecommerce.Account.API"
RUN dotnet build "Ecommerce.Account.API.csproj" -c Release -o /app/account/build --no-cache

FROM build AS publish
RUN dotnet publish "Ecommerce.Account.API.csproj" -c Release -o /app/account/publish --no-cache

FROM base AS final
WORKDIR /app
COPY --from=publish /app/account/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Account.API.dll","--server.urls","http://+:80;https://+:443"]