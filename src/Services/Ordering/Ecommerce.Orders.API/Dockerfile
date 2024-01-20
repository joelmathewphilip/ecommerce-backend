#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Ordering/Ecommerce.Orders.API/Ecommerce.Orders.API.csproj", "src/Services/Ordering/Ecommerce.Orders.API/"]
COPY ["src/Shared/Ecommerce.Shared/Ecommerce.Shared.csproj", "src/Shared/Ecommerce.Shared/"]
COPY ["src/Services/Ordering/Ecommerce.Orders.Application/Ecommerce.Orders.Application.csproj", "src/Services/Ordering/Ecommerce.Orders.Application/"]
COPY ["src/Services/Ordering/Ecommerce.Orders.Domain/Ecommerce.Orders.Domain.csproj", "src/Services/Ordering/Ecommerce.Orders.Domain/"]
COPY ["src/Services/Ordering/Ecommerce.Orders.Infrastructure/Ecommerce.Orders.Infrastructure.csproj", "src/Services/Ordering/Ecommerce.Orders.Infrastructure/"]
RUN dotnet restore "src/Services/Ordering/Ecommerce.Orders.API/Ecommerce.Orders.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Ordering/Ecommerce.Orders.API"
RUN dotnet build "Ecommerce.Orders.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Orders.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Orders.API.dll","--server.urls","http://+:80;https://+:443"]