#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["HamTestWasm.csproj", "HamTestWasm/"]
#RUN dotnet restore "HamTestWasm/HamTestWasm.csproj"
#COPY . .
#WORKDIR "/src/HamTestWasm"
#RUN dotnet build "HamTestWasm.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "HamTestWasm.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "HamTestWasm.dll"]
#

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY HamTestWasm.csproj .
RUN dotnet restore "HamTestWasm.csproj"
COPY . .
RUN dotnet build "HamTestWasm.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HamTestWasm.csproj" -c Release -o /app/publish
#RUN ["ls","-la","/app/publish"]

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
RUN cat /etc/nginx/nginx.conf
COPY nginx.conf /etc/nginx/nginx.conf
RUN cat /etc/nginx/nginx.conf

#/src/bin/Release/net5.0/wwwroot