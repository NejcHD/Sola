# 1. FAZA: Osnova za poganjanje (Runtime)
# Uporabimo ASP.NET 9.0 sliko, ki je optimizirana za produkcijo
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
# Docker bo poslušal na portu 80 znotraj vsebnika
EXPOSE 80
EXPOSE 443

# 2. FAZA: Gradnja (Build)
# Potrebujemo celoten SDK 9.0, da lahko prevedemo kodo
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Kopiramo projektno datoteko in obnovimo pakete (NuGet restore)
COPY ["RanljivostiSpletneStrani.csproj", "."]
RUN dotnet restore "./RanljivostiSpletneStrani.csproj"

# Kopiramo vse ostale datoteke (vključno z baza.db, če je v tej mapi)
COPY . .

# Prevedemo aplikacijo
RUN dotnet build "RanljivostiSpletneStrani.csproj" -c Release -o /app/build

# 3. FAZA: Objavljanje (Publish)
FROM build AS publish
RUN dotnet publish "RanljivostiSpletneStrani.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. FAZA: Končna slika (Final)
FROM base AS final
WORKDIR /app
# Iz faze 'publish' skopiramo samo končne datoteke v mapo /app
COPY --from=publish /app/publish .

# KLJUČNA VRSTICA: Ukaz, ki zažene aplikacijo ob startu vsebnika
ENTRYPOINT ["dotnet", "RanljivostiSpletneStrani.dll"]