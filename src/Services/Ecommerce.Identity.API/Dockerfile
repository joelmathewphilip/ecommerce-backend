#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Ecommerce.Identity.API/Ecommerce.Identity.API.csproj", "src/Services/Ecommerce.Identity.API/"]
COPY ["src/Shared/Ecommerce.Shared/Ecommerce.Shared.csproj", "src/Shared/Ecommerce.Shared/"]
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