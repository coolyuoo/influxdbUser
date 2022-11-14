#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["influxdbUser/influxdbUser.csproj", "influxdbUser/"]
RUN dotnet restore "influxdbUser/influxdbUser.csproj"
COPY . .
WORKDIR "/src/influxdbUser"
RUN dotnet build "influxdbUser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "influxdbUser.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "influxdbUser.dll"]