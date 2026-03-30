# UPORABITE .NET 9.0 namesto 7.0!
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Razvoj2.csproj", "./"]
RUN dotnet restore "Razvoj2.csproj"

COPY . .
WORKDIR "/src"
RUN dotnet build "Razvoj2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Razvoj2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Razvoj2.dll"]