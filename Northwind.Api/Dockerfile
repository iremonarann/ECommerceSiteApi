#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Northwind.Api/Northwind.Api.csproj", "Northwind.Api/"]
COPY ["Northwind.Business/Northwind.Business.csproj", "Northwind.Business/"]
RUN dotnet restore "Northwind.Api/Northwind.Api.csproj"
COPY . .
WORKDIR "/src/Northwind.Api"
RUN dotnet build "Northwind.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Northwind.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Northwind.Api.dll"]