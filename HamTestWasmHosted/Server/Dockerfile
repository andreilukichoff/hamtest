#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HamTestWasmHosted/Server/HamTestWasmHosted.Server.csproj", "HamTestWasmHosted/Server/"]
COPY ["HamTestWasmHosted/Client/HamTestWasmHosted.Client.csproj", "HamTestWasmHosted/Client/"]
COPY ["HamTestWasmHosted/Shared/HamTestWasmHosted.Shared.csproj", "HamTestWasmHosted/Shared/"]
RUN dotnet restore "HamTestWasmHosted/Server/HamTestWasmHosted.Server.csproj"
COPY . .
WORKDIR "/src/HamTestWasmHosted/Server"
RUN dotnet build "HamTestWasmHosted.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HamTestWasmHosted.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HamTestWasmHosted.Server.dll"]