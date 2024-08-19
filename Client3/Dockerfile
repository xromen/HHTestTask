#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Client3/Client3.csproj", "Client3/"]
RUN dotnet restore "Client3/Client3.csproj"
COPY . .
WORKDIR "/src/Client3"
RUN dotnet build "Client3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Client3.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Client3.dll"]