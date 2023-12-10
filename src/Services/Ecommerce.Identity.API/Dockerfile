

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7013

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Shared/Ecommerce.Shared/Ecommerce.Shared.csproj", "src/Shared/Ecommerce.Shared/"]
COPY ["Ecommerce.Identity.API.csproj", "src/Services/Ecommerce.Identity.API/"]

RUN dotnet restore "src/Services/Ecommerce.Identity.API/Ecommerce.Identity.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Ecommerce.Identity.API"
RUN dotnet build "Ecommerce.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Identity.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Identity.API.dll"]