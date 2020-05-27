#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["FamilySys/FamilySys.csproj", "FamilySys/"]
RUN dotnet restore "FamilySys/FamilySys.csproj"
COPY . .
WORKDIR "/src/FamilySys"
RUN dotnet build "FamilySys.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FamilySys.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FamilySys.dll"]